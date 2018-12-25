using Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers;

namespace Chinchillada.RandomExtensions.FilteredRandom
{
    /// <summary>
    /// Interface for filtered random generators.
    /// </summary>
    /// <typeparam name="T">The type of values to generate.</typeparam>
    public interface IFilteredRandom<T> : IGenerator<T>
    {
        /// <summary>
        /// The filter applied to the randomly generated values.
        /// </summary>
        RandomFilter<T> Filter { get; }
    }
}
