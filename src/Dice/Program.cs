using System;
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

            // dice roles from teh rules of Zombicide

            var threeDiceThreePlus =
                 from d1 in Random.Dice(6)
                 from d2 in Random.Dice(6)
                 from d3 in Random.Dice(6)
                 select new[] { d1, d2, d3 }.Count(d => d >= 3);
            Show("three dice 3+", threeDiceThreePlus);

            var threeDiceThreePlusTwoDamage =
              from ds in Random.Dice(6).Repeat(3)
              select ds.Where(d => d >= 4).Select(d => 2).Sum();
            Show("four dice 4+, 2 damage", threeDiceThreePlusTwoDamage);

            var fourDiceFourPlus =
             from ds in Random.Dice(6).Repeat(4)
             select ds.Count(d => d >= 4);
            Show("four dice 4+", fourDiceFourPlus);

            var threeDiceThreePlusWithOnes =
                from ds in Random.Dice(6).Repeat(3)
                let hits = ds.Count(d => d >= 3)
                select new { Hits = hits, Ones = Math.Min(hits, ds.Count(d => d == 1)) };
            Show("three dice 3+ show 1's count", threeDiceThreePlusWithOnes, p => string.Format("{0}/{1}", p.Hits, p.Ones));
        }

        private static void Show<T>(string title, IRandom<T> random, Func<T, string> fmt = null)
        {
            fmt = fmt ?? (t => t.ToString());

            Console.WriteLine("# ############################################################");
            Console.WriteLine("# {0}", title);
            foreach (var outcome in random)
            {
                Console.WriteLine("{0,2} {1,7:P2}", fmt(outcome.Key), outcome.Value);
            }
            Console.WriteLine();

            var rand = new System.Random();
            Console.WriteLine(random.Sample(rand).Take(10).Format(" ", "{0}", p => fmt(p)));
            Console.WriteLine();

            var n = 1000000;
            var sample = random.Sample(rand).Take(n);
            var pairs = sample.GroupBy(p => p).Select(p => new { p.Key, P = p.Count() / (double)n }).OrderBy(p => p.P);
            Console.WriteLine(pairs.Format(Environment.NewLine, "{0,2} {1,7:P2}", p => fmt(p.Key), p => p.P));
            Console.WriteLine();
        }
    }
}
