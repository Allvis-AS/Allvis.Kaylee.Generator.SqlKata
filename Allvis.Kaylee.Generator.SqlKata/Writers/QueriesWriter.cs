using Allvis.Kaylee.Analyzer.Extensions;
using Allvis.Kaylee.Analyzer.Models;
using Allvis.Kaylee.Generator.SqlKata.Builders;
using Allvis.Kaylee.Generator.SqlKata.Extensions;
using CaseExtensions;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Allvis.Kaylee.Generator.SqlKata.Writers
{
    public static class QueriesWriter
    {
        public static string Write(Ast ast)
        {
            var sb = new SourceBuilder();
            sb.AL("#nullable enable");
            sb.NL();
            sb.AL("using System.Linq;");
            sb.NL();
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
            sb.WriteExistsUniqueKey(entity);
            sb.WriteCount(entity);
            sb.WriteGet(entity);
            sb.WriteGetUniqueKey(entity);
            if (!entity.IsQuery)
            {
                sb.WriteInsert(entity);
                sb.WriteInsertMany(entity);
                sb.WriteDelete(entity);
                sb.WriteDeleteUniqueKey(entity);
                foreach (var mutation in entity.Mutations)
                {
                    sb.WriteUpdate(mutation);
                    sb.WriteUpdateUniqueKey(mutation);
                }
            }
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
            sb.PublicStaticMethod("global::SqlKata.Query", $"Exists_{entityName}", parameters, sb =>
            {
                var viewName = entity.GetViewName();
                sb.AL($@"return new global::SqlKata.Query(""{viewName}"")");
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

        private static void WriteExistsUniqueKey(this SourceBuilder sb, Entity entity)
        {
            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            foreach (var key in entity.UniqueKeys)
            {
                var fields = key.FieldReferences.Select(fr => fr.ResolvedField);
                var methodNamePostfix = string.Join("_", fields.Select(f => f.Name));
                var parameters = fields.Select(f =>
                {
                    return (f.Type.ToCSharp(), f.Name.ToCamelCase());
                });
                sb.PublicStaticMethod("global::SqlKata.Query", $"Exists_{entityName}_UK_{methodNamePostfix}", parameters, sb =>
                {
                    var viewName = entity.GetViewName();
                    sb.AL($@"return new global::SqlKata.Query(""{viewName}"")");
                    sb.I(sb =>
                    {
                        foreach (var field in fields)
                        {
                            var fieldName = field.Name;
                            var parameterName = fieldName.ToCamelCase();
                            sb.AL($@".Where(""{fieldName}"", {parameterName})");
                        }
                        sb.AL(@".SelectRaw(""1"")");
                        sb.AL(@".Limit(1);");
                    });
                });
            }
        }

        private static void WriteCount(this SourceBuilder sb, Entity entity)
        {
            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            var fullPrimaryKey = entity.GetFullPrimaryKey().ToList();

            var fields = fullPrimaryKey.Take(fullPrimaryKey.Count - 1).Select(fr =>
            {
                var field = fr.ResolvedField;
                return (field.Type.ToCSharp(), field.Name, field.Name.ToCamelCase());
            }).ToList();

            var stackedGroupable = new Stack<(string Type, string Name, string NameCamel)>(fields);
            var i = stackedGroupable.Count;
            while (i >= 0)
            {
                var stackedPivotable = new Stack<(string Type, string Name, string NameCamel)>(fields.Take(i));
                var j = stackedPivotable.Count;
                while (j >= 0)
                {
                    if (j == 0)
                    {
                        var parameters = stackedGroupable.Select(p => (p.Type, p.NameCamel)).Reverse();
                        var arguments = string.Join(", ", stackedGroupable.Reverse().Select(p => p.NameCamel));
                        sb.PublicStaticMethod("global::SqlKata.Query", $"Count_{entityName}", parameters, sb =>
                        {
                            var viewName = entity.GetViewName();
                            sb.AL($@"return new global::SqlKata.Query(""{viewName}"")");
                            sb.I(sb =>
                            {
                                foreach (var field in stackedGroupable.Reverse())
                                {
                                    var fieldName = field.Name;
                                    var parameterName = field.NameCamel;
                                    sb.AL($@".Where(""{fieldName}"", {parameterName})");
                                }
                                foreach (var field in stackedPivotable.Reverse())
                                {
                                    var fieldName = field.Name;
                                    sb.AL($@".GroupBy(""{fieldName}"")");
                                }
                                foreach (var field in stackedPivotable.Reverse())
                                {
                                    var fieldName = field.Name;
                                    sb.AL($@".Select(""{fieldName}"")");
                                }
                                sb.AL(".AsCount();");
                            });
                        });
                    }
                    else if (i > 0)
                    {
                        var parameters = stackedPivotable.Skip(1).Select(p => (p.Type, p.NameCamel)).Reverse();
                        var groupByName = string.Join("_", stackedGroupable.Reverse().Select(p => p.Name));
                        sb.PublicStaticMethod("global::SqlKata.Query", $"Count_{entityName}_GroupBy_{groupByName}", parameters, sb =>
                        {
                            var viewName = entity.GetViewName();
                            sb.AL($@"return new global::SqlKata.Query(""{viewName}"")");
                            sb.I(sb =>
                            {
                                foreach (var field in stackedPivotable.Skip(1).Reverse())
                                {
                                    var fieldName = field.Name;
                                    var parameterName = field.NameCamel;
                                    sb.AL($@".Where(""{fieldName}"", {parameterName})");
                                }
                                foreach (var field in stackedGroupable.Reverse())
                                {
                                    var fieldName = field.Name;
                                    sb.AL($@".GroupBy(""{fieldName}"")");
                                }
                                foreach (var field in stackedGroupable.Reverse())
                                {
                                    var fieldName = field.Name;
                                    sb.AL($@".Select(""{fieldName}"")");
                                }
                                sb.AL(@".SelectRaw(""COUNT(*) as Count"");");
                            });
                        });
                    }
                    if (stackedPivotable.Count > 0)
                    {
                        stackedPivotable.Pop();
                    }
                    j--;
                }
                if (stackedGroupable.Count > 0)
                {
                    stackedGroupable.Pop();
                }
                i--;
            }
        }

        private static void WriteGet(this SourceBuilder sb, Entity entity)
        {
            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            var fullPrimaryKey = entity.GetFullPrimaryKey().ToList();
            var parameters = fullPrimaryKey.Select(fr =>
            {
                var field = fr.ResolvedField;
                return (field.Type.ToCSharp(), field.Name.ToCamelCase());
            }).ToList();
            var allFields = fullPrimaryKey.Select(fr => fr.ResolvedField).Concat(entity.Fields).Distinct().ToList();

            var stackedFullPrimaryKey = new Stack<FieldReference>(fullPrimaryKey);
            var stackedParameters = new Stack<(string Type, string Name)>(parameters);

            var i = stackedParameters.Count;
            while (i >= 0)
            {
                sb.PublicStaticMethod("global::SqlKata.Query", $"Get_{entityName}", stackedParameters.Reverse(), sb =>
                {
                    var viewName = entity.GetViewName();
                    sb.AL($@"return new global::SqlKata.Query(""{viewName}"")");
                    sb.I(sb =>
                    {
                        foreach (var field in stackedFullPrimaryKey.Reverse())
                        {
                            var fieldName = field.FieldName;
                            var parameterName = fieldName.ToCamelCase();
                            sb.AL($@".Where(""{fieldName}"", {parameterName})");
                        }

                        allFields.ForEach((field, last) =>
                        {
                            var fieldName = field.Name;
                            var semicolon = last ? ";" : "";
                            sb.AL($@".Select(""{fieldName}""){semicolon}");
                        });
                    });
                });
                if (stackedParameters.Count > 0)
                {
                    stackedFullPrimaryKey.Pop();
                    stackedParameters.Pop();
                }
                i--;
            }
        }

        private static void WriteGetUniqueKey(this SourceBuilder sb, Entity entity)
        {
            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            var fullPrimaryKey = entity.GetFullPrimaryKey().ToList();
            var allFields = fullPrimaryKey.Select(fr => fr.ResolvedField).Concat(entity.Fields).Distinct().ToList();
            foreach (var key in entity.UniqueKeys)
            {
                var fields = key.FieldReferences.Select(fr => fr.ResolvedField);
                var methodNamePostfix = string.Join("_", fields.Select(f => f.Name));
                var parameters = fields.Select(f =>
                {
                    return (f.Type.ToCSharp(), f.Name.ToCamelCase());
                });
                sb.PublicStaticMethod("global::SqlKata.Query", $"Get_{entityName}_UK_{methodNamePostfix}", parameters, sb =>
                {
                    var viewName = entity.GetViewName();
                    sb.AL($@"return new global::SqlKata.Query(""{viewName}"")");
                    sb.I(sb =>
                    {
                        foreach (var field in fields)
                        {
                            var fieldName = field.Name;
                            var parameterName = fieldName.ToCamelCase();
                            sb.AL($@".Where(""{fieldName}"", {parameterName})");
                        }

                        allFields.ForEach((field, last) =>
                        {
                            var fieldName = field.Name;
                            var semicolon = last ? ";" : "";
                            sb.AL($@".Select(""{fieldName}""){semicolon}");
                        });
                    });
                });
            }
        }

        private static void WriteInsert(this SourceBuilder sb, Entity entity)
        {
            bool IsOptional(Field field)
            {
                return !field.IsPartOfParentKey(entity) && (field.HasDefault() || field.Nullable);
            }

            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            var fullPrimaryKey = entity.GetFullPrimaryKey();
            var allFields = fullPrimaryKey.Select(fr => fr.ResolvedField).Where(f => f.IsInsertablePK(entity)).Concat(entity.Fields.Where(f => f.IsInsertable(entity))).Distinct();
            var parameters = allFields.Select(f => (IsOptional(f), f.Type.ToCSharp(), f.Name.ToCamelCase()));
            sb.PublicStaticMethod("global::SqlKata.Query", $"Insert_{entityName}", parameters, sb =>
            {
                sb.AL("var _columns = new global::System.Collections.Generic.List<string>();");
                sb.AL("var _values = new global::System.Collections.Generic.List<object?>();");
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
                var tableName = entity.GetTableName();
                sb.AL($@"return new global::SqlKata.Query(""{tableName}"")");
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
                return !field.IsPartOfParentKey(entity) && field.Nullable;
            }

            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            var fullPrimaryKey = entity.GetFullPrimaryKey();
            var allFields = fullPrimaryKey.Select(fr => fr.ResolvedField).Where(f => f.IsInsertablePK(entity)).Concat(entity.Fields.Where(f => f.IsInsertable(entity))).Distinct();
            var tupleParameters = allFields.Select(f =>
            {
                var nullable = IsNullable(f);
                var type = f.Type.ToCSharp();
                return (nullable ? $"{type}?" : type, f.Name.ToPascalCase());
            });
            var parameters = new[] { ($"global::System.Collections.Generic.IEnumerable<({tupleParameters.Join()})>", "rows") };
            sb.PublicStaticMethod("global::SqlKata.Query", $"Insert_{entityName}", parameters, sb =>
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
                sb.AL("var _values = rows.Select(_row => new object?[] {");
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
                var tableName = entity.GetTableName();
                sb.AL($@"return new global::SqlKata.Query(""{tableName}"")");
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
            sb.PublicStaticMethod("global::SqlKata.Query", $"Delete_{entityName}", parameters, sb =>
            {
                var tableName = entity.GetTableName();
                sb.AL($@"return new global::SqlKata.Query(""{tableName}"")");
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

        private static void WriteDeleteUniqueKey(this SourceBuilder sb, Entity entity)
        {
            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            var fullPrimaryKey = entity.GetFullPrimaryKey().ToList();
            var allFields = fullPrimaryKey.Select(fr => fr.ResolvedField).Concat(entity.Fields).Distinct().ToList();
            foreach (var key in entity.UniqueKeys)
            {
                var fields = key.FieldReferences.Select(fr => fr.ResolvedField);
                var methodNamePostfix = string.Join("_", fields.Select(f => f.Name));
                var parameters = fields.Select(f =>
                {
                    return (f.Type.ToCSharp(), f.Name.ToCamelCase());
                });
                sb.PublicStaticMethod("global::SqlKata.Query", $"Delete_{entityName}_UK_{methodNamePostfix}", parameters, sb =>
                {
                    var tableName = entity.GetTableName();
                    sb.AL($@"return new global::SqlKata.Query(""{tableName}"")");
                    sb.I(sb =>
                    {
                        foreach (var field in fields)
                        {
                            var fieldName = field.Name;
                            var parameterName = fieldName.ToCamelCase();
                            sb.AL($@".Where(""{fieldName}"", {parameterName})");
                        }
                        sb.AL(@".AsDelete();");
                    });
                });
            }
        }

        private static void WriteUpdate(this SourceBuilder sb, Mutation mutation)
        {
            bool IsNullable(Field field)
            {
                return !field.IsPartOfParentKey(mutation.Entity) && field.Nullable;
            }

            var entityName = mutation.Entity.DisplayName.Replace(".", "").Replace("::", "_");
            var mutationName = mutation.Name;
            var fullPrimaryKey = mutation.Entity.GetFullPrimaryKey();
            var keyParameters = fullPrimaryKey.Select(fr =>
            {
                var field = fr.ResolvedField;
                return (IsNullable(field), field.Type.ToCSharp(), $"k_{field.Name.ToPascalCase()}");
            });
            var parameters = keyParameters.Concat(mutation.FieldReferences.Select(fr =>
            {
                var field = fr.ResolvedField;
                return (IsNullable(field), field.Type.ToCSharp(), field.Name.ToCamelCase());
            }));
            sb.PublicStaticMethod("global::SqlKata.Query", $"Update_{entityName}_{mutationName}", parameters, sb =>
            {
                var fields = mutation.FieldReferences.Select(fr => fr.ResolvedField);
                sb.AL("var _columns = new string[] {");
                sb.I(sb =>
                {
                    fields.ForEach((field, last) =>
                    {
                        var fieldName = field.Name;
                        var comma = last ? "" : ",";
                        sb.AL($@"""{fieldName}""{comma}");
                    });
                });
                sb.AL("};");
                sb.AL("var _values = new object?[] {");
                sb.I(sb =>
                {
                    fields.ForEach((field, last) =>
                    {
                        var parameterName = field.Name.ToCamelCase();
                        var comma = last ? "" : ",";
                        sb.AL($@"{parameterName}{comma}");
                    });
                });
                sb.AL("};");
                var tableName = mutation.Entity.GetTableName();
                sb.AL($@"return new global::SqlKata.Query(""{tableName}"")");
                sb.I(sb =>
                {
                    foreach (var field in fullPrimaryKey)
                    {
                        var fieldName = field.FieldName;
                        var parameterName = $"k_{fieldName.ToPascalCase()}";
                        sb.AL($@".Where(""{fieldName}"", {parameterName})");
                    }
                    sb.AL(@".AsUpdate(_columns, _values);");
                });
            });
        }

        private static void WriteUpdateUniqueKey(this SourceBuilder sb, Mutation mutation)
        {
            bool IsNullable(Field field)
            {
                return !field.IsPartOfParentKey(mutation.Entity) && field.Nullable;
            }

            var entityName = mutation.Entity.DisplayName.Replace(".", "").Replace("::", "_");
            var mutationName = mutation.Name;
            var fullPrimaryKey = mutation.Entity.GetFullPrimaryKey();

            foreach (var key in mutation.Entity.UniqueKeys)
            {
                var keyFields = key.FieldReferences;
                var keyParameters = keyFields.Select(fr =>
                {
                    var field = fr.ResolvedField;
                    return (IsNullable(field), field.Type.ToCSharp(), $"k_{field.Name.ToPascalCase()}");
                });
                var parameters = keyParameters.Concat(mutation.FieldReferences.Select(fr =>
                {
                    var field = fr.ResolvedField;
                    return (IsNullable(field), field.Type.ToCSharp(), field.Name.ToCamelCase());
                }));
                var methodNamePostfix = string.Join("_", keyFields.Select(fr => fr.FieldName));
                sb.PublicStaticMethod("global::SqlKata.Query", $"Update_{entityName}_{mutationName}_UK_{methodNamePostfix}", parameters, sb =>
                {
                    var fields = mutation.FieldReferences.Select(fr => fr.ResolvedField);
                    sb.AL("var _columns = new string[] {");
                    sb.I(sb =>
                    {
                        fields.ForEach((field, last) =>
                        {
                            var fieldName = field.Name;
                            var comma = last ? "" : ",";
                            sb.AL($@"""{fieldName}""{comma}");
                        });
                    });
                    sb.AL("};");
                    sb.AL("var _values = new object?[] {");
                    sb.I(sb =>
                    {
                        fields.ForEach((field, last) =>
                        {
                            var parameterName = field.Name.ToCamelCase();
                            var comma = last ? "" : ",";
                            sb.AL($@"{parameterName}{comma}");
                        });
                    });
                    sb.AL("};");
                    var tableName = mutation.Entity.GetTableName();
                    sb.AL($@"return new global::SqlKata.Query(""{tableName}"")");
                    sb.I(sb =>
                    {
                        foreach (var field in keyFields)
                        {
                            var fieldName = field.FieldName;
                            var parameterName = $"k_{fieldName.ToPascalCase()}";
                            sb.AL($@".Where(""{fieldName}"", {parameterName})");
                        }
                        sb.AL(@".AsUpdate(_columns, _values);");
                    });
                });
            }
        }
    }
}