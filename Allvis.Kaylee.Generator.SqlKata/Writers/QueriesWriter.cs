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
            sb.WriteInsert(entity);
            sb.WriteInsertMany(entity);
            sb.WriteDelete(entity);
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
            sb.PublicStaticMethod("SqlKata.Query", $"Exists_{entityName}", parameters, sb =>
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

        private static void WriteInsert(this SourceBuilder sb, Entity entity)
        {
            bool IsOptional(Field field)
            {
                var partOfParentKey = field.Entity != entity;
                return !partOfParentKey && (field.HasDefault() || field.Nullable);
            }

            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            var fullPrimaryKey = entity.GetFullPrimaryKey();
            var allFields = fullPrimaryKey.Select(fr => fr.ResolvedField).Concat(entity.Fields).Distinct();
            var parameters = allFields.Select(f => (IsOptional(f), f.Type.ToCSharp(), f.Name.ToCamelCase()));
            sb.PublicStaticMethod("SqlKata.Query", $"Insert_{entityName}", parameters, sb =>
            {
                sb.AL("var _columns = new System.Collections.Generic.List<string>();");
                sb.AL("var _values = new System.Collections.Generic.List<object>();");
                foreach (var field in allFields)
                {
                    var fieldName = field.Name;
                    var parameterName = fieldName.ToCamelCase();
                    var optional = IsOptional(field);
                    if (optional)
                    {
                        sb.AL($"if ({parameterName} != null)");
                        sb.B(sb =>
                        {
                            sb.AL($@"_columns.Add(""{fieldName}"");");
                            sb.AL($@"_values.Add({parameterName});");
                        });
                    }
                    else
                    {
                        sb.AL($@"_columns.Add(""{fieldName}"");");
                        sb.AL($@"_values.Add({parameterName});");
                    }
                }
                var tableName = entity.DisplayName.Replace(".", "").Replace("::", ".tbl_");
                sb.AL($@"return new SqlKata.Query(""{tableName}"")");
                sb.I(sb =>
                {
                    sb.AL(@".AsInsert(_columns, _values);");
                });
            });
        }

        private static void WriteInsertMany(this SourceBuilder sb, Entity entity)
        {
            bool IsNullable(Field field)
            {
                var partOfParentKey = field.Entity != entity;
                return !partOfParentKey && field.Nullable;
            }

            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            var fullPrimaryKey = entity.GetFullPrimaryKey();
            var allFields = fullPrimaryKey.Select(fr => fr.ResolvedField).Concat(entity.Fields).Distinct();
            var tupleParameters = allFields.Select(f =>
            {
                var nullable = IsNullable(f);
                var type = f.Type.ToCSharp();
                return (nullable ? $"{type}?" : type, f.Name.ToPascalCase());
            });
            var parameters = new[] { ($"System.Collections.Generic.IEnumerable<({tupleParameters.Join()})>", "rows") };
            sb.PublicStaticMethod("SqlKata.Query", $"Insert_{entityName}", parameters, sb =>
            {
                sb.AL("var _columns = new string[] {");
                sb.I(sb =>
                {
                    allFields.ForEach((field, last) =>
                    {
                        var fieldName = field.Name;
                        var comma = last ? "" : ",";
                        sb.AL($@"""{fieldName}""{comma}");
                    });
                });
                sb.AL("};");
                sb.AL("var _values = rows.Select(_row => new object[] {");
                sb.I(sb =>
                {
                    allFields.ForEach((field, last) =>
                    {
                        var parameterName = field.Name.ToPascalCase();
                        var comma = last ? "" : ",";
                        sb.AL($@"_row.{parameterName}{comma}");
                    });
                });
                sb.AL("});");
                var tableName = entity.DisplayName.Replace(".", "").Replace("::", ".tbl_");
                sb.AL($@"return new SqlKata.Query(""{tableName}"")");
                sb.I(sb =>
                {
                    sb.AL(@".AsInsert(_columns, _values);");
                });
            });
        }

        private static void WriteDelete(this SourceBuilder sb, Entity entity)
        {
            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            var fullPrimaryKey = entity.GetFullPrimaryKey();
            var parameters = fullPrimaryKey.Select(fr =>
            {
                var field = fr.ResolvedField;
                return (field.Type.ToCSharp(), field.Name.ToCamelCase());
            });
            sb.PublicStaticMethod("SqlKata.Query", $"Delete_{entityName}", parameters, sb =>
            {
                var viewName = entity.DisplayName.Replace(".", "").Replace("::", ".tbl_");
                sb.AL($@"return new SqlKata.Query(""{viewName}"")");
                sb.I(sb =>
                {
                    foreach (var field in fullPrimaryKey)
                    {
                        var fieldName = field.FieldName;
                        var parameterName = fieldName.ToCamelCase();
                        sb.AL($@".Where(""{fieldName}"", {parameterName})");
                    }
                    sb.AL(@".AsDelete();");
                });
            });
        }
    }
}