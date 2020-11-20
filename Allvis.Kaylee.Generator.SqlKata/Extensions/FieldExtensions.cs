using System.Linq;
using Allvis.Kaylee.Analyzer.Models;

namespace Allvis.Kaylee.Generator.SqlKata.Extensions
{
    public static class FieldExtensions
    {
        public static bool HasDefault(this Field field)
            => field.DefaultExpression != null;

        public static bool IsInsertable(this Field field, Entity forEntity)
            => !field.Computed
            && field.Type.IsInsertable()
            && (!field.IsPartOfPrimaryKey(forEntity) || field.IsInsertablePK(forEntity));

        public static bool IsInsertablePK(this Field field, Entity forEntity)
            => (field.IsPartOfParentKey(forEntity) || !field.AutoIncrement);

        public static bool IsPartOfPrimaryKey(this Field field, Entity forEntity)
            => field.Entity == forEntity;

        public static bool IsPartOfParentKey(this Field field, Entity forEntity)
            => field.Entity != forEntity;
    }
}