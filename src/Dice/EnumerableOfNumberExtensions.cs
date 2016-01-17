using System;
using System.Collections.Generic;
using System.Linq;                      

namespace Xtof.RandomVariables
{
    static class EnumerableOfNumberExtensions
    {
        #region decimal
        public static decimal Sum(this IEnumerable<decimal> source)
        {
            return source.Aggregate((decimal)0, (a, b) => a + b);
        }

        public static decimal Product(this IEnumerable<decimal> source)
        {
            return source.Aggregate((decimal)1, (a, b) => a * b);
        }

        public static decimal Product<T>(this IEnumerable<T> source, Func<T, decimal> select)
        {
            return source.Aggregate((decimal)1, (a, b) => a * select(b));
        }

        public static IDictionary<T, decimal> ReduceToSum<T>(this IEnumerable<KeyValuePair<T, decimal>> enumerable)
        {
            return enumerable.ReduceByKey(grp => grp.Sum());
        }
        #endregion

        #region double
        public static double Sum(this IEnumerable<double> source)
        {
            return source.Aggregate((double)0, (a, b) => a + b);
        }

        public static double Product(this IEnumerable<double> source)
        {
            return source.Aggregate((double)1, (a, b) => a * b);
        }

        public static double Product<T>(this IEnumerable<T> source, Func<T, double> select)
        {
            return source.Aggregate((double)1, (a, b) => a * select(b));
        }

        public static IDictionary<T, double> ReduceToSum<T>(this IEnumerable<KeyValuePair<T, double>> enumerable)
        {
            return enumerable.ReduceByKey(grp => grp.Sum());
        }
        #endregion

        #region Rational
        public static Rational Sum(this IEnumerable<Rational> source)
        {
            return source.Aggregate((Rational)0, (a, b) => a + b);
        }

        public static Rational Product(this IEnumerable<Rational> source)
        {
            return source.Aggregate((Rational)1, (a, b) => a * b);
        }

        public static Rational Product<T>(this IEnumerable<T> source, Func<T, Rational> select)
        {
            return source.Aggregate((Rational)1, (a, b) => a * select(b));
        }

        public static IDictionary<T, Rational> ReduceToSum<T>(this IEnumerable<KeyValuePair<T, Rational>> enumerable)
        {
            return enumerable.ReduceByKey(grp => grp.Sum());
        }
        #endregion

    }
}
