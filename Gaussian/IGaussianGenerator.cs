using Chinchillada.RandomExtensions.FilteredRandom;

namespace Chinchillada.RandomExtensions.Guassian
{
    public interface IGaussianGenerator : IGenerator<float>
    {
        /// <summary>
        /// The mean of the gaussian distribution to generate values in.
        /// </summary>
        float Mean { get; }

        /// <summary>
        /// The standard deviation of the gaussian distribution to generate values in.
        /// </summary>
        float StandardDeviation { get; }
    }
}
