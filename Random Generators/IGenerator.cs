namespace Chinchillada.RandomExtensions.FilteredRandom
{
    /// <summary>
    /// Simple interface for generators.
    /// </summary>
    /// <typeparam name="T">The type of values generated.</typeparam>
    public interface IGenerator<out T>
    {
        /// <summary>
        /// Generates a value.
        /// </summary>
        /// <returns>The generated value.</returns>
        T Generate();
    }
}
