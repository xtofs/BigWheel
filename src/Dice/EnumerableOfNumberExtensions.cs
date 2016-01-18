using System;
using System.Collections.Generic;
using System.Linq;                      
using Xtof.Numeric;

namespace Xtof.RandomVariables
{
    static class EnumerableOfDecimalExtensions
    {        
        public static Decimal Sum(this IEnumerable<Decimal> source)
        {
            return source.Aggregate((Decimal)0, (a, b) => a + b);
        }

        public static Decimal Product(this IEnumerable<Decimal> source)
        {
            return source.Aggregate((Decimal)1, (a, b) => a * b);
        }

        public static Decimal Product<T>(this IEnumerable<T> source, Func<T, Decimal> select)
        {
            return source.Aggregate((Decimal)1, (a, b) => a * select(b));
        }

        public static IDictionary<T, Decimal> ReduceToSum<T>(this IEnumerable<KeyValuePair<T, Decimal>> enumerable)
        {
            return enumerable.ReduceByKey(grp => grp.Sum());
        }
    }

    static class EnumerableOfDoubleExtensions
    {        
        public static Double Sum(this IEnumerable<Double> source)
        {
            return source.Aggregate((Double)0, (a, b) => a + b);
        }

        public static Double Product(this IEnumerable<Double> source)
        {
            return source.Aggregate((Double)1, (a, b) => a * b);
        }

        public static Double Product<T>(this IEnumerable<T> source, Func<T, Double> select)
        {
            return source.Aggregate((Double)1, (a, b) => a * select(b));
        }

        public static IDictionary<T, Double> ReduceToSum<T>(this IEnumerable<KeyValuePair<T, Double>> enumerable)
        {
            return enumerable.ReduceByKey(grp => grp.Sum());
        }
    }

    static class EnumerableOfRationalExtensions
    {        
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
    }

}
