using System.Collections.Generic;

namespace Statistics
{
    /// <summary>
    /// discrete random variable
    /// </summary>    
    interface IRandom<T> : IDictionary<T, double>
    {
        IEnumerable<T> Sample(System.Random rand);
    }
}