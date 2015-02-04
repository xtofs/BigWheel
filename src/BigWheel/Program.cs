using System;
using System.Collections.Generic;
using System.Linq;

namespace Statistics
{
    class Program
    {
        static void Main(string[] args)
        {
            var twoDice =
              from d1 in Random.Dice(6)
              from d2 in Random.Dice(6)
              select d1 + d2;
            Show("two dice", twoDice); 

             var threeDiceThreePlus =
                 from d1 in Random.Dice(6)
                 from d2 in Random.Dice(6)
                 from d3 in Random.Dice(6)
                 select new[] { d1, d2, d3 }.Count(d => d >= 3);
            Show("three dice 3+", threeDiceThreePlus);

            var fourDiceFourPlus =
              from ds in Random.Dice(6).Repeat(4)
              select ds.Count(d => d >= 4);
            Show("four dice 4+", fourDiceFourPlus);

            var threeDiceThreePlusCrawlers =
                from ds in Random.Dice(6).Repeat(3)
                let hits = ds.Count(d => d >= 3)
                select new { Hits = hits, Crawlers = Math.Min(hits, ds.Count(d => d == 1)) };
            Show("three dice 3+ show crawlers", threeDiceThreePlusCrawlers, p => string.Format("{0}/{1}", p.Hits, p.Crawlers));
        }

        private static void Show<T>(string title, IRandom<T> random, Func<T, string> fmt = null)
        {
            fmt = fmt ?? (t => t.ToString());
            var rand = new System.Random();

            Console.WriteLine("# ############################################################");
            Console.WriteLine("# {0}", title);
            foreach (var outcome in random)
            {
                Console.WriteLine("{0,2} {1,7:P2}", fmt(outcome.Key), outcome.Value);
            }
            Console.WriteLine();

            Console.WriteLine(random.Sample(rand).Take(10).Format(" ", "{0}", p => fmt(p)));
            Console.WriteLine();

            var n = 1000000;
            var sample = random.Sample(rand).Take(n);
            var pairs = sample.GroupBy(p => p).Select(p => new { p.Key, P = p.Count() / (double)n }).OrderBy(p => p.P);
            Console.WriteLine(pairs.Format(Environment.NewLine, "{0,2} {1,7:P2}", p => fmt(p.Key), p => p.P));
            Console.WriteLine();
        }
    }

    /// <summary>
    /// discrete random variable
    /// </summary>    
    interface IRandom<T> : IDictionary<T, double>
    {
        IEnumerable<T> Sample(System.Random rand);
    }

    static class Random
    {
        public static IRandom<int> Dice(int sides)
        {
            return new _Random<int>(Enumerable.Range(1, 6).Select(i => Probability(i, 1.0 / sides)));
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
