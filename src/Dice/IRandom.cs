using System.Collections.Generic;

namespace Xtof.RandomVariables
{
    /// <summary>
    /// Discrete random variable with a immutable distribution for domain T.
    /// Represented as a dictionary of Value -> Probability
    /// Instances created through factory methods and combinators of class Random
    /// </summary>    
    interface IRandom<T> : IReadOnlyDictionary<T, double>, IReadOnlyCollection<KeyValuePair<T, double>>
    {
        /// <summary>
        /// produce an infinite sequence of random picks based on this random distribution
        /// by drawing from given random number generator
        /// </summary>
        /// <returns>infinite sequence of random picks</returns>
        IEnumerable<T> Sample(System.Random rand);
    }
}