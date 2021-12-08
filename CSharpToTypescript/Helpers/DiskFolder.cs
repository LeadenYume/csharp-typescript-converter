using System;
using System.Collections.Generic;
using System.IO;

namespace CSharpToTypescript.Helpers
{
    public class DiskFolder
    {
        public string BasePath { get; }
        public string Name { get; }
        public List<DiskFolder> ChildFolders { get; } = new List<DiskFolder>();
        public List<string> Files { get; } = new List<string>();
        public List<string> Folders { get; } = new List<string>();

        public DiskFolder(string name, string basePath)
        {
            Name = name;
            BasePath = basePath;
        }

        public List<string> Find(string name)
        {
            List<string> result = new List<string>();
            foreach (var fileName in Files)
            {
                if (fileName == name)
                    result.Add(FolderMath.Join(this.BasePath, fileName));
            }
            foreach(var level in ChildFolders)
            {
                result.AddRange(level.Find(name));
            }
            return result;
        }

        public List<string> FindWithType(string type)
        {
            List<string> result = new List<string>();
            foreach (var fileName in Files)
            {
                var dot = fileName.LastIndexOf('.');
                var _type = fileName.Substring(dot + 1);
                if (_type == type)
                    result.Add(FolderMath.Join(BasePath, fileName));
            }
            foreach (var level in ChildFolders)
            {
                result.AddRange(level.FindWithType(type));
            }
            return result;
        }
        public static DiskFolder FromPath(string path, List<string> excludeFolders = null, string fileMask = "", List<string> excludeFiels = null)
        {
            var _path = FolderMath.Normalize(path);
            var dirs = Directory.GetDirectories(path);
            var Files = fileMask == "" ? Directory.GetFiles(path) : Directory.GetFiles(path, fileMask);
            var LevelPrefix = FolderMath.GetContainingFolder(_path);
            var name = FolderMath.Sub(_path, LevelPrefix);
            var Level = new DiskFolder(name, _path);
  
            foreach (var File in Files)
            {
                var FileName = FolderMath.Clear(FolderMath.Sub(FolderMath.Normalize(File), Level.BasePath));
                if (excludeFiels != null && excludeFiels.IndexOf(FileName) != -1)
                    continue;
                Level.Files.Add(FileName);
            }
            foreach (var Folder in dirs)
            {
                var DirName = FolderMath.Clear(FolderMath.Sub(FolderMath.Normalize(Folder), Level.BasePath));
                if (excludeFolders != null && excludeFolders.IndexOf(DirName) != -1)
                    continue;
                Level.Folders.Add(DirName);
                var diskFolder = FromPath(Folder, excludeFolders, fileMask);
                Level.ChildFolders.Add(diskFolder);
            }
            return Level;
        }

        
        public string Print(bool GenerateString) => Print("", "", GenerateString);
        private string Print(string prefix, string prefixForFolder, bool GenerateString)
        {
            var result = "";
            if (ChildFolders.Count < 20)
            {
                var folderName = prefixForFolder + ((Files.Count > 0 || ChildFolders.Count > 0) ? "┬" : "─") + Name + "/";
                result += PrintToConsole(folderName, GenerateString);
                for (int i = 0; i < ChildFolders.Count; i++)
                {
                    var Level = ChildFolders[i];
                    result += Level.Print(prefix + "│   ", prefix + "├───", GenerateString);
                }
                if (Files.Count > 20)
                {
                    result += PrintToConsole(prefix + "└─ ...many...", GenerateString);
                }
                else
                {
                    for (int i = 0; i < Files.Count; i++)
                    {
                        if (i == Files.Count - 1)
                            result += PrintToConsole(prefix + "└─ " + Files[i], GenerateString);
                        else
                            result += PrintToConsole(prefix + "├─ " + Files[i], GenerateString);
                    }
                }
            }
            else
            {
                var folderName = prefixForFolder + ((Files.Count > 0 || ChildFolders.Count > 0) ? "┬" : "─") + Name + "/";
                result += PrintToConsole(folderName, GenerateString);
                result += PrintToConsole(prefix + "└─ ...many...", GenerateString);
            }
            return result;
        }

        private string PrintToConsole(string str, bool GenerateString)
        {
            if (GenerateString)
            {
                return str + "\n";
            }
            else
            {
                Console.WriteLine(str);
                return "";
            }
        }
    }
}
