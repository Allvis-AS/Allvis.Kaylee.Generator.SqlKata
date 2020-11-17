using System.IO;
using System.Threading.Tasks;

namespace Allvis.Kaylee.Generator.SqlKata.Utilities
{
    public static class DebugUtils
    {
        public static Task WriteGeneratedFileToDisk(string fileName, string contents)
        {
            Directory.CreateDirectory("gen");
            return File.WriteAllTextAsync(Path.Combine("gen", fileName), contents);
        }
    }
}