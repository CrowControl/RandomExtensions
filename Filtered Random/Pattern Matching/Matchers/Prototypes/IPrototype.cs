namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    /// <summary>
    /// Interface for pattern matcher prototypes.
    /// </summary>
    /// <remarks>With prototype we mean a blueprint to instantiate an instance of a class from.</remarks>
    /// <typeparam name="T">The type of the pattern matcher.</typeparam>
    public interface IPrototype<T>
    {
        /// <summary>
        /// The name of the pattern that pattern matchers instantiated from this prototype will match for.
        /// </summary>
        string PatternName { get; }

        /// <summary>
        /// Instantiates a new pattern matcher from this prototype.
        /// </summary>
        /// <returns>The instantiated pattern matcher.</returns>
        IPatternMatcher<T> Instantiate();
    }
}
