using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada.RandomExtensions.Utilities;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    internal partial class OccurencePatternMatcher<T> : IPatternMatcher<T> where T : IComparable
    {
        /// <summary>
        /// The amount of matches necessary to constitute a pattern-match.
        /// </summary>
        private readonly int _matchCount;

        /// <summary>
        /// The previously accepted values.
        /// </summary>
        private readonly PatternMatcherHistory<T> _history;

        /// <summary>
        /// The function that builds the predicate.
        /// </summary>
        private readonly IPredicateBuilder<T, T> _predicateBuilder;

        /// <summary>
        /// A lazy property that encapsulates the predicate.
        /// </summary>
        private readonly LazyContainer<Func<T, bool>> _predicateContainer;

        /// <inheritdoc />
        public int MinLookBackCount { get; }
        /// <inheritdoc />
        public int MaxLookBackCount { get; }
        /// <inheritdoc />
        public string PatternName { get; }

        /// <summary>
        /// Constructs a new occurence pattern-mather
        /// </summary>
        /// <param name="patternName"><see cref="PatternName"/>.</param>
        /// <param name="matchCount">The amount of matches necessary to constitute a pattern-match.</param>
        /// <param name="minLookBackCount"><see cref="MinLookBackCount"/>.</param>
        /// <param name="maxLookBackCount"><see cref="MaxLookBackCount"/>.</param>
        /// <param name="predicateBuilderFunction">The function that builds the predicate.</param>
        public OccurencePatternMatcher(string patternName, int matchCount, int minLookBackCount, int maxLookBackCount,
            Func<T, T, Func<T, bool>> predicateBuilderFunction)
        {
            //Cache the parameters.
            _matchCount = matchCount;
            PatternName = patternName;
            MinLookBackCount = minLookBackCount;
            MaxLookBackCount = maxLookBackCount;

            //Initialize the history.
            _history = new OccurenceHistory<T>(MaxLookBackCount - 1);

            //Initialize the predicate builder and property.
            _predicateBuilder = new RangePredicateBuilder<T, T>(predicateBuilderFunction);
            _predicateContainer = new LazyContainer<Func<T, bool>>(_predicateBuilder.BuildPredicate);
        }

        /// <inheritdoc />
        public void SetRange(T min, T max)
        {
            _predicateBuilder.SetRange(min, max);
        }

        /// <inheritdoc />
        public void RegisterValue(T value)
        {
            _history.SaveValue(value);
        }

        /// <inheritdoc />
        public bool MatchValue(T value)
        {
            bool match = ApplyPredicate(value);
            _history.CacheValue(value, match);

            return match &&                             //This value matches.
                _history.Matches + 1 >= _matchCount;    //And with it we've reached the match count.
        }
        /// <inheritdoc />
        public bool MatchSequence(List<T> values)
        {
            return values.Count(ApplyPredicate) >= _matchCount;
        }

        /// <summary>
        /// Applies the predicate to the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the applied predicate.</returns>
        private bool ApplyPredicate(T value)
        {
            Func<T, bool> predicate = _predicateContainer.Get();
            return predicate(value);
        }

    }
}
