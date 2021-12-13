using System.Collections.Generic;

namespace CSharpToTypescript.Models
{
    public class ModelDescription
    {
        public string Name { get; }
        public string Parent { get; } = null;
        public List<FieldDescription> Fields { get; }

        public ModelDescription(string name, string parent, List<FieldDescription> fields)
        {
            Name = name;
            Fields = fields;
            Parent = parent;
        }
    }
}
