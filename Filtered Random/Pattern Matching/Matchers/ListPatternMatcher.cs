using System;
using System.Collections.Generic;
using Chinchillada.RandomExtensions.Utilities;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    internal partial class ListPatternMatcher<T> : IPatternMatcher<T> where T : IComparable
    {
        /// <summary>
        /// The predicate that is applied to the list to match it for the pattern.
        /// </summary>
        private readonly Func<List<T>, bool> _listPredicate;

        /// <summary>
        /// The history of previously accepted values.
        /// </summary>
        private readonly FixedQueue<T> _history;

        /// <summary>
        /// The minimum amount of values required for this pattern matcher to activate.
        /// </summary>
        public int MinLookBackCount { get; }
        
        /// <summary>
        /// The maximum amount of values that this pattern matcher looks at.
        /// </summary>
        public int MaxLookBackCount { get; }

        /// <inheritdoc />
        public string PatternName { get; }

        /// <summary>
        /// Constructs a new list pattern-matcher.
        /// </summary>
        /// <param name="patternName"><see cref="PatternName"/>.</param>
        /// <param name="minLookBackCount"><see cref="MinLookBackCount"/>.</param>
        /// <param name="maxLookBackCount"><see cref="MaxLookBackCount"/>.</param>
        /// <param name="listPredicate">The predicate that's applied to the list.</param>
        public ListPatternMatcher(string patternName, int minLookBackCount, int maxLookBackCount,
                                  Func<List<T>, bool> listPredicate)
        {
            PatternName = patternName;

            MinLookBackCount = minLookBackCount;
            MaxLookBackCount = maxLookBackCount;

            _listPredicate = listPredicate;

            _history = new FixedQueue<T>(MaxLookBackCount);
        }
        
        /// <inheritdoc />
        public void SetRange(T min, T max) { }

        /// <inheritdoc />
        public void RegisterValue(T value)
        {
            _history.Enqueue(value);
        }

        /// <inheritdoc />
        public bool MatchValue(T value)
        {
            List<T> values = new List<T>(_history.ToList()) { value };
            return _listPredicate(values);
        }

        /// <inheritdoc />
        public bool MatchSequence(List<T> values)
        {
            return _listPredicate(values);
        }
    }
}
