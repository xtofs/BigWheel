using System;
using System.Collections.Generic;
using System.Linq;

namespace Xof.RandomVariables
{
    using Number = System.Decimal;
              
    static class Random
    {
        public static IRandom<int> Dice(int sides)
        {
            return new _Random<int>(Enumerable.Range(1, sides).Select(i => Probability(i, (Number)1 / (Number)sides)));
        }

        public static IRandom<int> Coin()
        {
            return Coin((Number)1 / (Number)2);
        }

        public static IRandom<int> Coin(Number pHeads)
        {
            return new _Random<int>(new[] { Probability(0, pHeads), Probability(1, 1 - pHeads) });
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


        private static KeyValuePair<T, Number> Probability<T>(T value, Number probability)
        {
            return new KeyValuePair<T, Number>(value, probability);
        }

        private static Number Zero = 0;

        private class _Random<T> : Dictionary<T, Number>, IRandom<T>
        {
            private readonly List<Number> intervals;
            private readonly List<T> items;

            public _Random(IEnumerable<KeyValuePair<T, Number>> enumerable) :
                base(GroupAndSum(enumerable))
            {
                this.items = this.Select(p => p.Key).ToList();
                this.intervals = this.Scan(Zero, (v, a) => a + v.Value).ToList();

                System.Diagnostics.Debug.Assert(intervals.Last() - (Number)1 < 1E-10m);
            }

            private static Dictionary<T, Number> GroupAndSum(IEnumerable<KeyValuePair<T, Number>> enumerable)
            {
                return enumerable
                    .GroupBy(e => e.Key, e => e.Value)
                    .ToDictionary(p => p.Key, p => p.Sum());
            }

            public IEnumerable<T> Sample(System.Random rand)
            {
                while (true)
                {
                    var d = (Number)rand.NextDouble();
                    int ix = intervals.BinarySearch(d);
                    ix = ix < 0 ? (~ix) : ix;
                    yield return items[ix];
                }
            }
        }
    }
}