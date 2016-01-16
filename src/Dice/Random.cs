using System;
using System.Collections.Generic;
using System.Linq;

namespace Statistics
{
    static class Random
    {
        public static IRandom<int> Dice(int sides)
        {
        }

        public static IRandom<int> Coin(double pZero = 0.5)
        {
            return new _Random<int>(new[] { Probability(0, pZero), Probability(1, 1.0 - pZero) });
        }

        #region combinators

        public static IRandom<T> Select<S, T>(this IRandom<S> source, Func<S, T> selector)
        {
            var ts =
                from s in source
                select Probability(selector(s.Key), s.Value);

            return new _Random<T>(ts);
        }

        public static IRandom<U> SelectMany<S, T, U>(this IRandom<S> source, Func<S, IRandom<T>> second, Func<S, T, U> selector)
        {
            var us =
                from s in source
                from t in second(s.Key)
                select Probability(selector(s.Key, t.Key), s.Value * t.Value);

            return new _Random<U>(us);
        }

        public static IRandom<IEnumerable<T>> Repeat<T>(this IRandom<T> source, int n)
        {
            var pps = source
                .Combinations(n)
                .Select(pa => Probability(
                    pa.Select(p => p.Key),
                    pa.Product(p => p.Value)));
            return new _Random<IEnumerable<T>>(pps);
        }

        #endregion

        private static KeyValuePair<T, double> Probability<T>(T value, double probability)
        {
            return new KeyValuePair<T, double>(value, probability);
        }

        private class _Random<T> : Dictionary<T, double>, IRandom<T>
        {
            private readonly List<double> intervals;
            private readonly List<T> items;

            public _Random(IEnumerable<KeyValuePair<T, double>> enumerable) :
                base(GroupAndSum(enumerable))
            {
                this.items = this.Select(p => p.Key).ToList();
                this.intervals = this.Scan(0.0, (v, a) => a + v.Value).ToList();
            }

            private static Dictionary<T, double> GroupAndSum(IEnumerable<KeyValuePair<T, double>> enumerable)
            {
                return enumerable
                    .GroupBy(e => e.Key, e => e.Value)
                    .ToDictionary(p => p.Key, p => p.Sum());
            }

            public IEnumerable<T> Sample(System.Random rand)
            {
                while (true)
                {
                    var d = rand.NextDouble();
                    int ix = intervals.BinarySearch(d);
                    ix = ix < 0 ? (~ix) : ix;
                    yield return items[ix];
                }
            }
        }
    }
}