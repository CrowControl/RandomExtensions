namespace Chinchillada.RandomExtensions.FilteredRandom
{
    /// <summary>
    /// Generates random sequences of boolean values with certain patterns filtered out.
    /// </summary>
    public partial class FilteredRandomBool : FilteredRandom<bool>
    {
        /// <inheritdoc />
        protected override IGenerator<bool> Generator { get; }

        /// <summary>
        /// Construct a new <see cref="FilteredRandomBool"/>.
        /// </summary>
        /// <param name="generator">The generator used to generate random values. If left null, <see cref="RandomGenerator.Instance"/> will be used.</param>
        /// <param name="filter">The filter that is applied on generated values. If left null, <see cref="DefaultPatterns.All"/> will be used.</param>
        public FilteredRandomBool(IRandomGenerator generator = null, RandomFilter<bool> filter = null)
            :base(generator, filter)
        {
            Generator = new BoolGenerator(this.RNG);
        }

        /// <summary>
        /// Boolean generator.
        /// </summary>
        private class BoolGenerator : IGenerator<bool>
        {
            private readonly IRandomGenerator _rng;

            public BoolGenerator(IRandomGenerator rng)
            {
                _rng = rng;
            }

            public bool Generate()
            {
                return _rng.RandomBool();
            }
        }
    }
}
