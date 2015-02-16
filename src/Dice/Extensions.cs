using System;
using System.Collections.Generic;
using System.Linq;

namespace Statistics
{
    static class Extensions
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

        public static double Product(this IEnumerable<double> source)
        {
            return source.Aggregate(1.0, (a, b) => a * b);
        }

        public static double Product<T>(this IEnumerable<T> source, Func<T, double> select)
        {
            return source.Aggregate(1.0, (a, b) => a * select(b));
        }

        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> items, int numDigits)
        {
            var list = items.ToList();
            foreach (var ix in Combinations(numDigits, list.Count))
            {
                yield return ix.Select(i => list[i]).ToArray();
            }
        }

        private static IEnumerable<int[]> Combinations(int numDigits, int @base)
        {
            var n = (int)Math.Pow(@base, numDigits);
            for (int i = 0; i < n; i++)
            {
                var current = i;
                var number = new int[numDigits];
                for (int j = 0; j < numDigits; j++)
                {
                    number[j] = current % @base;
                    current = current / @base;
                }
                yield return number;
            }
        }

        public static string Format<T>(this IEnumerable<T> source, string separator, string format, params Func<T, object>[] args)
        {
            return string.Join(separator, source.Select(item => string.Format(format, args.Select(a => a(item)).ToArray())));
        }
    }
}