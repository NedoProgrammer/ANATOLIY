using System.Linq;

namespace ANATOLIY.Core.Instructions
{
    public class Create : Instruction
    {
        /// <summary>
        ///     Types which are allowed to be created.
        /// </summary>
        public static readonly string[] Types = {"МЕТОД", "КЛАСС", "ПЕРЕМЕННУЮ"};

        /// <inheritdoc cref="Instruction" />
        public override string CheckValid(string line)
        {
            var split = line.Split(" ");
            if (split.Length <= 1)
                return
                    "Invalid amount of arguments, you probably forgot to enter the type.\nСОЗДАТЬ {type} {name} {arguments}";
            var types = "";
            for (var i = 0; i < Types.Length; i++)
                if (i != Types.Length - 1)
                    types += Types[i] + ", ";
                else
                    types += Types[i];
            if (!Types.Contains(split[1]))
                return
                    $"Couldn't identify the type, you probably made a typo or wrote it in lowercase. Should be {types}.";
            if (split[1] == "КЛАСС" && split.Length != 6)
                return
                    "Invalid amount of arguments for creation of КЛАСС (class).\nСОЗДАТЬ КЛАСС {class} В ПРОСТРАНСТВЕ {namespace}";
            if (split[1] == "МЕТОД" && split.Length < 5)
                return
                    "Invalid amount of arguments for creation of МЕТОД (method)\nСОЗДАТЬ МЕТОД {methodName} БЕЗ ПАРАМЕТРОВ\nСОЗДАТЬ МЕТОД {methodName} С АРГУМЕНТАМИ {...}";
            if (split[1] == "ПЕРЕМЕННУЮ" && split.Length < 4)
                return
                    "Invalid amount of arguments for creation of ПЕРЕМЕННУЮ (variable)\nСОЗДАТЬ ПЕРЕМЕННУЮ {name} С ТИПОМ {type}\nСОЗДАТЬ ПЕРЕМЕННУЮ {name} С ТИПОМ {type} СО ЗНАЧЕНИЕМ {value}";
            return "valid";
        }

        /// <inheritdoc cref="Instruction" />
        public override void PrepareForExecution()
        {
            switch (Parameters[0])
            {
                case "КЛАСС":
                    Parameters.Remove("В");
                    Parameters.Remove("ПРОСТРАНСТВЕ");
                    break;
                case "МЕТОД":
                    Parameters.RemoveAll(p => p == "С" || p == "ТИПОМ" || p == "ПАРАМЕТРАМИ");
                    break;
                case "ПЕРЕМЕННУЮ":
                    Parameters.RemoveAll(p => p == "С" || p == "ТИПОМ" || p == "СО" || p == "ЗНАЧЕНИЕМ");
                    break;
            }
        }

        /// <inheritdoc cref="Instruction" />
        public override void Execute()
        {
            //not used. this instruction is handled by the parser.
        }
    }
}