namespace Chinchillada.RandomExtensions.FilteredRandom
{
    /// <summary>
    /// Interface for component wrappers for <see cref="IFilteredRandom{T}"/> implementations.
    /// </summary>
    /// <typeparam name="T">Type of value to generate.</typeparam>
    public interface IFilteredRandomComponent<T> : IFilteredRandom<T>
    {
        /// <summary>
        /// The filter that filters the values that are generated.
        /// </summary>
        new RandomFilter<T> Filter { get; set; }

        /// <summary>
        /// Initializes the generator.
        /// </summary>
        /// <param name="filter">The filter used to filter the generated values.</param>
        void InitializeGenerator(RandomFilter<T> filter = null);
    }
}
