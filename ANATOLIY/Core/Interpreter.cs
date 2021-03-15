using System;
using System.Collections.Generic;
using ANATOLIY.Structure;

namespace ANATOLIY.Core
{
    public class Interpreter
    {
        public static bool Debug = false;
        public static bool LinkStd = true;
        public static List<AnatoliyClass> AvailableClasses = new();
        public static List<AnatoliyClass> CurrentFileAvailableClasses = new();
        public static string CurrentFile { get; private set; }

        public static void Interpret(string file)
        {
            CurrentFile = file;
            CurrentFileAvailableClasses.Clear();
            var parserResult = Parser.Parse(file);
            AvailableClasses.AddRange(parserResult.Classes);
            if (Debug)
                Console.WriteLine(
                    $"Added {parserResult.Classes.Count} classes after parsing {file}\nExecuting left commands..");
            foreach (var instruction in parserResult.Instructions)
                instruction.Execute();
        }
    }
}