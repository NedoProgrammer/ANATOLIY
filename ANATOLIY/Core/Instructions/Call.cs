using System;
using System.Collections.Generic;
using System.Linq;

namespace ANATOLIY.Core.Instructions
{
    /// <summary>
    ///     Call some method or internal function.
    /// </summary>
    public class Call : Instruction
    {
        /// <inheritdoc cref="Instruction" />
        public override string CheckValid(string line)
        {
            var split = line.Split(" ");
            if (split.Length < 2)
                return
                    "Invalid amount of arguments for calling a method. Should be at least 4.\nВЫЗВАТЬ ВСТРОЕННУЮ ФУНКЦИЮ ВЫВОДА С ПАРАМЕТРАМИ \"hello\" <--- internal output\nВЫЗВАТЬ СКАЗАТЬ В КОНСОЛЬ С ПАРАМЕТРАМИ \"hello\" <--- the console class";
            return "valid";
        }

        /// <inheritdoc cref="Instruction" />
        public override void PrepareForExecution()
        {
            //uh oh, we have string parameters!
            if (Parameters.Any(p => p.StartsWith("\"")))
                _correctSpaces();
            //we dont need that.
            Parameters.Remove("В");
            Parameters.Remove("С");
            Parameters.Remove("ПАРАМЕТРАМИ");
            Parameters.Remove("ФУНКЦИЮ");
        }

        /// <summary>
        ///     Every space in the instruction is thought to be an argument,
        ///     but if a parameter is a string, and it contains spaces,
        ///     these strings should be merged together.
        /// </summary>
        private void _correctSpaces()
        {
            //What's the current index of the element with a beginning quote?
            var start = 0;
            //If the strings were merged correctly,
            //How much elements should the program skip?
            var skipCount = 0;
            //Storing new strings
            var newParameters = new List<string>();
            foreach (var parameter in Parameters)
            {
                //If we should skip
                if (skipCount != 0)
                {
                    skipCount--;
                    continue;
                }

                //If the parameter does not begin with a quote,
                //OR it has quotes on both sides
                //First: alalalal  <--- skip
                //Second: "alalalal" <--- skip
                if (!parameter.StartsWith("\"") || parameter.StartsWith("\"") && parameter.EndsWith("\""))
                {
                    newParameters.Add(parameter);
                    continue;
                }

                //Where is the beginning?
                //If it was 0, just search for the first result,
                //If not, search for the index higher than the current one.
                start = start == 0
                    ? Parameters.IndexOf(parameter)
                    : Parameters.FindIndex(p => p.StartsWith("\"") && Parameters.IndexOf(p) > start);
                //Find any string that ends with a quote, 
                //AND has a higher index than "start".
                var next = Parameters.FindIndex(p =>
                {
                    var index = Parameters.IndexOf(p);
                    return index >= start && p.EndsWith("\"");
                });
                //Combine strings.
                var combined = "";
                for (var i = start; i <= next; i++)
                    combined += i == next ? Parameters[i] : Parameters[i] + " ";
                newParameters.Add(combined);
                // "hello world"
                // Beginning --> 0
                // Next --> 1
                // If we want to skip world in the next foreach loop,
                // SkipCount should be (next - start) OR (1 - 0) = 1
                skipCount = next - start;
                // Search further.
                start = next;
            }

            //We're done! :D
            Parameters = newParameters;
        }

        /// <inheritdoc cref="Instruction" />
        public override void Execute()
        {
            //Parameters[0] --> Name of the method.
            //Parameters[1] --> Name of the class.
            //Parameters[2...] --> Parameters.

            //Try to find a class which has the name passed. (Parameters[1])
            var @class = Interpreter.CurrentFileAvailableClasses.FirstOrDefault(c => c.Name == Parameters[1]);
            if (@class == null)
                return;
            //If that class does not contain a method passed. (Parameters[0])
            if (!@class.Methods.ContainsKey(Parameters[0]))
                return;
            if (Interpreter.Debug)
                Console.WriteLine($"Calling {@class.Methods[Parameters[0]].Name} from {@class.Name}..");
            //Get arguments.
            var args = Parameters.Skip(2).ToList();
            for (var i = 0; i < args.Count; i++)
                @class.Methods[Parameters[0]].Parameters.ElementAt(i).Value.Value = args[i];
            //We're done! :D
            @class.Methods[Parameters[0]].Execute();
        }
    }
}