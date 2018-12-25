using System;
using System.Collections.Generic;
using Chinchillada.RandomExtensions.Utilities;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    internal partial class NeighbourPatternMatcher<T> : IPatternMatcher<T> where T : IComparable
    {
        /// <summary>
        /// The amount of pairs that this matcher will check.
        /// </summary>
        private readonly int _matchAmount;

        /// <summary>
        /// The distance between the values that constitute a pair.
        /// </summary>
        private readonly int _neighbourOffset;

        private readonly PatternMatcherHistory<T> _history;

        private readonly IPredicateBuilder<T, Pair<T>> _predicateBuilder;
        private readonly LazyContainer<Func<Pair<T>, bool>> _predicateContainer;

        /// <inheritdoc />
        public int MinLookBackCount { get; }

        /// <inheritdoc />
        public int MaxLookBackCount { get; }

        /// <inheritdoc />
        public string PatternName { get; }
        
        /// <summary>
        /// Creates a new Pattern filter.
        /// </summary>
        /// <param name="patternName">Name of the pattern this filter matches for.</param>
        /// <param name="checkAmount">The amount of concurrent previous values to check.</param>
        /// <param name="matchAmount">The amount of instances of the <paramref name="neighborComparison"/> at which point the pattern is matched.</param>
        /// <param name="neighbourOffset">The size of the step between the value to check and it's neighbour.</param>
        /// <param name="neighborComparison">The predicate to compare the value and it's neighbour <paramref name="neighbourOffset"/> before it.</param>
        public NeighbourPatternMatcher(string patternName, int checkAmount, int matchAmount, int neighbourOffset,
                                       Func<Pair<T>, bool> neighborComparison)
        {
            PatternName = patternName;

            //Cache values.
            _matchAmount = matchAmount;
            _neighbourOffset = neighbourOffset;

            //Calculate the look back range.
            MinLookBackCount = matchAmount + neighbourOffset;
            MaxLookBackCount = checkAmount + neighbourOffset;

            _history = new NeighbourHistory<T>(MaxLookBackCount - 1, neighbourOffset);

            _predicateBuilder = new DefaultPredicateBuilder<T, Pair<T>>(neighborComparison);
            _predicateContainer = new LazyContainer<Func<Pair<T>, bool>>(_predicateBuilder.BuildPredicate);
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

        #region Matching

        /// <inheritdoc />
        public bool MatchValue(T value)
        {
            //Register if this value triggers a match.
            bool isMatch = MatchWithNewValue(value);
            _history.CacheValue(value, isMatch);
            
            //Return true if the new match triggers the max.
            return isMatch && _history.Matches + 1 >= _matchAmount;
        }

        /// <summary>
        /// Matches a new value with the current history.
        /// </summary>
        /// <param name="value">The value to match.</param>
        /// <returns>True if the pattern is matched.</returns>
        private bool MatchWithNewValue(T value)
        {
            //Need at least 1 lookback step to match.
            if (_history.Size < _neighbourOffset)
                return false;

            //Get the comparison neighbour for this value.
            T neighbour = _history.AtIndex(_history.Size - _neighbourOffset);
            Pair<T> pair = new Pair<T>(value, neighbour);

            //Check if it matches the predicate.
            return ApplyPredicate(pair);
        }

        /// <inheritdoc />
        public bool MatchSequence(List<T> values)
        {
            //Check for mininum expected length.
            if (values.Count < MinLookBackCount)
                return false;

            NeighbourHistory<T> history = new NeighbourHistory<T>(MaxLookBackCount, _neighbourOffset);
            for (int i = 0; i < _neighbourOffset; i++)
            {
                T value = values[i];

                history.CacheValue(value, false);
                history.SaveValue(value);
            }

            for (int i = _neighbourOffset; i < values.Count; i++)
            {
                //Check if we still have a neighbour to compare to.
                int neighbourIndex = i - _neighbourOffset;
                if (neighbourIndex >= values.Count)
                    return false;

                //Get values.
                T value = values[i];
                T neighbour = values[neighbourIndex];

                //Compare.
                Pair<T> pair = new Pair<T>(value, neighbour);
                bool match = ApplyPredicate(pair);
                
                history.CacheValue(value, match);
                history.SaveValue(value);

                if (history.Matches >= _matchAmount)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Applies the pattern predicate to the <paramref name="pair"/>.
        /// </summary>
        /// <param name="pair">The pair.</param>
        /// <returns>The result of the predicate.</returns>
        public bool ApplyPredicate(Pair<T> pair)
        {
            Func<Pair<T>, bool> predicate = _predicateContainer.Get();
            return predicate(pair);
        }

        #endregion Matching
    }
}
