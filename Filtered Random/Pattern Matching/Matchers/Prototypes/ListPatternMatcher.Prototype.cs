using System;
using System.Collections.Generic;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    /// <summary>
    /// Pattern matcher applies a pattern predicate to an entire list as a whole.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal partial class ListPatternMatcher<T> where T : IComparable
    {
        /// <summary>
        /// The prototype for list pattern matchers.
        /// </summary>
        public class Prototype : IPrototype<T>
        {
            /// <summary>
            /// <see cref="ListPatternMatcher{T}.MinLookBackCount"/>.
            /// </summary>
            private readonly int _minLookBackCount;

            /// <summary>
            /// <see cref="ListPatternMatcher{T}.MaxLookBackCount"/>.
            /// </summary>
            private readonly int _maxLookBackCount;

            /// <summary>
            /// The predicate that is applied to the list to match for the pattern.
            /// </summary>
            private readonly Func<List<T>, bool> _listPredicate;

            /// <inheritdoc />
            public string PatternName { get; }

            /// <summary>
            /// Constructs a new <see cref="ListPatternMatcher{T}"/> prototype.
            /// </summary>
            /// <param name="patternName"><see cref="PatternName"/>.</param>
            /// <param name="minLookBackCount"><see cref="ListPatternMatcher{T}.MinLookBackCount"/>.</param>
            /// <param name="maxLookBackCount"><see cref="ListPatternMatcher{T}.MaxLookBackCount"/>.</param>
            /// <param name="listPredicate">The predicate that is applied to the list to match for the pattern.</param>
            public Prototype(string patternName, int minLookBackCount, int maxLookBackCount,
                Func<List<T>, bool> listPredicate)
            {
                PatternName = patternName;
                _minLookBackCount = minLookBackCount;
                _maxLookBackCount = maxLookBackCount;
                _listPredicate = listPredicate;
            }

            /// <inheritdoc />
            public IPatternMatcher<T> Instantiate()
            {
                return new ListPatternMatcher<T>(PatternName, _minLookBackCount, _maxLookBackCount, _listPredicate);
            }
        }
    }
}
