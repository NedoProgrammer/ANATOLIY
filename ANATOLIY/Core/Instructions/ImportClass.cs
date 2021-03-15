using System;
using System.Linq;

namespace ANATOLIY.Core.Instructions
{
    /// <summary>
    /// Import a class into <see cref="Interpreter.CurrentFileAvailableClasses"/> from <see cref="Interpreter.AvailableClasses"/>
    /// </summary>
    public class ImportClass : Instruction
    {
        /// <inheritdoc cref="Instruction"/>
        public override string CheckValid(string line)
        {
            var split = line.Split(" ");
            if (split.Length != 5)
                return "invalid amount of spaces! Should be 5:\nПОДКЛЮЧИТЬ КЛАСС {class} ИЗ {namespace}";

            return "valid";
        }

        /// <inheritdoc cref="Instruction"/>
        public override void PrepareForExecution()
        {
            Parameters.Remove("КЛАСС");
            Parameters.Remove("ИЗ");
        }

        /// <inheritdoc cref="Instruction"/>
        public override void Execute()
        {
            if (Interpreter.AvailableClasses.All(c => c.Name != Parameters[0]))
            {
                Console.WriteLine($"Couldn't find a class with name {Parameters[0]} in any namespace.");
            }
            else if (Interpreter.AvailableClasses.All(c => c.Namespace == Parameters[1] && c.Name != Parameters[0]))
            {
                Console.WriteLine($"Couldn't find class with name {Parameters[0]} in namespace {Parameters[1]}.");
            }
            else
            {
                var correctClass =
                    Interpreter.AvailableClasses.First(c => c.Namespace == Parameters[1] && c.Name == Parameters[0]);
                Interpreter.CurrentFileAvailableClasses.Add(correctClass);
                if (Interpreter.Debug)
                    Console.WriteLine(
                        $"Successfully imported class {Parameters[0]} from namespace {Parameters[1]} into {Interpreter.CurrentFile}");
            }
        }
    }
}