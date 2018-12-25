using System.Collections.Generic;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    public interface IPatternMatcher<T>
    {
        /// <summary>
        /// The minimum amount of previous values the matcher looks back at.
        /// </summary>
        int MinLookBackCount { get; }
        
        /// <summary>
        /// The maximum amount of previous values the matcher looks back at.
        /// </summary>
        int MaxLookBackCount { get; }

        /// <summary>
        /// The name of the pattern we're matching.
        /// </summary>
        string PatternName { get; }
        
        /// <summary>
        /// Sets the range of possible values.
        /// </summary>
        /// <param name="min">The lower bound of the range.</param>
        /// <param name="max">The upper bound of the range.</param>
        void SetRange(T min, T max);

        /// <summary>
        /// Registers a value to the history.
        /// </summary>
        /// <param name="value">The value.</param>
        void RegisterValue(T value);

        /// <summary>
        /// Matches a new value, taking the history into account.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <returns>True if the new value triggers a pattern match.</returns>
        bool MatchValue(T value);

        /// <summary>
        /// Matches a complete sequence.
        /// </summary>
        /// <param name="values">the sequence.</param>
        /// <returns>True if a pattern is matches in the <paramref name="values"/>.</returns>
        bool MatchSequence(List<T> values);
    }
}
