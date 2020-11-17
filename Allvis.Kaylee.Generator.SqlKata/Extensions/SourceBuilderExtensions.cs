using System;
using System.Collections.Generic;
using System.Linq;
using Allvis.Kaylee.Generator.SqlKata.Builders;

namespace Allvis.Kaylee.Generator.SqlKata.Extensions
{
    public static class SourceBuilderExtensions
    {
        public static void PublicStaticClass(this SourceBuilder sb, string ns, string className, Action<SourceBuilder> builder)
        {
            sb.AL($"namespace {ns}");
            sb.B(sb =>
            {
                sb.AL($"public static class {className}");
                sb.B(builder);
            });
        }

        public static void PublicStaticMethod(this SourceBuilder sb, string returnType, string methodName, IEnumerable<(string DataType, string ParameterName)> parameters, Action<SourceBuilder> builder)
        {
            sb.AL($"public static {returnType} {methodName}({parameters.Join()})");
            sb.B(builder);
        }
    }
}