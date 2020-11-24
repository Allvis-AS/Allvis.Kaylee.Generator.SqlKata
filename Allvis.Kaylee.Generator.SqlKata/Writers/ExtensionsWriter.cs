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
    public static class ExtensionsWriter
    {
        public static string Write(Ast ast)
        {
            var sb = new SourceBuilder();
            sb.AL("#nullable enable");
            sb.NL();
            sb.AL("using System.Linq;");
            sb.NL();
            sb.PublicStaticClass("Allvis.Kaylee.Generated.SqlKata", "Extensions", sb =>
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
                return (Type: field.Type.ToCSharp(), Name: field.Name.ToCamelCase());
            });
            sb.PublicStaticMethod("async global::System.Threading.Tasks.Task<bool>", $"Exists_{entityName}", parameters.PrefixWithQueryFactory(), sb =>
            {
                var arguments = string.Join(", ", parameters.Select(p => p.Name));
                sb.AL($"var _rows = await _db.GetAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Exists_{entityName}({arguments})).ConfigureAwait(false);");
                sb.AL("return _rows.Any();");
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
                    return (Type: f.Type.ToCSharp(), Name: f.Name.ToCamelCase());
                });
                sb.PublicStaticMethod("async global::System.Threading.Tasks.Task<bool>", $"Exists_{entityName}_UK_{methodNamePostfix}", parameters.PrefixWithQueryFactory(), sb =>
                {
                    var arguments = string.Join(", ", parameters.Select(p => p.Name));
                    sb.AL($"var _rows = await _db.GetAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Exists_{entityName}_UK_{methodNamePostfix}({arguments})).ConfigureAwait(false);");
                    sb.AL("return _rows.Any();");
                });
            }
        }

        private static void WriteCount(this SourceBuilder sb, Entity entity)
        {
            var modelName = $"global::Allvis.Kaylee.Generated.SqlKata.Entities.{entity.DisplayName.Replace(".", "").Replace("::", ".")}";
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
                        var parameters = stackedGroupable.Select(p => (p.Type, p.NameCamel)).Reverse().PrefixWithQueryFactory();
                        var arguments = string.Join(", ", stackedGroupable.Reverse().Select(p => p.NameCamel));
                        sb.PublicStaticMethod("global::System.Threading.Tasks.Task<int>", $"Count_{entityName}", parameters, sb =>
                        {
                            sb.AL($@"return _db.ExecuteScalarAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Count_{entityName}({arguments}));");
                        });
                    }
                    else if (i > 0)
                    {
                        var parameters = stackedPivotable.Skip(1).Select(p => (p.Type, p.NameCamel)).Reverse().PrefixWithQueryFactory();
                        var arguments = string.Join(", ", stackedPivotable.Skip(1).Reverse().Select(p => p.NameCamel));
                        var resultTuple = stackedGroupable.Reverse().Select(p => (p.Type, p.Name)).Concat(new[] { ("int", "Count") }).Join();
                        var groupByName = string.Join("_", stackedGroupable.Reverse().Select(p => p.Name));
                        sb.PublicStaticMethod($"global::System.Threading.Tasks.Task<global::System.Collections.Generic.IEnumerable<({resultTuple})>>", $"Count_{entityName}_GroupBy_{groupByName}", parameters, sb =>
                        {
                            sb.AL($@"return _db.GetAsync<({resultTuple})>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Count_{entityName}_GroupBy_{groupByName}({arguments}));");
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
            var modelName = $"global::Allvis.Kaylee.Generated.SqlKata.Entities.{entity.DisplayName.Replace(".", "").Replace("::", ".")}";
            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            var fullPrimaryKey = entity.GetFullPrimaryKey().ToList();
            var parameters = fullPrimaryKey.Select(fr =>
            {
                var field = fr.ResolvedField;
                return (field.Type.ToCSharp(), field.Name.ToCamelCase());
            }).ToList();

            var stackedParameters = new Stack<(string Type, string Name)>(parameters);

            var i = stackedParameters.Count;
            while (i >= 0)
            {
                var singular = stackedParameters.Count == parameters.Count;
                var returnType = singular
                    ? $"global::System.Threading.Tasks.Task<{modelName}>"
                    : $"global::System.Threading.Tasks.Task<global::System.Collections.Generic.IEnumerable<{modelName}>>";
                sb.PublicStaticMethod(returnType, $"Get_{entityName}", stackedParameters.Reverse().PrefixWithQueryFactory(), sb =>
                {
                    var getMethod = singular ? "FirstAsync" : "GetAsync";
                    var arguments = string.Join(", ", stackedParameters.Reverse().Select(p => p.Name));
                    sb.AL($@"return _db.{getMethod}<{modelName}>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Get_{entityName}({arguments}));");
                });
                if (stackedParameters.Count > 0)
                {
                    stackedParameters.Pop();
                }
                i--;
            }
        }

        private static void WriteGetUniqueKey(this SourceBuilder sb, Entity entity)
        {
            var modelName = $"global::Allvis.Kaylee.Generated.SqlKata.Entities.{entity.DisplayName.Replace(".", "").Replace("::", ".")}";
            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            var fullPrimaryKey = entity.GetFullPrimaryKey().ToList();
            foreach (var key in entity.UniqueKeys)
            {
                var fields = key.FieldReferences.Select(fr => fr.ResolvedField);
                var methodNamePostfix = string.Join("_", fields.Select(f => f.Name));
                var parameters = fields.Select(f =>
                {
                    return (Type: f.Type.ToCSharp(), Name: f.Name.ToCamelCase());
                });
                var returnType = $"global::System.Threading.Tasks.Task<{modelName}>";
                sb.PublicStaticMethod(returnType, $"Get_{entityName}_UK_{methodNamePostfix}", parameters.PrefixWithQueryFactory(), sb =>
                {
                    var arguments = string.Join(", ", parameters.Select(p => p.Name));
                    sb.AL($@"return _db.FirstAsync<{modelName}>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Get_{entityName}_UK_{methodNamePostfix}({arguments}));");
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
            var parameters = allFields.Select(f => (Optional: IsOptional(f), Type: f.Type.ToCSharp(), Name: f.Name.ToCamelCase()));
            sb.PublicStaticMethod("global::System.Threading.Tasks.Task<int>", $"Insert_{entityName}", parameters.PrefixWithQueryFactory(), sb =>
            {
                var arguments = string.Join(", ", parameters.Select(p => p.Name));
                sb.AL($"return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Insert_{entityName}({arguments}));");
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
            var parameters = new[] { (Type: $"global::System.Collections.Generic.IEnumerable<({tupleParameters.Join()})>", Name: "rows") };
            sb.PublicStaticMethod("global::System.Threading.Tasks.Task<int>", $"Insert_{entityName}", parameters.PrefixWithQueryFactory(), sb =>
            {
                var arguments = string.Join(", ", parameters.Select(p => p.Name));
                sb.AL($"return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Insert_{entityName}({arguments}));");
            });
        }

        private static void WriteDelete(this SourceBuilder sb, Entity entity)
        {
            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            var fullPrimaryKey = entity.GetFullPrimaryKey();
            var parameters = fullPrimaryKey.Select(fr =>
            {
                var field = fr.ResolvedField;
                return (Type: field.Type.ToCSharp(), Name: field.Name.ToCamelCase());
            }).ToList();

            var stackedParameters = new Stack<(string Type, string Name)>(parameters);

            var i = stackedParameters.Count;
            while (i >= 0)
            {
                sb.PublicStaticMethod("global::System.Threading.Tasks.Task<int>", $"Delete_{entityName}", stackedParameters.Reverse().PrefixWithQueryFactory(), sb =>
                {
                    var arguments = string.Join(", ", stackedParameters.Reverse().Select(p => p.Name));
                    sb.AL($"return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Delete_{entityName}({arguments}));");
                });
                if (stackedParameters.Count > 0)
                {
                    stackedParameters.Pop();
                }
                i--;
            }
        }

        private static void WriteDeleteUniqueKey(this SourceBuilder sb, Entity entity)
        {
            var entityName = entity.DisplayName.Replace(".", "").Replace("::", "_");
            foreach (var key in entity.UniqueKeys)
            {
                var fields = key.FieldReferences.Select(fr => fr.ResolvedField);
                var methodNamePostfix = string.Join("_", fields.Select(f => f.Name));
                var parameters = fields.Select(f =>
                {
                    return (Type: f.Type.ToCSharp(), Name: f.Name.ToCamelCase());
                });
                sb.PublicStaticMethod("global::System.Threading.Tasks.Task<int>", $"Delete_{entityName}_UK_{methodNamePostfix}", parameters.PrefixWithQueryFactory(), sb =>
                {
                    var arguments = string.Join(", ", parameters.Select(p => p.Name));
                    sb.AL($"return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Delete_{entityName}_UK_{methodNamePostfix}({arguments}));");
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
                return (Optional: IsNullable(field), Type: field.Type.ToCSharp(), Name: $"k_{field.Name.ToPascalCase()}");
            });
            var parameters = keyParameters.Concat(mutation.FieldReferences.Select(fr =>
            {
                var field = fr.ResolvedField;
                return (Optional: IsNullable(field), Type: field.Type.ToCSharp(), Name: field.Name.ToCamelCase());
            }));
            sb.PublicStaticMethod("global::System.Threading.Tasks.Task<int>", $"Update_{entityName}_{mutationName}", parameters.PrefixWithQueryFactory(), sb =>
            {
                var arguments = string.Join(", ", parameters.Select(p => p.Name));
                sb.AL($"return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Update_{entityName}_{mutationName}({arguments}));");
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
                    return (Optional: IsNullable(field), Type: field.Type.ToCSharp(), Name: $"k_{field.Name.ToPascalCase()}");
                });
                var parameters = keyParameters.Concat(mutation.FieldReferences.Select(fr =>
                {
                    var field = fr.ResolvedField;
                    return (Optional: IsNullable(field), Type: field.Type.ToCSharp(), Name: field.Name.ToCamelCase());
                }));
                var methodNamePostfix = string.Join("_", keyFields.Select(fr => fr.FieldName));
                sb.PublicStaticMethod("global::System.Threading.Tasks.Task<int>", $"Update_{entityName}_{mutationName}_UK_{methodNamePostfix}", parameters.PrefixWithQueryFactory(), sb =>
                {
                    var arguments = string.Join(", ", parameters.Select(p => p.Name));
                    sb.AL($"return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Update_{entityName}_{mutationName}_UK_{methodNamePostfix}({arguments}));");
                });
            }
        }

        private static IEnumerable<(bool Optional, string Type, string Name)> PrefixWithQueryFactory(this IEnumerable<(bool Optional, string Type, string Name)> parameters)
            => new[] { (false, "this global::SqlKata.Execution.QueryFactory", "_db") }.Concat(parameters);

        private static IEnumerable<(string Type, string Name)> PrefixWithQueryFactory(this IEnumerable<(string Type, string Name)> parameters)
            => new[] { ("this global::SqlKata.Execution.QueryFactory", "_db") }.Concat(parameters);
    }
}