using System.Linq;

namespace ANATOLIY.Core.Instructions
{
    /// <summary>
    /// Identify the end of an method or class
    /// like in pascal.
    /// </summary>
    public class End : Instruction
    {
        /// <summary>
        /// Allowed types to be "ended"..?
        /// </summary>
        public static readonly string[] Types = {"МЕТОДА", "КЛАССА"};
        /// <inheritdoc cref="Instruction"/>
        public override string CheckValid(string line)
        {
            var split = line.Split(" ");
            if (split.Length <= 1)
                return
                    "Invalid amount of arguments, you probably forgot to enter the type.\nКОНЕЦ {type}";
            var types = "";
            for (var i = 0; i < Types.Length; i++)
                if (i != Types.Length - 1)
                    types += Types[i] + ", ";
                else
                    types += Types[i];
            return !Types.ToList().Contains(split[1])
                ? $"Couldn't identify the type, you probably made a typo or wrote it in lowercase. Should be {types}."
                : "valid";
        }
        /// <inheritdoc cref="Instruction"/>
        public override void PrepareForExecution()
        {
        }
        /// <inheritdoc cref="Instruction"/>
        public override void Execute()
        {
            //not used.
            //this instruction is made for the parser.
        }
    }
}