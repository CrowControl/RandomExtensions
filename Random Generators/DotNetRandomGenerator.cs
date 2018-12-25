using System;

namespace Chinchillada.RandomExtensions
{
    /// <inheritdoc />
    /// <summary>
    /// Generates random values using the .Net System.Random.
    /// </summary>
    public class DotNetRandomGenerator : IRandomGenerator
    {
        private static Random _random;

        /// <inheritdoc />
        public int Seed
        {
            set { _random = new Random(value); }
        }

        /// <summary>
        /// Constructs a new DotnetRandomGenerator.
        /// </summary>
        /// <param name="seed">The seed for the rng.</param>
        public DotNetRandomGenerator(int? seed = null)
        {
            _random = seed != null 
                ? new Random((int) seed) 
                : new Random();
        }

        /// <inheritdoc />
        public int RandomRange(int min, int max)
        {
            return _random.Next(min, max);
        }

        /// <inheritdoc />
        public int RandomRange(int max)
        {
            return _random.Next(max);
        }

        /// <inheritdoc />
        public float RandomRange(float min, float max)
        {
            return (float)_random.NextDouble() * (max - min) + min;
        }

        /// <inheritdoc />
        public bool RandomBool(float chance = 0.5f)
        {
            return _random.NextDouble() < chance;
        }
    }
}
