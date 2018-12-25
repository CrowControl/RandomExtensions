using UnityEngine;

namespace Chinchillada.RandomExtensions.Guassian
{
    public class GaussianGenerator : IGaussianGenerator
    {
        private readonly IRandomGenerator _generator;

        /// <summary>
        /// The mean of the distribution of the generated values.
        /// </summary>
        public float Mean { get; }

        /// <summary>
        /// The standard deviation of the distribution of the generated values.
        /// </summary>
        public float StandardDeviation { get; }

        /// <summary>
        /// Construct a Gaussian generator that generates values in a standard normal distribution.
        /// </summary>
        /// <remarks>A standard normal distribution is a gaussian distribution witha mean of 0 and a standard deviation of 1.</remarks>
        public GaussianGenerator() : this(null) { }

        /// <summary>
        /// Construct a Gaussian generator that generates values in a standard normal distribution.
        /// </summary>
        /// <remarks>A standard normal distribution is a gaussian distribution witha mean of 0 and a standard deviation of 1.</remarks>
        public GaussianGenerator(IRandomGenerator generator) : this(0, 1, generator) { }

        /// <summary>
        /// Constructs a new random number generator that generates values in a normal distribution.
        /// </summary>
        /// <param name="mean">The mean of the gaussian distribution.</param>
        /// <param name="standardDeviation">The standard deviation of the distribution.</param>
        /// <param name="generator">The generator used to generate the values.</param>
        public GaussianGenerator(float mean, float standardDeviation, IRandomGenerator generator = null)
        {
            Mean = mean;
            StandardDeviation = standardDeviation;
            _generator = generator ?? RandomGenerator.Instance;
        }

        /// <summary>
        /// Generates a random Gaussian distributed float.
        /// </summary>
        /// <remarks>Implemented using yoyoyosef's answer on https://stackoverflow.com/questions/218060/random-gaussian-variables </remarks>
        /// <returns>A normally distributed float.</returns>
        public float Generate()
        {
            //We need 2 uniformly distributed values to generate a normally distributed value.
            float uniform1 =  1.0f - _generator.RandomRange();
            float uniform2 =  1.0f - _generator.RandomRange();

            //This generates a guassian value in the range of [0-1].
            float standardNormal = Mathf.Sqrt(-2.0f * Mathf.Log(uniform1)) 
                                 * Mathf.Sin(2.0f * Mathf.PI * uniform2);

            //Cast the value to our parameters.
            return Mean + StandardDeviation * standardNormal;
        }
    }
}