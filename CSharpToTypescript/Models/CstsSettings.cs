using System.Collections.Generic;

namespace CSharpToTypescript.Models
{
    public class CstsSettings
    {
        public string BuildPath { get; set; }

        public List<string> HeadStrings { get; set; } = new List<string>();
    }
}
