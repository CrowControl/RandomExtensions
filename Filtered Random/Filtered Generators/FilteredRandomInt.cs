namespace Chinchillada.RandomExtensions.FilteredRandom
{
    public partial class FilteredRandomInt : FilteredRandom<int>
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
        protected override IGenerator<int> Generator { get; }

        /// <summary>
        /// Constructs a new Filtered random int generator.
        /// </summary>
        /// <param name="rangeMin">The underbound of the range of possible values.</param>
        /// <param name="rangeMax">The upper bound of the range of possible values.</param>
        /// <param name="generator">The generator used to generate random values. If left null, <see cref="RandomGenerator.Instance"/> will be used.</param>
        /// <param name="filter">The filter that is applied on generated values. If left null, <see cref="DefaultPatterns.All"/> will be used.</param>
        public FilteredRandomInt(int rangeMin, int rangeMax, IRandomGenerator generator = null, RandomFilter<int> filter = null)
            :base(generator, filter)
        {
            RangeMin = rangeMin;
            RangeMax = rangeMax; 

            Generator = new IntGenerator(this.RNG, rangeMin, rangeMax);
            SetRange(rangeMin, rangeMax);
        }

        /// <summary>
        /// Random integer generator.
        /// </summary>
        private class IntGenerator : IGenerator<int>
        {
            /// <summary>
            /// The random number generator used to generate integer values.
            /// </summary>
            private readonly IRandomGenerator _rng;

            /// <summary>
            /// The possible range for generated values.
            /// </summary>
            private readonly int _min;
            private readonly int _max;
            
            /// <summary>
            /// Construct a new integer generator.
            /// </summary>
            /// <param name="rng">The random number generator used to generate integer values.</param>
            /// <param name="min">The underbound of the possible range for generated values.</param>
            /// <param name="max">The upperbound of the possible range for generated values.</param>
            public IntGenerator(IRandomGenerator rng, int min, int max)
            {
                _rng = rng;

                _min = min;
                _max = max;
            }

            ///<inheritdoc />
            public int Generate()
            {
                return _rng.RandomRange(_min, _max);
            }
        }
    }
}
