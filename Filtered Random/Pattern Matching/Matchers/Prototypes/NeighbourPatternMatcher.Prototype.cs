using System;
using System.Collections.Generic;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    using Chinchillada.RandomExtensions.Utilities;

    internal partial class NeighbourPatternMatcher<T> where T : IComparable
    {
        public class Prototype : IPrototype<T>
        {
            /// <summary>
            /// The amount of pairs that this matcher will check.
            /// </summary>
            private readonly int _checkAmount;

            /// <summary>
            /// The amount of pairs that should match the <see cref="_neighbourOffset"/> to count as a pattern-match.
            /// </summary>
            private readonly int _matchAmount;

            /// <summary>
            /// The distance between the values that constitute a pair.
            /// </summary>
            private readonly int _neighbourOffset;

            /// <summary>
            /// The function used to compare neighbours.
            /// </summary>
            private readonly Func<Pair<T>, bool> _neighborComparison;

            /// <inheritdoc />
            public string PatternName { get; }

            /// <summary>
            /// Instantiates a new <see cref="NeighbourPatternMatcher{T}"/>.
            /// </summary>
            /// <param name="patternName"><see cref="PatternName"/>.</param>
            /// <param name="checkAmount">The amount of pairs that this matcher will check.</param>
            /// <param name="matchAmount">The amount of pairs that should match the <paramref name="neighbourOffset"/> to count as a pattern-match.<s</param>
            /// <param name="neighbourOffset">The distance between the values that constitute a pair.</param>
            /// <param name="neighborComparison">The function used to compare neighbours.</param>
            public Prototype(string patternName, int checkAmount, int matchAmount, int neighbourOffset,
                             Func<Pair<T>, bool> neighborComparison)
            {
                PatternName = patternName;
                _checkAmount = checkAmount;
                _matchAmount = matchAmount;
                _neighbourOffset = neighbourOffset;
                _neighborComparison = neighborComparison;
            }

            /// <inheritdoc />
            public IPatternMatcher<T> Instantiate()
            {
                return new NeighbourPatternMatcher<T>(PatternName, _checkAmount, _matchAmount, 
                                                      _neighbourOffset, _neighborComparison);
            }
        }
    }
}
