namespace CSharpToTypescript.Helpers
{
    public class FolderMath
    {
        public static string Normalize(string path)
        {
            var normalizedPath = path
              .Replace(@"\", "/").Replace(@"\\", "/").Replace("//", "/")
              .Replace(@"\\\", "/").Replace("///", "/");
            if (normalizedPath.Length > 0 && normalizedPath[normalizedPath.Length - 1] == '/')
                normalizedPath = normalizedPath.Substring(0, normalizedPath.Length - 1);
            return normalizedPath;
        }
        public static string Clear(string path) => path
            .Replace(@"/", "").Replace(@"\", "").Replace(@"\\", "").Replace("//", "")
            .Replace(@"\\\", "").Replace("///", "");

        public static string Sub(string path, string subPath)
        {
            var result = path;
            var index = path.IndexOf(subPath);
            if (index == 0)
            {
                result = path.Substring(subPath.Length);
            }
            return result;
        }

        public static bool IsChild(string path, string find)
        {
            var index = path.IndexOf(find);
            if (index == 0)
                return true;
            else
                return false;
        }

        public static string Join(params string[] parts)
        {
            return Normalize(string.Join("/", parts));
        }

        public static string GetContainingFolder(string path)
        {
            path = Normalize(path);
            var index = path.LastIndexOf('/');
            if (index == -1)
                return "";
            var result = path.Substring(0, index);
            return result + '/';
        }

        public static string GetLastName(string path)
        {
            var Path = Normalize(path);
            var Containing = GetContainingFolder(Path);
            var Name = Sub(Path, Containing);
            return Clear(Name);
        }
    }
}
