using CSharpToTypescript.Models;
using System.Collections.Generic;

namespace CSharpToTypescript.Scts
{
    public class Generator
    {
        public static string GenerateCSTS(List<ModelDescription> models)
        {
            var result = "";
            result += GenerateModels(models);
            return result;
        }
        private static string GenerateModels(List<ModelDescription> Models)
        {
            string result = "";
            foreach (var model in Models)
            {
                var parent = model.Parent != null ? $" extends {model.Parent} " : "";
                result += "export interface " + model.Name + parent + "{\n";
                foreach (var field in model.Fields)
                {
                    result += "\t" + field.Name + ": " + field.Type + ";\n";
                }
                result += "}\n\n";
            }
            return result;
        }
    }
}
