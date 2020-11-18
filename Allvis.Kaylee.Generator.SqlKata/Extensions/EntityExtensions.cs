using Allvis.Kaylee.Analyzer.Models;

namespace Allvis.Kaylee.Generator.SqlKata.Extensions
{
    public static class EntityExtensions
    {
        public static string GetViewName(this Entity entity)
            => entity.DisplayName.Replace(".", "").Replace("::", ".v_");

        public static string GetTableName(this Entity entity)
            => entity.DisplayName.Replace(".", "").Replace("::", ".tbl_");

        public static string GetModelName(this Entity entity)
        {
            var displayName = entity.DisplayName;
            var from = displayName.IndexOf("::") + 2;
            return displayName.Substring(from).Replace(".", "");
        }
    }
}