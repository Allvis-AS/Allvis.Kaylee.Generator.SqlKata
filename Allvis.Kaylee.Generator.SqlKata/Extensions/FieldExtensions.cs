using Allvis.Kaylee.Analyzer.Models;

namespace Allvis.Kaylee.Generator.SqlKata.Extensions
{
    public static class FieldExtensions
    {
        public static bool HasDefault(this Field field)
            => field.DefaultExpression != null;

        public static bool IsInsertable(this Field field)
            => !field.Computed && field.Type.IsInsertable() && field.IsInsertablePK();

        public static bool IsInsertablePK(this Field field)
            => !field.AutoIncrement;
    }
}