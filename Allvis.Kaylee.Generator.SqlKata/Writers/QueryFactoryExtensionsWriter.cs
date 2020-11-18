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
    public static class QueryFactoryExtensionsWriter
    {
        public static string Write(Ast ast)
        {
            var sb = new SourceBuilder();
            sb.PublicStaticClass("Allvis.Kaylee.Generated.SqlKata.Extensions", "QueryFactoryExtensions", sb =>
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
            sb.WriteGet(entity);
            sb.WriteInsert(entity);
            sb.WriteInsertMany(entity);
            sb.WriteDelete(entity);
            foreach (var mutation in entity.Mutations)
            {
                sb.WriteUpdate(mutation);
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
            sb.PublicStaticMethod("async System.Threading.Tasks.Task<bool>", $"Exists_{entityName}", parameters.PrefixWithQueryFactory(), sb =>
            {
                var arguments = string.Join(", ", parameters.Select(p => p.Name));
                sb.AL($"var _rows = await _db.GetAsync<int>(Queries.Exists_{entityName}({arguments})).ConfigureAwait(false);");
                sb.AL("return System.Linq.Enumerable.Any(_rows);");
            });
        }

        private static void WriteGet(this SourceBuilder sb, Entity entity)
        {
            var modelName = $"Models.{entity.DisplayName.Replace(".", "").Replace("::", ".")}";
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
                var singular = stackedParameters.Count == parameters.Count;
                var returnType = singular
                    ? $"System.Threading.Tasks.Task<{modelName}>"
                    : $"System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<{modelName}>>";
                sb.PublicStaticMethod(returnType, $"Get_{entityName}", stackedParameters.Reverse().PrefixWithQueryFactory(), sb =>
                {
                    var getMethod = singular ? "FirstAsync" : "GetAsync";
                    var arguments = string.Join(", ", stackedParameters.Reverse().Select(p => p.Name));
                    sb.AL($@"return _db.{getMethod}<{modelName}>(Queries.Get_{entityName}({arguments}));");
                });
                if (stackedParameters.Count > 0)
                {
                    stackedFullPrimaryKey.Pop();
                    stackedParameters.Pop();
                }
                i--;
            }
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
            var allFields = fullPrimaryKey.Select(fr => fr.ResolvedField).Concat(entity.Fields.Where(f => !f.Computed)).Distinct();
            var parameters = allFields.Select(f => (Optional: IsOptional(f), Type: f.Type.ToCSharp(), Name: f.Name.ToCamelCase()));
            sb.PublicStaticMethod("System.Threading.Tasks.Task<int>", $"Insert_{entityName}", parameters.PrefixWithQueryFactory(), sb =>
            {
                var arguments = string.Join(", ", parameters.Select(p => p.Name));
                sb.AL($"return _db.ExecuteAsync(Queries.Insert_{entityName}({arguments}));");
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
            var allFields = fullPrimaryKey.Select(fr => fr.ResolvedField).Concat(entity.Fields.Where(f => !f.Computed)).Distinct();
            var tupleParameters = allFields.Select(f =>
            {
                var nullable = IsNullable(f);
                var type = f.Type.ToCSharp();
                return (nullable ? $"{type}?" : type, f.Name.ToPascalCase());
            });
            var parameters = new[] { (Type: $"System.Collections.Generic.IEnumerable<({tupleParameters.Join()})>", Name: "rows") };
            sb.PublicStaticMethod("System.Threading.Tasks.Task<int>", $"Insert_{entityName}", parameters.PrefixWithQueryFactory(), sb =>
            {
                var arguments = string.Join(", ", parameters.Select(p => p.Name));
                sb.AL($"return _db.ExecuteAsync(Queries.Insert_{entityName}({arguments}));");
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
            });
            sb.PublicStaticMethod("System.Threading.Tasks.Task<int>", $"Delete_{entityName}", parameters.PrefixWithQueryFactory(), sb =>
            {
                var arguments = string.Join(", ", parameters.Select(p => p.Name));
                sb.AL($"return _db.ExecuteAsync(Queries.Delete_{entityName}({arguments}));");
            });
        }

        private static void WriteUpdate(this SourceBuilder sb, Mutation mutation)
        {
            bool IsNullable(Field field)
            {
                var partOfParentKey = field.Entity != mutation.Entity;
                return !partOfParentKey && field.Nullable;
            }

            var entityName = mutation.Entity.DisplayName.Replace(".", "").Replace("::", "_");
            var mutationName = mutation.Name;
            var fullPrimaryKey = mutation.Entity.GetFullPrimaryKey();
            var allFields = fullPrimaryKey.Concat(mutation.FieldReferences).Select(fr => fr.ResolvedField).Distinct();
            var parameters = allFields.Select(f =>
            {
                return (Optional: IsNullable(f), Type: f.Type.ToCSharp(), Name: f.Name.ToCamelCase());
            });
            sb.PublicStaticMethod("System.Threading.Tasks.Task<int>", $"Update_{entityName}_{mutationName}", parameters.PrefixWithQueryFactory(), sb =>
            {
                var arguments = string.Join(", ", parameters.Select(p => p.Name));
                sb.AL($"return _db.ExecuteAsync(Queries.Update_{entityName}_{mutationName}({arguments}));");
            });
        }

        private static IEnumerable<(bool Optional, string Type, string Name)> PrefixWithQueryFactory(this IEnumerable<(bool Optional, string Type, string Name)> parameters)
            => new[] { (false, "this SqlKata.Execution.QueryFactory", "_db") }.Concat(parameters);

        private static IEnumerable<(string Type, string Name)> PrefixWithQueryFactory(this IEnumerable<(string Type, string Name)> parameters)
            => new[] { ("this SqlKata.Execution.QueryFactory", "_db") }.Concat(parameters);
    }
}