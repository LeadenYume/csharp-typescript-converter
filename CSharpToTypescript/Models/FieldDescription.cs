namespace CSharpToTypescript.Models
{

    public class FieldDescription
    {
        public string Name;
        public string Type;

        public FieldDescription(string name, string type)
        {
            Name = PascalToCamel(name);
            Type = TypeConvert(type);
        }

        private static string TypeConvert(string type)
        {
            var Type = type.Replace("List<", "Array<");
            Type = Type.Replace("decimal[]", "number[]");

            switch (Type)
            {
                case "int": return "number";
                case "long": return "number";
                case "byte": return "number";
                case "decimal": return "number";
                case "float": return "number";
                case "double": return "number";
                case "DateTime": return "Date";
                case "bool": return "boolean";
                case "dynamic": return "any";
                default: return Type;
            }
        }

        private static string PascalToCamel(string Name)
        {
            var first = Name.Substring(0, 1).ToLower();
            return first + Name.Substring(1);
        }
    }
}
