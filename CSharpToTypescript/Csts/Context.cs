using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CSharpToTypescript.Scts
{
    public class Context
    {
        private List<string> ClassNames { get; } = new List<string>();

        public void ParseFromCode(string code)
        {
            var syntaxTree = SyntaxFactory.ParseSyntaxTree(code);
            var root = syntaxTree.GetRoot();
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();
            var classNames = classes.Select(cls => cls.Identifier.Text).ToList();

            ClassNames.AddRange(classNames);
        }

        public bool ContainsClass(string name)
        {
            var cleanedName = name
                .Replace(" ", "")
                .Replace("\n", "")
                .Replace("\t", "")
                .Replace("\r", "");

            return ClassNames.Contains(cleanedName);
        }
    }
}
