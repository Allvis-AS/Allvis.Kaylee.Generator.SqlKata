using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Allvis.Kaylee.Generator.SqlKata.Utilities
{
    public static class DebugUtils
    {
        public static Task WriteGeneratedFileToDisk(string fileName, string contents)
        {
            if (Debugger.IsAttached)
            {
                Directory.CreateDirectory("gen");
                return File.WriteAllTextAsync(Path.Combine("gen", fileName), contents);
            }
            return Task.CompletedTask;
        }
    }
}