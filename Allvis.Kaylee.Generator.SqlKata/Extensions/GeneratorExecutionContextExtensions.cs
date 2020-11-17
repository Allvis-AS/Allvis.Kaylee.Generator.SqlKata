using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Allvis.Kaylee.Generator.SqlKata.Extensions
{
    public static class GeneratorExecutionContextExtensions
    {
        public static IEnumerable<AdditionalText> GetKayleeModelFiles(this GeneratorExecutionContext context)
            => context.AdditionalFiles.Where(f => IsKayleeFile(f.Path));

        public static bool IsKayleeFile(string path)
            => Path.GetExtension(path).ToUpperInvariant() == ".KAY";
    }
}