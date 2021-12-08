using CSharpToTypescript.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CSharpToTypescript.Scts
{
    public static class ModelsParser
    {
        public static string ModelAtribute = "TypeScriptModel";
        public static void FromText(string code, Context context, List<ModelDescription> result)
        {
            var syntaxTree = SyntaxFactory.ParseSyntaxTree(code);
            var root = syntaxTree.GetRoot();
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();

            foreach (var _class in classes)
            {
                var isCstsModel = false;
                foreach (var attributeList in _class.AttributeLists)
                {
                    foreach (var attribute in attributeList.Attributes)
                    {
                        if (ModelAtribute == attribute.Name.ToString())
                        {
                            isCstsModel = true;
                        }
                    }
                }
                if (isCstsModel)
                {
                    var fieldDescriptions = new List<FieldDescription>();

                    var fields = _class.Members.OfType<FieldDeclarationSyntax>()
                        .Where(field => !field.Modifiers.Any(SyntaxKind.StaticKeyword))
                        .Where(field => !field.Modifiers.Any(SyntaxKind.ProtectedKeyword))
                        .Where(field => !field.Modifiers.Any(SyntaxKind.PrivateKeyword));

                    var properties = _class.Members.OfType<PropertyDeclarationSyntax>()
                        .Where(field => !field.Modifiers.Any(SyntaxKind.StaticKeyword))
                        .Where(field => !field.Modifiers.Any(SyntaxKind.ProtectedKeyword))
                        .Where(field => !field.Modifiers.Any(SyntaxKind.PrivateKeyword));

                    string Parent = null;
                    if (_class.BaseList != null && _class.BaseList.Types != null && _class.BaseList.Types.Count > 0)
                    {
                        var Parents = _class.BaseList.Types
                            .Select(type => type.ToString())
                            .Where(text => context.ContainsClass(text))
                            .ToList();
                        Parent = Parents.Count > 0 ? Parents[0] : null;
                    }

                    foreach (var field in fields)
                    {
                        fieldDescriptions.Add(new FieldDescription(
                                field.Declaration.Variables.ToString(),
                                field.Declaration.Type.ToString()
                            ));
                    }
                    foreach (var property in properties)
                    {
                        fieldDescriptions.Add(new FieldDescription(
                                property.Identifier.Text,
                                property.Type.ToString()
                            ));
                    }

                    result.Add(new ModelDescription(_class.Identifier.Text, Parent, fieldDescriptions));
                }
            }
        }
    }
}