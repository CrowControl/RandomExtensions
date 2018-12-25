using UnityEngine;

namespace Chinchillada.RandomExtensions
{
    /// <summary>
    /// Generates random values using the UnityEngine.Random.
    /// </summary>
    internal class UnityRandomGenerator : IRandomGenerator
    {
        /// <inheritdoc />
        public int Seed
        {
            set
            {
                Random.InitState(value);
            }
        }

        public UnityRandomGenerator(int? seed = null)
        {
            if (seed != null)
                Seed = (int) seed;
        }

        /// <inheritdoc />
        public int RandomRange(int min, int max)
        {
            return Random.Range(min, max);
        }

        /// <inheritdoc />
        public int RandomRange(int max)
        {
            return Random.Range(0, max);
        }

        /// <inheritdoc />
        public float RandomRange(float min, float max)
        {
            return Random.Range(min, max);
        }

        /// <inheritdoc />
        public bool RandomBool(float chance = 0.5f)
        {
            return Random.value < chance;
        }
    }
}
