using System;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    internal partial class OccurencePatternMatcher<T> where T : IComparable
    {
        public class Prototype : IPrototype<T>
        {
            /// <summary>
            /// The amount of occurences befre it counts as a matched pattern.
            /// </summary>
            private readonly int _matchCount;

            /// <summary>
            /// <see cref="OccurencePatternMatcher{T}.MinLookBackCount"/>
            /// </summary>
            private readonly int _minLookBackCount;

            /// <summary>
            /// <see cref="OccurencePatternMatcher{T}.MaxLookBackCount"/>
            /// </summary>
            private readonly int _maxLookBackCount;

            /// <summary>
            /// The function that builds the predicate.
            /// </summary>
            private readonly Func<T, T, Func<T, bool>> _predicateBuilderFunction;

            /// <inheritdoc />
            public string PatternName { get; }

            /// <summary>
            /// Constructs a new <see cref="OccurencePatternMatcher{T}"/> prototype.
            /// </summary>
            /// <param name="patternName"><see cref="PatternName"/>.</param>
            /// <param name="matchCount">The amount of occurences befre it counts as a matched pattern.</param>
            /// <param name="minLookBackCount"><see cref="OccurencePatternMatcher{T}.MinLookBackCount"/></param>
            /// <param name="maxLookBackCount"><see cref="OccurencePatternMatcher{T}.MaxLookBackCount"/></param>
            /// <param name="predicateBuilderFunction">The function that builds the predicate.</param>
            public Prototype(string patternName, int matchCount, int minLookBackCount, int maxLookBackCount,
                Func<T, T, Func<T, bool>> predicateBuilderFunction)
            {
                PatternName = patternName;
                _matchCount = matchCount;
                _minLookBackCount = minLookBackCount;
                _maxLookBackCount = maxLookBackCount;
                _predicateBuilderFunction = predicateBuilderFunction;
            }

            /// <inheritdoc />
            public IPatternMatcher<T> Instantiate()
            {
                return new OccurencePatternMatcher<T>(PatternName, _matchCount, _minLookBackCount, _maxLookBackCount,
                    _predicateBuilderFunction);
            }
        }
    }
}
