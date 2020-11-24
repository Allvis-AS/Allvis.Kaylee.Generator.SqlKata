using System;
using System.Collections.Generic;
using System.Linq;
using Allvis.Kaylee.Generator.SqlKata.Builders;

namespace Allvis.Kaylee.Generator.SqlKata.Extensions
{
    public static class SourceBuilderExtensions
    {
        public static void PublicClass(
            this SourceBuilder sb,
            string ns,
            string className,
            Action<SourceBuilder> builder)
        {
            sb.Namespace(ns, sb =>
            {
                sb.PublicClass(className, builder);
            });
        }

        public static void PublicClass(
            this SourceBuilder sb,
            string className,
            Action<SourceBuilder> builder)
        {
            sb.AL($"public class {className}");
            sb.B(builder);
        }

        public static void PublicStaticClass(
            this SourceBuilder sb,
            string ns,
            string className,
            Action<SourceBuilder> builder)
        {
            sb.Namespace(ns, sb =>
            {
                sb.PublicStaticClass(className, builder);
            });
        }

        public static void PublicStaticClass(
            this SourceBuilder sb,
            string className,
            Action<SourceBuilder> builder)
        {
            sb.AL($"public static class {className}");
            sb.B(builder);
        }

        private static void Namespace(
            this SourceBuilder sb,
            string ns,
            Action<SourceBuilder> builder)
        {
            sb.AL($"namespace {ns}");
            sb.B(builder);
        }

        public static void PublicStaticMethod(
            this SourceBuilder sb,
            string returnType,
            string methodName,
            IEnumerable<(bool Optional, string DataType, string ParameterName)> parameters,
            Action<SourceBuilder> builder)
            => sb.PublicStaticMethod(
                returnType,
                methodName,
                parameters.Select(p => (p.Optional ? $"{p.DataType}?" : p.DataType, p.ParameterName)),
                builder);

        public static void PublicStaticMethod(
            this SourceBuilder sb,
            string returnType,
            string methodName,
            IEnumerable<(string DataType, string ParameterName)> parameters,
            Action<SourceBuilder> builder)
        {
            sb.AL($"public static {returnType} {methodName}({parameters.Join()})");
            sb.B(builder);
        }
    }
}