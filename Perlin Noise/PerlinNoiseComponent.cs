using System.Collections.Generic;
using UnityEngine;

namespace Chinchillada.RandomExtensions.PerlinNoise
{
    /// <summary>
    /// Component wrapper for <see cref="PerlinNoise"/>.
    /// </summary>
    internal class PerlinNoiseComponent : MonoBehaviour, IPerlinNoise
    {
        /// <summary>
        /// The amount of octaves that the perlin noise should consist of.
        /// </summary>
        [SerializeField] private int _octaveCount = 1;
        
        /// <summary>
        /// The value that decides how persistent each subsequent octave is.
        /// </summary>
        [SerializeField] private float _persistence = 0.5f;

        /// <summary>
        /// The instance of a perlin noise generator that this component wraps.
        /// </summary>
        private PerlinNoise _perlinNoise;

        /// <inheritdoc />
        public int OctaveCount => _perlinNoise.OctaveCount;

        /// <inheritdoc />
        public float Persistence => _perlinNoise.Persistence;

        /// <summary>
        /// Called when the component first awakes.
        /// Resets the generator.
        /// </summary>
        private void Awake()
        {
            ResetGenerator();
        }

        /// <summary>
        /// Called when the values in the editor have changed.
        /// Resets the generator.
        /// </summary>
        private void OnValidate()
        {
            ResetGenerator();
        }

        /// <summary>
        /// Resets the generator. Uses the current values of the editor fields as parameters.
        /// </summary>
        public void ResetGenerator()
        {
            _perlinNoise = new PerlinNoise(_octaveCount, _persistence);
            Debug.Log("Perlin Noise generator has been reset.");
        }

        /// <inheritdoc />
        public IEnumerator<float> PerlinNoiseEnumerator(int pointsPerInterval = 1000)
        {
            return _perlinNoise.PerlinNoiseEnumerator(pointsPerInterval);
        }

        /// <inheritdoc />
        public float Sample(float point)
        {
            return _perlinNoise.Sample(point);
        }

        /// <inheritdoc />
        public void EvolveOctaves()
        {
            _perlinNoise.EvolveOctaves();
        }
    }
}
