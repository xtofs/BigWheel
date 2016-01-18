using System;
using System.Linq;

namespace Xtof.RandomVariables
{
    class Program
    {
        static void Main(string[] args)
        {


            var twoDice0 = from a in Random.Dice(6) from b in Random.Dice(6) select a + b;
            var twoDice1 = Random.Dice(6).Repeat(2).Select(lst => lst.Sum());
            var twoDice2 = Random.Tuple(Random.Dice(6), Random.Dice(6)).Select(t => t.Item1 + t.Item2);
            Demo("two dice", twoDice0);

            // dice roles from the rules of Zombicide
            Zomicide();

            //DnD();
        }

        private static void DnD()
        {
            var plus10Vsplus10 =
              from a in Random.Dice(20)
              from d in Random.Dice(20)              
              select a + 10 >= d + 10;
            Demo("+10 attacks +10", plus10Vsplus10);
        }


        // dice roles from the rules of Zombicide
        private static void Zomicide()
        {
        
            var threeDiceThreePlus =
                 from d1 in Random.Dice(6)
                 from d2 in Random.Dice(6)
                 from d3 in Random.Dice(6)
                 select new[] { d1, d2, d3 }.Count(d => d >= 3);
            Demo("number of dice out of 3, that have value >= 3 (three dice 3+)", threeDiceThreePlus);

            var threeDiceFourPlus =
                from ds in Random.Dice(6).Repeat(3)
                select ds.Count(d => d >= 4);
            Demo("three dice 4+", threeDiceFourPlus);

            var fourDiceFourPlus =
                from ds in Random.Dice(6).Repeat(4)
                select ds.Count(d => d >= 4);
            Demo("four dice 4+", fourDiceFourPlus);

            var threeDiceThreePlusTwoDamage =
                from ds in Random.Dice(6).Repeat(3)
                select ds.Where(d => d >= 4).Select(d => 2).Sum();
            Demo("four dice 4+, 2 damage", threeDiceThreePlusTwoDamage);


            var threeDiceThreePlusWithOnes =
                from ds in Random.Dice(6).Repeat(3)
                let hits = ds.Count(d => d >= 3)
                select new { Hits = hits, Ones = Math.Min(hits, ds.Count(d => d == 1)) };
            Demo("three dice 3+ with count of 1's", threeDiceThreePlusWithOnes, p => string.Format("{0} - {1}", p.Hits, p.Ones));
        }

        private static System.Random rand = new System.Random();

        private static void Demo<T>(string title, IRandom<T> random, Func<T, string> fmt = null, int m = 10, int n = 1000000)
        {
            fmt = fmt ?? (t => t.ToString());

            Console.WriteLine("# ############################################################");
            Console.WriteLine("# {0}", title);
            foreach (var outcome in random)
            {
                Console.WriteLine("{0,4} {1,8:P3}", fmt(outcome.Key), outcome.Value);
            }
            Console.WriteLine();

            Console.WriteLine("# {0} samples", m);
            var result = random.Sample(rand).Take(m);
            Console.WriteLine(result.Format(fmt));
            Console.WriteLine();

            Console.WriteLine("# frequency of {0} samples", n);
            var sample = random.Sample(rand).Take(n);
            var pairs = sample
                .GroupBy(p => p)
                .Select(p => new { p.Key, Frequency = p.Count() / (decimal)n })
                .OrderBy(p => p.Frequency);
            foreach(var pair in pairs)
            {
                Console.WriteLine("{0,4} {1,8:P3}", fmt(pair.Key), pair.Frequency);
            }
            Console.WriteLine();
        }
    }
}
