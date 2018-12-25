using System.Collections.Generic;

namespace Chinchillada.RandomExtensions.PerlinNoise
{
    /// <summary>
    /// Interface for perlin noise generators.
    /// </summary>
    public interface IPerlinNoise
    {
        /// <summary>
        /// The amount of octaves used.
        /// </summary>
        int OctaveCount { get; }

        /// <summary>
        /// The persistence.
        /// </summary>
        float Persistence { get; }
        
        /// <summary>
        /// Samples the value at the given point.
        /// </summary>
        float Sample(float point);

        /// <summary>
        /// Replaces each octave by a random successor.
        /// </summary>
        void EvolveOctaves();

        /// <summary>
        /// Returns an enumerator that infinitely generates new perlin noise values.
        /// </summary>
        /// <param name="pointsPerInterval">Amount of points per interval.</param>
        /// <returns>A perlin noise value.</returns>
        IEnumerator<float> PerlinNoiseEnumerator(int pointsPerInterval = 1000);
    }
}
