namespace Chinchillada.RandomExtensions
{
    /// <summary>
    /// Interface for objects which handle the generation of random values.
    /// </summary>
    public interface IRandomGenerator
    {
        /// <summary>
        /// The seed used for the generation.
        /// </summary>
        int Seed { set; }

        /// <summary>
        /// Returns a random value between <paramref name="min"/>[Inclusive] and <paramref name="max"/>[exclusive].
        /// </summary>
        /// <param name="min">The inclusive minimum value.</param>
        /// <param name="max">The exclusive maximum value.</param>
        /// <returns>A randomly generated value.</returns>
        int RandomRange(int min, int max);

        /// <summary>
        /// Returns a random value between zero and <paramref name="max"/>[exclusive].
        /// </summary>
        /// <param name="max">The exclusive maximum value.</param>
        /// <returns>A randomly generated value.</returns>
        int RandomRange(int max);

        /// <summary>
        /// Returns a random value between <paramref name="min"/>[Inclusive] and <paramref name="max"/>[exclusive].
        /// </summary>
        /// <param name="min">The inclusive minimum value.</param>
        /// <param name="max">The exclusive maximum value.</param>
        /// <returns>A randomly generated value.</returns>
        float RandomRange(float min = 0, float max = 1);

        /// <summary>
        /// Returns either true or false, randomly chosen based on <paramref name="chance"/>.
        /// </summary>
        /// <param name="chance">The chance the returned value is true.</param>
        /// <returns>A Randomly chosen bool.</returns>
        bool RandomBool(float chance = 0.5f);
    }
}
