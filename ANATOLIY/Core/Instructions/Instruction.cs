using System.Collections.Generic;

namespace ANATOLIY.Core.Instructions
{
    public abstract class Instruction
    {
        public int Line;

        /// <summary>
        ///     What comes after the Beginning of the instruction?
        /// </summary>
        public List<string> Parameters;

        /// <summary>
        ///     Check if the instruction is written correctly.
        ///     If there are no errors, "valid" must be returned.
        /// </summary>
        /// <returns>A string containing "valid" or the error message.</returns>
        public abstract string CheckValid(string line);
        
        /// <summary>
        /// Remove not used parameters.
        /// </summary>
        public abstract void PrepareForExecution();
        /// <summary>
        /// Execute! :D
        /// </summary>
        public abstract void Execute();
    }
}