using Allvis.Kaylee.Analyzer.Extensions;
using Allvis.Kaylee.Analyzer.Models;
using Allvis.Kaylee.Generator.SqlKata.Builders;
using Allvis.Kaylee.Generator.SqlKata.Extensions;
using CaseExtensions;
using System.Collections.Generic;
using System.Linq;

namespace Allvis.Kaylee.Generator.SqlKata.Writers
{
    public static class ModelsWriter
    {
        public static IEnumerable<(string HintName, string Source)> Write(Ast ast)
            => ast.Schemata.SelectMany(schema => schema.Write());

        private static IEnumerable<(string HintName, string Source)> Write(this Schema schema)
            => schema.Entities.SelectMany(entity => entity.Write());

        private static IEnumerable<(string HintName, string Source)> Write(this Entity entity)
        {
            var sb = new SourceBuilder();
            var schemaName = entity.Schema.Name;
            var modelName = entity.GetModelName();
            var hintName = $"Allvis.Kaylee.Generated.SqlKata.Models.{schemaName}.{modelName}";
            sb.WriteModel(entity);
            var source = sb.ToString();
            yield return (hintName, source);
            foreach (var child in entity.Children)
            {
                foreach (var tuple in child.Write())
                {
                    yield return tuple;
                }
            }
        }

        private static void WriteModel(this SourceBuilder sb, Entity entity)
        {
            bool IsNullable(Field field)
            {
                var partOfParentKey = field.Entity != entity;
                return !partOfParentKey && field.Nullable;
            }

            var schemaName = entity.Schema.Name;
            var ns = $"Allvis.Kaylee.Generated.SqlKata.Models.{schemaName}";
            var className = entity.GetModelName();
            sb.PublicClass(ns, className, sb =>
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