using System;
using System.IO;
using System.Linq;
using ANATOLIY.Core;

namespace ANATOLIY
{
    internal class Program
    {
        /// <summary>
        ///     Only used for <see cref="HasArgument" />.
        ///     This is the copy of the original args passed into
        ///     the main function.
        /// </summary>
        private static string[] Args;

        private static void Main(string[] args)
        {
            //:D
            Args = args;
            //Same thingy as in GCC
            if (args.Length == 0)
            {
                WriteError("no input files");
            }
            else
            {
                //If we wanna see all messages by ANATOLIY
                if (HasArgument("--debug"))
                    Interpreter.Debug = true;
                //If we wanna be a little faster
                if (HasArgument("--dontlinkstd"))
                    Interpreter.LinkStd = false;
                //If the file passed does indeed exist
                if (args.Any(File.Exists))
                {
                    var file = args.First(File.Exists);
                    if (Interpreter.Debug)
                        Console.WriteLine($"{file} file was selected.\nStarting..");
                    //Add the standart classes like for IO, and etc.
                    Linker.AddStd("std");
                    //...and finally run the file
                    Interpreter.Interpret(file);
                    if(Interpreter.Debug)
                        Console.WriteLine("\nAnatoliy is happy, the code ran without errors! :D");
                }
                else
                {
                    WriteError("none of the files exist");
                }
            }
        }

        /// <summary>
        ///     Beautiful error printing.
        /// </summary>
        /// <param name="error">The error message.</param>
        public static void WriteError(string error)
        {
            Write("ANATOLIY: ", ConsoleColor.White);
            Write("fatal error: ", ConsoleColor.DarkRed);
            WriteLine(error, ConsoleColor.White);
        }

        /// <summary>
        ///     Write line with color.
        /// </summary>
        /// <param name="text">The... text...? :D</param>
        /// <param name="color">The foreground color.</param>
        private static void WriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        ///     Write with color.
        /// </summary>
        /// <param name="text">The... text...? :D</param>
        /// <param name="color">The foreground color.</param>
        private static void Write(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        /// <summary>
        ///     Check if an argument has been passed.
        /// </summary>
        /// <param name="name">Name of the argument (with "--"!)</param>
        /// <returns>A boolean showing is the argument passed present in <see cref="Args" />.</returns>
        public static bool HasArgument(string name)
        {
            return Args.Any(a => a.ToLower() == name.ToLower());
        }
    }
}