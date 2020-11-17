using System;
using System.Collections.Generic;
using System.Linq;

namespace Allvis.Kaylee.Generator.SqlKata.Extensions
{
    public static class StringExtensions
    {
        public static string Indent(this string str, int levels, int spaces = 4)
            => new string(' ', Math.Max(0, levels * spaces)) + str;

        public static string Join(this IEnumerable<(string DataType, string ParameterName)> parameters)
            => string.Join(", ", parameters.Select(p => $"{p.DataType} {p.ParameterName}"));
    }
}