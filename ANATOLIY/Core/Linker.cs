using System;
using System.IO;

namespace ANATOLIY.Core
{
    public class Linker
    {
        public static void AddStd(string stdFolder)
        {
            if (!Interpreter.LinkStd) return;
            if (!Directory.Exists(stdFolder))
                throw new ArgumentException("The provided STD folder does not exist!", nameof(stdFolder));
            if (Interpreter.Debug)
                Console.WriteLine("Constructing STD code..");
            foreach (var file in Directory.GetFiles(stdFolder, "*.ANATOLIY", SearchOption.AllDirectories))
                Interpreter.Interpret(file);
        }
    }
}