using System;
using System.Linq;
using Allvis.Kaylee.Analyzer.Models;
using Allvis.Kaylee.Generator.SqlKata.Builders;
using Allvis.Kaylee.Generator.SqlKata.Extensions;
using CaseExtensions;

namespace Allvis.Kaylee.Generator.SqlKata.Writers
{
    public static class QueriesWriter
    {
        public static string Write(Ast ast)
        {
            var sb = new SourceBuilder();
            sb.PublicStaticClass("Allvis.Kaylee.Generated.SqlKata", "Queries", sb =>
            {
                foreach (var schema in ast.Schemata)
                {
                    sb.Write(schema);
                }
            });
            return sb.ToString();
        }

        private static void Write(this SourceBuilder sb, Schema schema)
        {
            foreach (var entity in schema.Entities)
            {
                sb.Write(entity);
            }
        }

        private static void Write(this SourceBuilder sb, Entity entity)
        {
            sb.WriteExists(entity);
            
            foreach (var child in entity.Children)
            {
                sb.Write(child);
            }
        }

        private static void WriteExists(this SourceBuilder sb, Entity entity)
        {
            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            var fullPrimaryKey = entity.GetFullPrimaryKey();
            var parameters = fullPrimaryKey.Select(fr =>
            {
                var field = fr.ResolvedField;
                return (field.Type.ToCSharp(), field.Name.ToCamelCase());
            });
            sb.PublicStaticMethod("SqlKata.Query", $"{entityName}_Exists", parameters, sb =>
            {
                var viewName = entity.DisplayName.Replace(".", "").Replace("::", ".v_");
                sb.AL($@"return new SqlKata.Query(""{viewName}"")");
                sb.I(sb =>
                {
                    foreach (var field in fullPrimaryKey)
                    {
                        var fieldName = field.FieldName;
                        var parameterName = fieldName.ToCamelCase();
                        sb.AL($@".Where(""{fieldName}"", {parameterName})");
                    }
                    sb.AL(@".SelectRaw(""1"")");
                    sb.AL(@".Limit(1);");
                });
            });
        }
    }
}