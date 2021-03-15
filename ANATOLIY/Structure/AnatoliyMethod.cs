#nullable enable
using System;
using System.Collections.Generic;
using ANATOLIY.Core;
using ANATOLIY.Core.Instructions;

namespace ANATOLIY.Structure
{
    public class AnatoliyMethod
    {
        public List<AccessLevel> AccessLevel = new();
        public List<Instruction> Instructions = new();
        public string Name;
        public Dictionary<string, AnatoliyVariable> Parameters = new();
        public AnatoliyClass? ReturnType;

        private void _clear()
        {
            foreach (var item in Parameters)
                item.Value.Value = null;
        }

        public void Execute()
        {
            foreach (var ins in Instructions)
                if (ins is Call && ins.Parameters[0] == "ВСТРОЕННУЮ" && ins.Parameters[1] == "ВЫВОДА")
                {
                    if (Interpreter.Debug)
                        Console.WriteLine("Oops~ An IO monad~");
                    var str = (string) Parameters[ins.Parameters[2]].Value!;
                    str = str.Replace("\"", "").Trim();
                    Console.Write(str);
                }
                else
                {
                    ins.Execute();
                }

            _clear();
        }
    }
}