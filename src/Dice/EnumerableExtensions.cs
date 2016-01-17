using System;
using System.Collections.Generic;
using System.Linq;

namespace Xtof.RandomVariables
{
    static class EnumerableExtensions
    {
        public static IEnumerable<U> Scan<T, U>(this IEnumerable<T> source, U init, Func<T, U, U> selector)
        {
            var x = init;
            foreach (var item in source)
            {
                x = selector(item, x);
                yield return x;
            }
        }

        public static string Format<T>(this IEnumerable<T> source, params Func<T, object>[] args)
        {
            return source.Format(" ", "{0}", args);
        }

        public static string Format<T>(this IEnumerable<T> source, string separator, params Func<T, object>[] args)
        {
            return source.Format(separator, "{0}", args);
        }                                                

        public static string Format<T>(this IEnumerable<T> source, string separator, string format, params Func<T, object>[] args)
        {
            return string.Join(separator, source.Select(item => string.Format(format, args.Select(a => a(item)).ToArray())));
        }

        public static IDictionary<T, TV> ReduceByKey<T,TV>(this IEnumerable<KeyValuePair<T, TV>> enumerable, Func<IEnumerable<TV>,TV> reduce)
        {
            return enumerable
                .GroupBy(e => e.Key, e => e.Value)
                .ToDictionary(grp => grp.Key, grp => reduce(grp));
        }                              

        /// <summary>
        /// Collection of all combinations of items of length n
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IEnumerable<IReadOnlyCollection<T>> Combinations<T>(this IReadOnlyCollection<T> items, int n)
        {
            var list = items.ToList();
            foreach (var ix in Combinations(n, list.Count))
            {
                yield return ix.Select(i => list[i]).ToArray();
            }
        }

        /// <summary>
        /// helper method to create combinations of indices
        /// </summary>
        /// <param name="len"></param>
        /// <param name="base"></param>
        /// <returns></returns>
        private static IEnumerable<int[]> Combinations(int len, int @base)
        {
            var n = (int)Math.Pow(@base, len);
            for (int i = 0; i < n; i++)
            {
                var current = i;
                var item = new int[len];
                for (int j = 0; j < len; j++)
                {
                    item[j] = current % @base;
                    current = current / @base;
                }
                yield return item;
            }
        }
    }                         
}