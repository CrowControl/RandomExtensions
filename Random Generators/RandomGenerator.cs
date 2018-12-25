namespace Chinchillada.RandomExtensions
{
    /// <inheritdoc />
    /// <summary>
    /// Wrapper for the actual implementations of the RandomGenerator, which can be used as an abstract interface.
    /// </summary>
    internal class RandomGenerator : IRandomGenerator
    {
        private static RandomGenerator _instance;

        /// <summary>
        /// Singleton instance of the random generator.
        /// </summary>
        public static RandomGenerator Instance => _instance ?? (_instance = new RandomGenerator());

        public IRandomGenerator ImplementingGenerator { get; set; }

        /// <inheritdoc />
        public int Seed
        {
            set { ImplementingGenerator.Seed = value; }
        }

        /// <summary>
        /// Constructs 
        /// </summary>
        private RandomGenerator()
        {
            ImplementingGenerator = new DotNetRandomGenerator();
        }
        
        /// <inheritdoc />
        public int RandomRange(int min, int max)
        {
            return ImplementingGenerator.RandomRange(min, max);
        }

        /// <inheritdoc />
        public int RandomRange(int max)
        {
            return ImplementingGenerator.RandomRange(max);
        }

        /// <inheritdoc />
        public float RandomRange(float min, float max)
        {
            return ImplementingGenerator.RandomRange(min, max);
        }

        /// <inheritdoc />
        public bool RandomBool(float chance = 0.5f)
        {
            return ImplementingGenerator.RandomBool(chance);
        }
    }
}
