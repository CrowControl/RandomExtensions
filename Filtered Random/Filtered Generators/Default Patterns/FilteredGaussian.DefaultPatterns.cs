using System.Collections.Generic;
using System.Linq;
using Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers;

namespace Chinchillada.RandomExtensions.FilteredRandom
{
    public partial class FilteredGuassian
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
        /// The collection of predefined patterns to filter gaussian values.
        /// </summary>
        public class DefaultPatterns
        {
            /// <summary>
            /// All the default patterns.
            /// </summary>
            public static List<IPatternMatcher<float>> All => Prototypes.Select(p => p.Instantiate()).ToList();

            /// <summary>
            /// All default pattern prototypes.
            /// </summary>
            public static List<IPrototype<float>> Prototypes
            {
                get
                {
                    List<IPrototype<float>> prototypes = FilteredRandomFloat.DefaultPatterns.Prototypes;
                    prototypes.AddRange(new List<IPrototype<float>>
                    {
                        ConsecutiveAboveMeanPrototype,
                        ConsecutiveBelowMeanPrototype,
                        TooFewInFirstDeviationPrototype,
                        TooManyInThirdDeviationPrototype
                    });

                    return prototypes;
                }
            }

            #region Properties

            /// <summary>
            /// Matches for too many consecutive values being above the mean.
            /// </summary>
            public static IPatternMatcher<float> ConsecutiveAboveMean => ConsecutiveAboveMeanPrototype.Instantiate();

            /// <summary>
            /// Matches for too many consecutive values being below the mean.
            /// </summary>
            public static IPatternMatcher<float> ConsecutiveBelowMean => ConsecutiveBelowMeanPrototype.Instantiate();

            /// <summary>
            /// Matches for too few values being in the first deviation.
            /// </summary>
            public static IPatternMatcher<float> TooFewInFirstDeviation => TooFewInFirstDeviationPrototype.Instantiate();

            /// <summary>
            /// Matches for too many values being in the third deviation.
            /// </summary>
            public static IPatternMatcher<float> TooManyInThirdDeviation => TooManyInThirdDeviationPrototype.Instantiate();

            #region Float Patterns

            /// <summary>
            /// Matches for 2 values being too close to each other.
            /// </summary>
            public static IPatternMatcher<float> TooCloseToPrevious =>
                FilteredRandomFloat.DefaultPatterns.TooCloseToPrevious;

            /// <summary>
            /// Natches for 3 values being to close to each other.
            /// </summary>
            public static IPatternMatcher<float> TooCloseToPrevious2 =>
                FilteredRandomFloat.DefaultPatterns.TooCloseToPrevious2;

            /// <summary>
            /// Matches for a sequence of 4 ascending values.
            /// </summary>
            public static IPatternMatcher<float> AscendingSequence =>
                FilteredRandomFloat.DefaultPatterns.AscendingSequence;

            /// <summary>
            /// Matches for a sequence of 4 descending values.
            /// </summary>
            public static IPatternMatcher<float> DescendingSequence =>
                FilteredRandomFloat.DefaultPatterns.DescendingSequence;

            /// <summary>
            /// Checks if too many recent values were in the bottom of the range.
            /// </summary>
            public static IPatternMatcher<float> BottomHeavy => FilteredRandomFloat.DefaultPatterns.BottomHeavy;

            /// <summary>
            /// Checks if too many recent values were in the top of the range.
            /// </summary>
            public static IPatternMatcher<float> TopHeavy => FilteredRandomFloat.DefaultPatterns.TopHeavy;

            #endregion

            #endregion

            #region Prototypes

            /// <summary>
            /// Prototype for <see cref="ConsecutiveAboveMean"/>.
            /// </summary>
            private static readonly IPrototype<float> ConsecutiveAboveMeanPrototype =
                new OccurencePatternMatcher<float>.Prototype
                (
                    patternName: "Consecutive Above Mean",
                    matchCount: 4,
                    minLookBackCount: 1,
                    maxLookBackCount: 4,
                    predicateBuilderFunction: Comparisons<float>.AbovePercentage(0.5f)
                );

            /// <summary>
            /// Prototype for <see cref="ConsecutiveBelowMean"/>.
            /// </summary>
            private static readonly IPrototype<float> ConsecutiveBelowMeanPrototype =
                new OccurencePatternMatcher<float>.Prototype
                (
                    patternName: "Consecutive Below Mean",
                    matchCount: 4,
                    minLookBackCount: 1,
                    maxLookBackCount: 4,
                    predicateBuilderFunction: Comparisons<float>.BelowPercentage(0.5f)
                );

            /// <summary>
            /// Prototype for <see cref="TooFewInFirstDeviation"/>.
            /// </summary>
            private static readonly IPrototype<float> TooFewInFirstDeviationPrototype =
                new OccurencePatternMatcher<float>.Prototype
                (
                    patternName: "Too Few In First Deviation",
                    matchCount: 4,
                    minLookBackCount: 1,
                    maxLookBackCount: 4,
                    predicateBuilderFunction: Comparisons<float>.CombineFunctionsAny
                    (
                        Comparisons<float>.BelowPercentage(0.33f),
                        Comparisons<float>.AbovePercentage(0.67f)
                    )
                );

            /// <summary>
            /// Prototype for <see cref="TooManyInThirdDeviation"/>.
            /// </summary>
            private static readonly IPrototype<float> TooManyInThirdDeviationPrototype =
                new OccurencePatternMatcher<float>.Prototype
                (
                    patternName: "Too Many In Third Deviation",
                    matchCount: 4,
                    minLookBackCount: 1,
                    maxLookBackCount: 4,
                    predicateBuilderFunction: Comparisons<float>.CombineFunctionsAny
                    (
                        Comparisons<float>.BelowPercentage(0.17f),
                        Comparisons<float>.AbovePercentage(0.83f)
                    )
                );

            #endregion
        }
    }
}
