# Dice

calculate combined dice rolls

    var twoDice = 
               from d1 in Random.Dice(6) 
               from d2 in Random.Dice(6) 
               select d1 + d2; 

returns a data structure that know the probability for all the outcomes (2, 3, 4, ... , 11, 12)
and allows to generate random number for that distribution

the expressions can become more complicated. For example, the game zombicide has a rule where the number of ones are important (Crawlers) but only if there is another die with three or more.

    var threeDiceThreePlusWithOnes = 
                 from ds in Random.Dice(6).Repeat(3) 
                 let hits = ds.Count(d => d >= 3) 
                 select new { Hits = hits, Ones = Math.Min(hits, ds.Count(d => d == 1)) }; 

