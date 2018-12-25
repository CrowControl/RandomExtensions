using System.Collections.Generic;
using System.Linq;
using Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers;

namespace Chinchillada.RandomExtensions.FilteredRandom
{
    /// <summary>
    /// A filter for randomly generated values.
    /// </summary>
    /// <typeparam name="T">The type of values to filter.</typeparam>
    public class RandomFilter<T>
    {
        /// <summary>
        /// The pattern matchers that make up the filter.
        /// </summary>
        private readonly List<IPatternMatcher<T>> _patternMatchers;

        /// <summary>
        /// The minimum lookback count of the pattern matchers.
        /// </summary>
        public int MinLookBackCount
        {
            get { return _patternMatchers.Min(m => m.MinLookBackCount); }
        }

        /// <summary>
        /// The maximum lookback count of the pattern matchers.
        /// </summary>
        public int MaxLookBackCount
        {
            get { return _patternMatchers.Max(m => m.MaxLookBackCount); }
        }
        
        /// <summary>
        /// Constructs a new random-filter.
        /// </summary>
        /// <param name="patternMatchers"></param>
        public RandomFilter(List<IPatternMatcher<T>> patternMatchers)
        {
            _patternMatchers = patternMatchers;
        }

        /// <summary>
        /// Sets the range of possible values.
        /// </summary>
        /// <param name="min">The underbound.</param>
        /// <param name="max">The upperbound.</param>
        public void SetRange(T min, T max)
        {
            //Pass the range to the pattern matchers.
            _patternMatchers.ForEach(p => p.SetRange(min, max));
        }

        /// <summary>
        /// Registers a value as accepted.
        /// </summary>
        /// <param name="value">The value.</param>
        public void RegisterValue(T value)
        {
            _patternMatchers.ForEach(p => p.RegisterValue(value));
        }

        /// <summary>
        /// Checks if the new value causes any of the pattern matchers in this set to trigger.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>True if any pattern is matched.</returns>
        public bool MatchNewValue(T value)
        {
            bool match = false;

            foreach (IPatternMatcher<T> patternMatcher in _patternMatchers)
                if (patternMatcher.MatchValue(value))
                    match = true;

            return match;
        }

        /// <summary>
        /// Matches an entire sequence.
        /// </summary>
        /// <param name="sequence">The sequence to match.</param>
        /// <returns></returns>
        public bool ValidateSequence(IEnumerable<T> sequence)
        {
            return !_patternMatchers.Any(matcher => matcher.MatchSequence(sequence.ToList()));
        }
    }
}
