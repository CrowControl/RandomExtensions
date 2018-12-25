using System.Collections.Generic;
using System.Runtime.InteropServices;
using Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers;

namespace Chinchillada.RandomExtensions.FilteredRandom
{
    public partial class FilteredRandomFloat
    {
        /// <inheritdoc />
        public override List<IPatternMatcher<float>> GetAllPatternMatchers()
        {
            return DefaultPatterns.All;
        }

        /// <inheritdoc />
        public override List<IPrototype<float>> GetAllPatternMatcherProtoTypes()
        {
            return DefaultPatterns.Prototypes;
        }

        /// <summary>
        /// The collection of predefined patterns to filter random floating point values.
        /// </summary>
        public class DefaultPatterns
        {
            #region Properties

            /// <summary>
            /// All the default patterns.
            /// </summary>
            public static List<IPatternMatcher<float>> All => new List<IPatternMatcher<float>>
            {
                TooCloseToPrevious,
                TooCloseToPrevious2,
                AscendingSequence,
                DescendingSequence,
                BottomHeavy,
                TopHeavy
            };

            /// <summary>
            /// All the default pattern prototypes.
            /// </summary>
            public static List<IPrototype<float>> Prototypes => new List<IPrototype<float>>
            {
                TooCloseToPreviousPrototype,
                TooCloseToPrevious2Prototype,
                AscendingSequencePrototype,
                DescendingSequencePrototype,
                BottomHeavyPrototype,
                TopHeavyPrototype
            };

            /// <summary>
            /// Matches for 2 values being too close to each other.
            /// </summary>
            public static IPatternMatcher<float> TooCloseToPrevious => TooCloseToPreviousPrototype.Instantiate();

            /// <summary>
            /// Natches for 3 values being to close to each other.
            /// </summary>
            public static IPatternMatcher<float> TooCloseToPrevious2 => TooCloseToPrevious2Prototype.Instantiate();

            /// <summary>
            /// Matches for a sequence of 4 ascending values.
            /// </summary>
            public static IPatternMatcher<float> AscendingSequence => AscendingSequencePrototype.Instantiate();

            /// <summary>
            /// Matches for a sequence of 4 descending values.
            /// </summary>
            public static IPatternMatcher<float> DescendingSequence => DescendingSequencePrototype.Instantiate();

            /// <summary>
            /// Checks if too many recent values were in the bottom of the range.
            /// </summary>
            public static IPatternMatcher<float> BottomHeavy => BottomHeavyPrototype.Instantiate();

            /// <summary>
            /// Checks if too many recent values were in the top of the range.
            /// </summary>
            public static IPatternMatcher<float> TopHeavy => TopHeavyPrototype.Instantiate();

            #endregion

            #region Prototypes

            /// <summary>
            /// prototype for <see cref="TooCloseToPrevious"/>.
            /// </summary>
            private static readonly IPrototype<float> TooCloseToPreviousPrototype = new NeighbourPatternMatcher<float>.Prototype
            (
                patternName: "Too small difference from last",
                checkAmount: 1,
                matchAmount: 1,
                neighbourOffset: 1,
                neighborComparison: Comparisons<float>.Difference(0.02f)
            );

            /// <summary>
            /// prototype for <see cref="TooCloseToPrevious2"/>.
            /// </summary>
            private static readonly IPrototype<float> TooCloseToPrevious2Prototype = new NeighbourPatternMatcher<float>.Prototype
            (
                patternName: "3 values too close",
                checkAmount: 2,
                matchAmount: 2,
                neighbourOffset: 1,
                neighborComparison: Comparisons<float>.Difference(0.1f)
            );

            /// <summary>
            /// prototype for <see cref="AscendingSequence"/>.
            /// </summary>
            private static readonly IPrototype<float> AscendingSequencePrototype = new NeighbourPatternMatcher<float>.Prototype
            (
                patternName: "Ascending Sequence",
                checkAmount: 4,
                matchAmount: 4,
                neighbourOffset: 1,
                neighborComparison: Comparisons<float>.GreaterThan
            );

            /// <summary>
            /// prototype for <see cref="DescendingSequence"/>.
            /// </summary>
            private static readonly IPrototype<float> DescendingSequencePrototype = new NeighbourPatternMatcher<float>.Prototype
            (
                patternName: "Descending Sequence",
                checkAmount: 4,
                matchAmount: 4,
                neighbourOffset: 1,
                neighborComparison: Comparisons<float>.LessThan
            );

            /// <summary>
            /// prototype for <see cref="BottomHeavy"/>.
            /// </summary>
            private static readonly IPrototype<float> BottomHeavyPrototype = new OccurencePatternMatcher<float>.Prototype
            (
                patternName: "Too Many in bottom of range",
                matchCount: 7,
                minLookBackCount: 7,
                maxLookBackCount: 10,
                predicateBuilderFunction: Comparisons<float>.PercentageBetween(0, 0.3f)
            );

            /// <summary>
            /// prototype for <see cref="TopHeavy"/>.
            /// </summary>
            private static readonly IPrototype<float> TopHeavyPrototype = new OccurencePatternMatcher<float>.Prototype
            (
                patternName: "Too Many in top of range",
                matchCount: 7,
                minLookBackCount: 7,
                maxLookBackCount: 10,
                predicateBuilderFunction: Comparisons<float>.PercentageBetween(0.7f, 1)
            );

            #endregion
        }
    }
}