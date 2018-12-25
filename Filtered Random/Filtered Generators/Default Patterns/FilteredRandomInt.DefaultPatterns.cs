using System.Collections.Generic;
using System.Linq;
using Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers;

namespace Chinchillada.RandomExtensions.FilteredRandom
{
    public partial class FilteredRandomInt
    {
        /// <inheritdoc />
        public override List<IPatternMatcher<int>> GetAllPatternMatchers()
        {
            return DefaultPatterns.All;
        }

        /// <inheritdoc />
        public override List<IPrototype<int>> GetAllPatternMatcherProtoTypes()
        {
            return DefaultPatterns.Prototypes;
        }

        /// <summary>
        /// The collection of predefined patterns to filter random floating point values.
        /// </summary>
        public static class DefaultPatterns
        {
            #region Properties

            /// <summary>
            /// All the default patterns.
            /// </summary>
            public static List<IPatternMatcher<int>> All => Prototypes.Select(s => s.Instantiate()).ToList();

            /// <summary>
            /// All the default pattern prototypes.
            /// </summary>
            public static List<IPrototype<int>> Prototypes => new List<IPrototype<int>>
            {
                RepeatingNumberPrototype,
                AlternatedNumberPrototype,
                AscendingSequencePrototype,
                DescendingSequencePrototype,
                RepeatingPairPrototype,
                SingleNumberOccurencePrototype,
                BottomHeavyPrototype,
                TopHeavyPrototype
            };

            /// <summary>
            /// Matches for a number repeated twice in a row.
            /// </summary>
            public static IPatternMatcher<int> RepeatingNumber => RepeatingNumberPrototype.Instantiate();

            /// <summary>
            /// Matches for a number being alternated.
            /// </summary>
            public static IPatternMatcher<int> AlternatedNumber => AlternatedNumberPrototype.Instantiate();

            /// <summary>
            /// Matches for a sequence of 4 ascending values.
            /// </summary>
            public static IPatternMatcher<int> AscendingSequence => AscendingSequencePrototype.Instantiate();

            /// <summary>
            /// Matches for a sequence of 4 descending values.
            /// </summary>
            public static IPatternMatcher<int> DescendingSequence => DescendingSequencePrototype.Instantiate();
            
            
            /// <summary>
            /// Matches for a sequence of 4 descending values.
            /// </summary>
            public static IPatternMatcher<int> RepeatingPair => RepeatingPairPrototype.Instantiate();
            
            /// <summary>
            /// Matches for a single number occuring to many times.
            /// </summary>
            public static IPatternMatcher<int> SingleNumberOccurence => SingleNumberOccurencePrototype.Instantiate();
            
            /// <summary>
            /// Matches for too many recent values appearing in the bottom of the range.
            /// </summary>
            public static IPatternMatcher<int> BottomHeavy => BottomHeavyPrototype.Instantiate();

            
            /// <summary>
            /// Matches for too many recent values appearing in the top of the range.
            /// </summary>
            public static IPatternMatcher<int> TopHeavy => TopHeavyPrototype.Instantiate();

            #endregion

            #region Prototypes

            /// <summary>
            /// Prototype for <see cref="RepeatingNumber"/>.
            /// </summary>
            private static readonly IPrototype<int> RepeatingNumberPrototype = new NeighbourPatternMatcher<int>.Prototype
            (
                patternName: "Repeating Number",
                checkAmount: 1,
                matchAmount: 1,
                neighbourOffset: 1,
                neighborComparison: Comparisons<int>.Equality
            );

            /// <summary>
            /// Prototype for <see cref="AlternatedNumber"/>.
            /// </summary>
            private static readonly IPrototype<int> AlternatedNumberPrototype =
                new NeighbourPatternMatcher<int>.Prototype
                (
                    patternName: "Alternated Number",
                    checkAmount: 1,
                    matchAmount: 1,
                    neighbourOffset: 2,
                    neighborComparison: Comparisons<int>.Equality
                );

            /// <summary>
            /// Prototype for <see cref="AscendingSequence"/>.
            /// </summary>
            private static readonly IPrototype<int> AscendingSequencePrototype =
                new NeighbourPatternMatcher<int>.Prototype
                (
                    patternName: "Ascending Sequence",
                    checkAmount: 3,
                    matchAmount: 3,
                    neighbourOffset: 1,
                    neighborComparison: Comparisons<int>.GreaterThan
                );

            /// <summary>
            /// Prototype for <see cref="DescendingSequence"/>.
            /// </summary>
            private static readonly IPrototype<int> DescendingSequencePrototype =
                new NeighbourPatternMatcher<int>.Prototype
                (
                    patternName: "Descending Sequence",
                    checkAmount: 3,
                    matchAmount: 3,
                    neighbourOffset: 1,
                    neighborComparison: Comparisons<int>.LessThan
                );

            /// <summary>
            /// Prototype for <see cref="RepeatingPair"/>.
            /// </summary>
            private static readonly IPrototype<int> RepeatingPairPrototype
                = new ListPatternMatcher<int>.Prototype
                (
                    patternName: "Repeating 2 numbers",
                    minLookBackCount: 4,
                    maxLookBackCount: 9,
                    listPredicate: Comparisons<int>.DuplicateSequence(2, 1)
                );

            /// <summary>
            /// Prototype for <see cref="SingleNumberOccurence"/>.
            /// </summary>
            private static readonly IPrototype<int> SingleNumberOccurencePrototype
                = new ListPatternMatcher<int>.Prototype
                (
                    patternName: "Too Many of single number",
                    minLookBackCount: 4,
                    maxLookBackCount: 10,
                    listPredicate: Comparisons<int>.DuplicateSequence(1, 3)
                );

            /// <summary>
            /// Prototype for <see cref="BottomHeavy"/>.
            /// </summary>
            private static readonly IPrototype<int> BottomHeavyPrototype
                = new OccurencePatternMatcher<int>.Prototype
                (
                    patternName: "Bottom heavy",
                    matchCount: 7,
                    minLookBackCount: 7,
                    maxLookBackCount: 10,
                    predicateBuilderFunction: Comparisons<int>.PercentageBetween(10, 0, 30)
                );

            /// <summary>
            /// Prototype for <see cref="TopHeavy"/>.
            /// </summary>
            private static readonly IPrototype<int> TopHeavyPrototype
                = new OccurencePatternMatcher<int>.Prototype
                (
                    patternName: "Top Heavy",
                    matchCount: 7,
                    minLookBackCount: 7,
                    maxLookBackCount: 10,
                    predicateBuilderFunction: Comparisons<int>.PercentageBetween(10, 70, 100)
                );

            #endregion
        }
    }
}