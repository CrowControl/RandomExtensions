namespace Chinchillada.RandomExtensions.FilteredRandom
{
    public partial class FilteredRandomFloat : FilteredRandom<float>
    {
        /// <summary>
        /// The underbound of the possible range for generated values.
        /// </summary>
        public float RangeMin { get; }

        /// <summary>
        /// The upperbound of the possible range for generated values.
        /// </summary>
        public float RangeMax { get; }

        ///<inheritdoc />
        protected override IGenerator<float> Generator { get; }

        /// <summary>
        /// Constructs a new Filtered random float generator.
        /// </summary>
        /// <param name="rangeMin">The underbound of the range of possible values.</param>
        /// <param name="rangeMax">The upper bound of the range of possible values.</param>
        /// <param name="generator">The generator used to generate random values. If left null, <see cref="RandomGenerator.Instance"/> will be used.</param>
        /// <param name="filter">The filter that is applied on generated values. If left null, <see cref="DefaultPatterns.All"/> will be used.</param>
        public FilteredRandomFloat(float rangeMin, float rangeMax, IRandomGenerator generator = null, RandomFilter<float> filter = null)
            : base(generator, filter)
        {
            //Set the range.
            RangeMin = rangeMin;
            RangeMax = rangeMax;

            //Initialize the generator.
            Generator = new FloatGenerator(this.RNG, rangeMin, rangeMax);
            SetRange(rangeMin, rangeMax);
        }

        /// <summary>
        /// Float generator.
        /// </summary>
        private class FloatGenerator : IGenerator<float>
        {
            /// <summary>
            /// The random number generator used to generate floating point values.
            /// </summary>
            private readonly IRandomGenerator _rng;

            /// <summary>
            /// The possible range for generated values.
            /// </summary>
            private readonly float _min;
            private readonly float _max;

            /// <summary>
            /// Construct a new float generator.
            /// </summary>
            /// <param name="rng">The random number generator used to generate floating point values.</param>
            /// <param name="min">The underbound of the possible range for generated values.</param>
            /// <param name="max">The upperbound of the possible range for generated values.</param>
            public FloatGenerator(IRandomGenerator rng, float min, float max)
            {
                _rng = rng;

                _min = min;
                _max = max;
            }

            /// <summary>
            /// Generates a random floating point value.
            /// </summary>
            /// <returns>A randomly generated floating point value.</returns>
            public float Generate()
            {
                return _rng.RandomRange(_min, _max);
            }
        }
    }
}
