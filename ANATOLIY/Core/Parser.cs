#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ANATOLIY.Core.Instructions;
using ANATOLIY.Structure;

namespace ANATOLIY.Core
{
    public class ParserResult
    {
        public List<AnatoliyClass> Classes = new();
        public List<Instruction> Instructions = new();
        public List<AnatoliyMethod> Methods = new();
        public List<AnatoliyVariable> Variables = new();
    }

    public class Parser
    {
        public static IReadOnlyDictionary<string, Type> InstructionTypes = new Dictionary<string, Type>
        {
            {"СОЗДАТЬ", typeof(Create)},
            {"КОНЕЦ", typeof(End)},
            {"ПОДКЛЮЧИТЬ", typeof(ImportClass)},
            {"ВЫЗВАТЬ", typeof(Call)}
        };

        public static ParserResult Parse(string file)
        {
            ParserResult result = new();
            var lines = File.ReadAllLines(file);
            for (var index = 0; index < lines.Length; index++)
            {
                var line = lines[index];
                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith("КОММЕНТАРИЙ:")) continue;
                if (InstructionTypes.All(i => !line.StartsWith(i.Key)))
                {
                    Program.WriteError($"\nfile: {file}\nline {index + 1}: unknown instruction.");
                    Environment.Exit(1);
                }
                else
                {
                    var name = InstructionTypes.First(i => line.StartsWith(i.Key));
                    var ins = (Instruction) Activator.CreateInstance(name.Value)!;
                    ins.Parameters = line.Split(" ").Skip(1).ToList();
                    ins.Line = index;
                    ins.PrepareForExecution();
                    var check = ins.CheckValid(line);
                    if (check == "valid")
                    {
                        result.Instructions.Add(ins);
                    }
                    else
                    {
                        Program.WriteError($"\nfile: {file}\nline {index + 1}: {check}");
                        Environment.Exit(1);
                    }
                }
            }

            foreach (var variable in from instruction in result.Instructions.ToList() where instruction is Create && instruction.Parameters[0] == "ПЕРЕМЕННУЮ" select _parseVariable(ref result.Instructions, instruction) into variable where variable != null select variable)
            {
                if (variable != null) result.Variables.Add(variable);
            }
            foreach (var method in from instruction in result.Instructions.ToList()
                where instruction is Create && instruction.Parameters[0] == "МЕТОД"
                select _parseMethod(ref result.Instructions, instruction)
                into method
                where method != null
                select method)
                if (method != null)
                    result.Methods.Add(method);
            foreach (var @class in from instruction in result.Instructions.ToList()
                where instruction is Create && instruction.Parameters[0] == "КЛАСС"
                select new AnatoliyClass())
            {
                var createLine = result.Instructions.First(i => i is Create && i.Parameters[0] == "КЛАСС");
                var end = result.Instructions.First(i => i.Line > createLine.Line && i is End);
                foreach (var m in result.Methods)
                    @class.Methods.Add(m.Name, m);
                @class.Namespace = createLine.Parameters[2];
                @class.Name = createLine.Parameters[1];
                result.Classes.Add(@class);
                var endIndex = end.Line == result.Instructions.Count ? end.Line : end.Line + 1;
                _removeInstructionFrom(ref result.Instructions, createLine.Line, endIndex);
            }
            
            return result;
        }

        private static AnatoliyMethod? _parseMethod(ref List<Instruction> instructions, Instruction beginning)
        {
            if (instructions.All(i => i is not End))
                return null;
            if (!instructions.Any(i => i is End && i.Parameters[0] == "МЕТОДА"))
                return null;
            if (instructions.All(i => i.Line < beginning.Line && i is not End))
                return null;
            var result = new AnatoliyMethod();
            var end = instructions.First(i => i.Line > beginning.Line && i is End);
            result.Name = beginning.Parameters[1];
            result.Instructions = instructions.Skip(beginning.Line + 1).Take(end.Line - beginning.Line).ToList();
            var parameters = beginning.Parameters.Skip(2).ToList();
            if (parameters.Count % 2 != 0)
                return null;
            for (var i = 0; i < parameters.Count; i += 2)
                result.Parameters[parameters[i]] = new AnatoliyVariable(parameters[i + 1], parameters[i]);
            _removeInstructionFrom(ref instructions, beginning.Line, end.Line);
            return result;
        }

        private static string TryGetType(string value)
        {
            if (int.TryParse(value, out _)) return "ЧИСЛО";
            if (float.TryParse(value, out _)) return "ДРОБНОЕ";
            if (value.StartsWith("\"")) return "СТРОКА";
            return "НЕОПРЕДЕЛЁН";
        }
        private static AnatoliyVariable _parseVariable(ref List<Instruction> instructions, Instruction create)
        {
            _removeInstructionFrom(ref instructions, create.Line, create.Line + 1);
            var toAdd = new AnatoliyVariable(create.Parameters[1], create.Parameters[2], create.Parameters[3]);
            if (TryGetType((string) toAdd.Value!) == toAdd.Type)
            {
                if (Interpreter.Debug)
                    Console.WriteLine($"Successfully created a variable with name \"{toAdd.Name}\" with type \"{toAdd.Type}\" and value \"{toAdd.Value}\"");
                return toAdd;
            }
            Program.WriteError($"\nfile: {Interpreter.CurrentFile}\nline {create.Line + 1}: The value of the variable didn't match with the specified type.");
            Environment.Exit(1);
            return null;
        }

        private static void _removeInstructionFrom(ref List<Instruction> instructions, int from, int to)
        {
            instructions.RemoveRange(from, to - from);
        }
    }
}