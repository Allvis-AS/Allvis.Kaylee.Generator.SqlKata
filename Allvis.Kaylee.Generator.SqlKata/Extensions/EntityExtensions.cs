using System.Collections.Generic;
using System.Linq;
using Allvis.Kaylee.Analyzer.Models;

namespace Allvis.Kaylee.Generator.SqlKata.Extensions
{
    public static class EntityExtensions
    {
        public static IEnumerable<FieldReference> GetFullPrimaryKey(this Entity entity)
        {
            if (entity.Parent != null)
            {
                return GetFullPrimaryKey(entity.Parent).Concat(entity.PrimaryKey);
            }
            return entity.PrimaryKey;
        }

        public static string GetViewName(this Entity entity)
            => entity.DisplayName.Replace(".", "").Replace("::", ".v_");

        public static string GetTableName(this Entity entity)
            => entity.DisplayName.Replace(".", "").Replace("::", ".tbl_");
    }
}