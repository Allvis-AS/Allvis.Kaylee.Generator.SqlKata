using System.IO;
using System.Threading.Tasks;

namespace Allvis.Kaylee.Generator.SqlKata.Utilities
{
    public static class DebugUtils
    {
        public static Task WriteGeneratedFileToDisk(string fileName, string contents)
        {
            var path = Path.Combine("gen", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            return File.WriteAllTextAsync(path, contents);
        }
    }
}