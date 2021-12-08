using System.Collections.Generic;

namespace CSharpToTypescript.Models
{
    public class ModelDescription
    {
        public string Name;
        public string Parent = null;
        public List<FieldDescription> Fields;

        public ModelDescription(string name, string parent, List<FieldDescription> fields)
        {
            Name = name;
            Fields = fields;
            Parent = parent;
        }
    }
}
