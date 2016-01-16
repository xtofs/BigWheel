using System.Collections.Generic;

namespace Xof.RandomVariables
{
    /// <summary>
    /// discrete random variable
    /// </summary>    
    interface IRandom<T> : IDictionary<T, decimal>
    {
        IEnumerable<T> Sample(System.Random rand);
    }
}