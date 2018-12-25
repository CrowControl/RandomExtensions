using System;
using System.Collections.Generic;
using System.Linq;

namespace Chinchillada.RandomExtensions.PerlinNoise
{
    /// <summary>
    /// Perlin noise generator.
    /// </summary>
    public class PerlinNoise : IPerlinNoise
    {
        /// <summary>
        /// The octaves of this perlin noise generator.
        /// </summary>
        private readonly Octave[] _octaves;

        /// <summary>
        /// The amount of octaves used.
        /// </summary>
        public int OctaveCount => _octaves.Length;

        /// <summary>
        /// The persistence.
        /// </summary>
        public float Persistence { get; }

        /// <summary>
        /// Constructs a new perlin noise generator.
        /// </summary>
        /// <param name="octaveCount">The amount of octaves.</param>
        /// <param name="persistence">The persistence constant.</param>
        /// <param name="splineDefinition">The function that defines the shape of the splines that define the ovtave. If left null <see cref="Spline.Predefined.SCurve"/> is used.</param>
        /// <param name="rng">Rng used to choose random values. If left null, <see cref="RandomGenerator.Instance"/> is used.</param>
        public PerlinNoise(int octaveCount, float persistence, IRandomGenerator rng = null, Func<float, float> splineDefinition = null)
        {
            Persistence = persistence;

            if (splineDefinition == null)
                splineDefinition = Spline.Predefined.SCurve;

            if (rng == null)
                rng = RandomGenerator.Instance;

            _octaves = InitializeOctaves(octaveCount, persistence, rng, splineDefinition);
        }

        /// <summary>
        /// Initializes the octaves.
        /// </summary>
        private static Octave[] InitializeOctaves(int octaveCount, float persistence, IRandomGenerator rng, Func<float, float> splineDefinition)
        {
            Octave[] octaves = new Octave[octaveCount];
            for (int index = 1; index <= octaveCount; index++)
            {
                //Calculate values.
                int pointCount = index + 1;
                double amplitude = Math.Pow(persistence, index);

                //Initialize Octave.
                Octave octave = new Octave(pointCount, amplitude, rng, splineDefinition);
                octaves[index - 1] = octave;
            }

            return octaves;
        }

        /// <summary>
        /// Samples the value at the given point.
        /// </summary>
        public float Sample(float point)
        {
            return _octaves.Sum(octave => octave.SampleWeighted(point));
        }

        /// <summary>
        /// Replaces each octave by a random successor.
        /// </summary>
        public void EvolveOctaves()
        {
            for (int index = 0; index < _octaves.Length; index++)
            {
                Octave currentOctave = _octaves[index];
                Octave nextOctave = currentOctave.GenerateSuccesor();

                _octaves[index] = nextOctave;
            }
        }

        /// <inheritdoc />
        public IEnumerator<float> PerlinNoiseEnumerator(int pointsPerInterval = 1000)
        {
            //Initialize and return starting value.
            float stepSize = 1f / pointsPerInterval;
            float point = 0;

            yield return Sample(point);

            while (true)
            {
                //Increase to next step.
                point += stepSize;

                //If we exceed the range, evolve the octaves.
                while (point > 1)
                {
                    point -= 1;
                    EvolveOctaves();
                }

                //Return the sample.
                yield return Sample(point);
            }
        }
    }
}
