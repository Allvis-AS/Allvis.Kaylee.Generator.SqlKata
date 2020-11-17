using Allvis.Kaylee.Analyzer.Models;

namespace Allvis.Kaylee.Generator.SqlKata.Extensions
{
    public static class FieldExtensions
    {
        public static bool HasDefault(this Field field)
            => field.DefaultExpression != null;
    }
}