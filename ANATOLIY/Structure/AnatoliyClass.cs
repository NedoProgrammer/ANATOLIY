using System.Collections.Generic;

namespace ANATOLIY.Structure
{
    public class AnatoliyClass
    {
        public List<AccessLevel> AccessLevel = new();
        public Dictionary<string, AnatoliyMethod> Methods = new();
        public string Name;
        public string Namespace;
        public Dictionary<string, AnatoliyVariable> Properties = new();
    }
}