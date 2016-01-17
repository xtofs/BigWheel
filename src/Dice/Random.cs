using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Xtof.RandomVariables
{                            
    static class Random
    {
        public static IRandom<int> Dice(int sides)
        {
            return new _Random<int>(Enumerable.Range(1, sides).Select(i => KeyValuePair(i, (Rational)1 / (Rational)sides)));
        }

        public static IRandom<int> Coin()
        {
            return Coin((Rational)1 / (Rational)2);
        }

        public static IRandom<int> Coin(Rational pHeads)
        {
            return new _Random<int>(new[] { KeyValuePair(0, pHeads), KeyValuePair(1, 1 - pHeads) });
        }

        public static IRandom<Tuple<T1,T2>> Tuple<T1,T2>(IRandom<T1> r1, IRandom<T2> r2)
        {
            return from v1 in r1 from v2 in r2 select System.Tuple.Create(v1, v2);
        }

        #region combinators

        public static IRandom<T> Select<S, T>(this IRandom<S> source, Func<S, T> selector)
        {
            var ts =
                from s in source
                select KeyValuePair(selector(s.Key), s.Value);

            return new _Random<T>(ts);
        }

        public static IRandom<U> SelectMany<S, T, U>(this IRandom<S> source, Func<S, IRandom<T>> second, Func<S, T, U> selector)
        {
            var us =
                from s in source
                from t in second(s.Key)
                select KeyValuePair(selector(s.Key, t.Key), s.Value * t.Value);

            return new _Random<U>(us);
        }

        public static IRandom<IEnumerable<T>> Repeat<T>(this IRandom<T> source, int n)
        {
            var pps = source
                .Combinations(n)
                .Select(pa => KeyValuePair(
                    pa.Select(p => p.Key),
                    pa.Product(p => p.Value)));
            return new _Random<IEnumerable<T>>(pps);
        }           
        #endregion  

        private static KeyValuePair<TK, TV> KeyValuePair<TK,TV>(TK key, TV value)
        {
            return new KeyValuePair<TK, TV>(key, value);
        }

        /// <summary>
        /// concrete implementation of IRandom constructed from an IEnumerable<KeyValuePair<T, N>> 
        /// if a key is occuring multiple times in the input, the final probability is the sum of values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class _Random<T> : ReadOnlyDictionary<T, Rational>, IRandom<T>
        {
            public _Random(IEnumerable<KeyValuePair<T, Rational>> enumerable) :
              base(enumerable.ReduceToSum())
            {
                this._items = this.Select(p => p.Key).ToList();
                this._intervals = this.Scan((Rational)0, (v, a) => a + v.Value).ToList();

                // System.Diagnostics.Tracing. (intervals.Last() == (Rational)1);
            }
            
            // _intervals and _items are used in combination to create random samples 
            // _intervals is used to do a binary search according to the distribution
            // that index is then used to look up the item in _items.
            private readonly List<Rational> _intervals;
            private readonly List<T> _items;
         
            public IEnumerable<T> Sample(System.Random rand)
            {
                while (true)
                {
                    var d = (Rational)rand.NextDouble();
                    int ix = _intervals.BinarySearch(d);
                    ix = ix < 0 ? (~ix) : ix;
                    yield return _items[ix];
                }
            }
        }
    }
}