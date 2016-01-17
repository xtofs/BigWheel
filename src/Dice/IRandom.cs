using System.Collections.Generic;

namespace Xtof.RandomVariables
{
    /// <summary>
    /// Discrete random variable with a immutable distribution for domain T
    /// represented as a dictionary of Value -> Probability
    /// Common instances created through factory methods of class Random.
    /// </summary>    
    interface IRandom<T,TR> : IReadOnlyDictionary<T, TR>, IReadOnlyCollection<KeyValuePair<T, TR>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rand"></param>
        /// <returns>infinet sequence of random picks</returns>
        IEnumerable<T> Sample(System.Random rand);
    }

    /// <summary>
    /// discrete random variable with Rational propabilities
    /// </summary>    
    interface IRandom<T> : IRandom<T, Rational> 
    {
    }
}