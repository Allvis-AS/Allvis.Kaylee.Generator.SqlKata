using System;
using System.Collections.Generic;

namespace Allvis.Kaylee.Generator.SqlKata.Extensions
{
    public delegate void ForEachAction<in T>(T obj, bool last);

    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, ForEachAction<T> action)
        {
            using var iter = source.GetEnumerator();
            if (iter.MoveNext())
            {
                var prev = iter.Current;
                while (iter.MoveNext())
                {
                    action(prev, false);
                    prev = iter.Current;
                }
                action(prev, true);
            }
        }
    }
}