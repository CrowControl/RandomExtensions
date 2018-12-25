namespace Chinchillada.RandomExtensions.PerlinNoise
{
    /// <summary>
    /// Interface for intervals.
    /// </summary>
    internal interface IInterval
    {
        /// <summary>
        /// The starting point of the interval.
        /// </summary>
        float StartPoint { get; }

        /// <summary>
        /// The ending point of the interval.
        /// </summary>
        float EndPoint { get; }

        /// <summary>
        /// Samples the interval.
        /// </summary>
        /// <param name="point">The point between 0 and 1 that want to sample.</param>
        /// <returns>The sample.</returns>
        float Sample(float point);
    }
}
