using CSharpToTypescript.Helpers;
using CSharpToTypescript.Models;
using Microsoft.Build.Evaluation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CSharpToTypescript.Scts
{
    public class CstsWorker
    {
        public static void Start()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var currentFolder = Directory.GetCurrentDirectory();
            var settingsFile = FolderMath.Join(currentFolder, "CstsSettings.json");

            //Settings
            Console.WriteLine("Current folder: " + currentFolder);
            if (!File.Exists(settingsFile))
            {
                Console.WriteLine("CstsSettings.json don't find");
                return;
            }

            CstsSettings settings;
            try
            {
                settings = JsonConvert.DeserializeObject<CstsSettings>(File.ReadAllText(settingsFile));
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't convert CstsSettings.json \n" + e.Message);
                return;
            }

            //CSPROJ
            var rootFolder = DiskFolder.FromPath(
                        currentFolder,
                        new List<string>() {
                        "node_modules",
                        "bin",
                        "obj",
                        ".vs",
                        ".git"
                        },
                        "*.csproj"
                    );

            var mainProjectFiles = rootFolder.FindWithType("csproj");
            string csprojFile;
            if (mainProjectFiles.Count == 0)
            {
                Console.WriteLine("Csproj not found.");
                return;
            }
            else
                csprojFile = mainProjectFiles[0];


            Console.WriteLine("Root project: " + csprojFile);
            Console.WriteLine("Output dir: " + settings.BuildPath);

            var context = new Context();
            var models = new List<ModelDescription>();
            var csharpFiles = FindCs(currentFolder, csprojFile);

            foreach (var csPath in csharpFiles)
            {
                var text = File.ReadAllText(csPath);
                context.ParseFromCode(text);
            }

            foreach (var csPath in csharpFiles)
            {
                var text = File.ReadAllText(csPath);
                ModelsParser.FromText(text, context, models);
            }

            var csts = Generator.GenerateCSTS(models);
            if (settings.HeadStrings != null)
            {
                settings.HeadStrings.Reverse();
                foreach (var str in settings.HeadStrings)
                    csts = str + "\n" + csts;
            }

            var outputFolder = FolderMath.Join(currentFolder, settings.BuildPath);
            var outputFilePath = FolderMath.Join(outputFolder, "csts.ts");
            File.WriteAllText(outputFilePath, csts);

            stopwatch.End();
            Console.WriteLine("Work time = " + stopwatch.SecondResult);
        }

        public static List<string> FindCs(string mainProjectFolder, string mainCsprojFile)
        {
            var sharpsFiles = new List<string>();
            var projectsPath = new List<string>();
            projectsPath.Add(mainProjectFolder);

            var rootProject = new Project(mainCsprojFile);
            var projectsRefs = rootProject.GetItems("ProjectReference");

            foreach (var refProject in projectsRefs)
            {
                var path = FolderMath.Normalize(refProject.EvaluatedInclude);
                var currentFolder = FolderMath.Normalize(mainProjectFolder);
                while (path.IndexOf("../") != -1)
                {
                    currentFolder = FolderMath.GetContainingFolder(currentFolder);
                    path = path.Substring(3);
                }
                if (path.IndexOf(@":/") == -1)
                    path = FolderMath.GetContainingFolder(FolderMath.Join(currentFolder, path));
                else
                    path = FolderMath.GetContainingFolder(path);
                projectsPath.Add(path);
                Console.WriteLine("Ref project: " + path);
            }

            foreach (var projectPath in projectsPath)
            {
                var currentFolder = DiskFolder.FromPath(
                        projectPath,
                        new List<string>() {
                        "node_modules",
                        "bin",
                        "obj",
                        ".vs",
                        ".git"
                        },
                        "*.cs"
                    );

                var files = currentFolder.FindWithType("cs");
                sharpsFiles.AddRange(files);
            }

            return sharpsFiles;
        }
    }
}
