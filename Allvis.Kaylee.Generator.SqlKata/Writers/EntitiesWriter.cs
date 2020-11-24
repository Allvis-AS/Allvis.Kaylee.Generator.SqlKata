using Allvis.Kaylee.Analyzer.Extensions;
using Allvis.Kaylee.Analyzer.Models;
using Allvis.Kaylee.Generator.SqlKata.Builders;
using Allvis.Kaylee.Generator.SqlKata.Extensions;
using CaseExtensions;
using System.Collections.Generic;
using System.Linq;

namespace Allvis.Kaylee.Generator.SqlKata.Writers
{
    public static class EntitiesWriter
    {
        public static string Write(Ast ast)
            => ast.Schemata.Write();

        private static string Write(this IEnumerable<Schema> schemata)
        {
            var sb = new SourceBuilder();
            sb.AL("#nullable enable");
            sb.NL();
            sb.PublicStaticClass("Allvis.Kaylee.Generated.SqlKata", "Entities", sb =>
            {
                foreach (var schema in schemata)
                {
                    sb.PublicStaticClass(schema.Name, sb =>
                    {
                        foreach (var entity in schema.Entities)
                        {
                            sb.Write(entity);
                        }
                    });
                }
            });
            return sb.ToString();
        }

        private static void Write(this SourceBuilder sb, Entity entity)
        {
            sb.WriteEntity(entity);
            foreach (var child in entity.Children)
            {
                sb.Write(child);
            }
        }

        private static void WriteEntity(this SourceBuilder sb, Entity entity)
        {
            bool IsNullable(Field field)
            {
                return !field.IsPartOfParentKey(entity) && field.Nullable;
            }
            var className = entity.GetModelName();
            sb.PublicClass(className, sb =>
            {
                var fullPrimaryKey = entity.GetFullPrimaryKey();
                var allFields = fullPrimaryKey.Select(fr => fr.ResolvedField).Concat(entity.Fields).Distinct();
                foreach (var field in allFields)
                {
                    var type = field.Type.ToCSharp();
                    var nullable = IsNullable(field);
                    var nullabilityMark = nullable ? "?" : "";
                    var name = field.Name.ToPascalCase();
                    var identity = field.Type.ToCSharpIdentity();
                    var defaultAssignment = !nullable && identity.Length > 0 ? $" = {identity};" : "";
                    sb.AL($"public {type}{nullabilityMark} {name} {{ get; set; }}{defaultAssignment}");
                }
            });
        }
    }
}