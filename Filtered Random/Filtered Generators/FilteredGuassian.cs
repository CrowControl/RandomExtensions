using Chinchillada.RandomExtensions.Guassian;

namespace Chinchillada.RandomExtensions.FilteredRandom
{
    /// <summary>
    /// Generates gaussian sequences with given patterns filtered out.
    /// </summary>
    public partial class FilteredGuassian : FilteredRandom<float>
    {
        /// <summary>
        /// The mean of the gaussian distribution to generate values in.
        /// </summary>
        public float Mean { get; }

        /// <summary>
        /// The standard deviation of the distribution to generate values in.
        /// </summary>
        public float StandardDeviation { get; }

        /// <summary>
        /// The floating point generator that is used to generate values.
        /// </summary>
        protected override IGenerator<float> Generator { get; }

        /// <summary>
        /// Constructs a new FilteredGuassian generator.
        /// </summary>
        /// <param name="mean">The mean of of the gaussian distribution we want to generate and filter.</param>
        /// <param name="standardDeviation">The standard deviation of the gaussian distribution we want to generate and filter.</param>
        /// <param name="generator">The basic generator we want to use to generate values. If null, <see cref="RandomGenerator.Instance"/> will be used.</param>
        /// <param name="filter">The filter we want to apply to the generated values. if null, <see cref="DefaultPatterns.All"/> will be used.</param>
        public FilteredGuassian(float mean, float standardDeviation, IRandomGenerator generator = null, RandomFilter<float> filter = null)
            : base(generator, filter)
        {
            //Set the parameters.
            Mean = mean;
            StandardDeviation = standardDeviation;

            //initialize the generator and range.
            Generator = new GaussianGenerator(mean, standardDeviation, RNG);
            InitializeRange(mean, standardDeviation);
        }

        /// <summary>
        /// Constructs a new filtered Gaussian with the given generator and filter.
        /// </summary>
        /// <param name="generator">The guassian generator.</param>
        /// <param name="filter">The filter.</param>
        public FilteredGuassian(IGaussianGenerator generator, RandomFilter<float> filter = null)
            : base(null, filter)
        {
            Mean = generator.Mean;
            StandardDeviation = generator.StandardDeviation;

            Generator = generator;
            InitializeRange(Mean, StandardDeviation);
        }

        /// <summary>
        /// Initializes the range.
        /// </summary>
        /// <remarks>This is quite hacky, but the extra time it would take to pass the gaussian parameters in a better way was too much for me.</remarks>
        /// <param name="mean">The mean of the distribution.</param>
        /// <param name="standardDeviation">The standard deviation of the distribution.</param>
        private void InitializeRange(float mean, float standardDeviation)
        {
            //We set the range to 3 deviations in both directions.
            float thirdDeviation = standardDeviation * 3;
            SetRange(mean - thirdDeviation, mean + thirdDeviation);
        }
    }
}