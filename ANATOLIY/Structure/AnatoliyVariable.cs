#nullable enable
namespace ANATOLIY.Structure
{
    public class AnatoliyVariable
    {
        public object? Value;

        public AnatoliyVariable(string name, string type, object value = null!)
        {
            Value = value;
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public string Type { get; }
    }
}