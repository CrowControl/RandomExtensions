using System.Collections.Generic;
using System.Linq;
using Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers;

namespace Chinchillada.RandomExtensions.FilteredRandom
{
    public partial class FilteredRandomBool
    { 
        /// <inheritdoc />
        public override List<IPatternMatcher<bool>> GetAllPatternMatchers()
        {
            return DefaultPatterns.All;
        }

        /// <inheritdoc />
        public override List<IPrototype<bool>> GetAllPatternMatcherProtoTypes()
        {
            return DefaultPatterns.Prototypes;
        }

        /// <summary>
        /// The collection of predefined patterns to filter random boolean values.
        /// </summary>
        public class DefaultPatterns
        {
            #region Properties

            /// <summary>
            /// All default bool pattern matchers.
            /// </summary>
            public static List<IPatternMatcher<bool>> All => Prototypes.Select(p => p.Instantiate()).ToList();

            /// <summary>
            /// All default pattern prototypes.
            /// </summary>
            public static List<IPrototype<bool>> Prototypes => new List<IPrototype<bool>>
            {
                InARow4Prototype,
                DuplicateSequencePrototype,
                OppositeSequencePrototype
            };

            /// <summary>
            /// Matches for a value repeating 4 times.
            /// </summary>
            public static IPatternMatcher<bool> InARow4 => InARow4Prototype.Instantiate();

            /// <summary>
            /// Matches for a duplicate sequence.
            /// </summary>
            public static IPatternMatcher<bool> DuplicateSequence => DuplicateSequencePrototype.Instantiate();

            /// <summary>
            /// Matches for a two subsequent sequences that are boolean opposites of each other.
            /// </summary>
            public static IPatternMatcher<bool> OppositeSequence => OppositeSequencePrototype.Instantiate();

            #endregion

            #region Prototypes

            /// <summary>
            /// Prototype for <see cref="InARow4"/> pattern matcher.
            /// </summary>
            private static readonly IPrototype<bool> InARow4Prototype = new NeighbourPatternMatcher<bool>.Prototype
            (
                patternName: "4 in a row",
                checkAmount: 3,
                matchAmount: 3,
                neighbourOffset: 1,
                neighborComparison: Comparisons<bool>.Equality
            );

            /// <summary>
            /// Prototype for <see cref="DuplicateSequence"/> pattern matcher.
            /// </summary>
            private static readonly IPrototype<bool> DuplicateSequencePrototype = new NeighbourPatternMatcher<bool>.Prototype
            (
                patternName: "Duplicate sequence",
                checkAmount: 4,
                matchAmount: 4,
                neighbourOffset: 4,
                neighborComparison: Comparisons<bool>.Equality
            );

            /// <summary>
            /// Prototype for <see cref="DuplicateSequence"/> pattern matcher.
            /// </summary>
            private static readonly IPrototype<bool> OppositeSequencePrototype = new ListPatternMatcher<bool>.Prototype
            (
                patternName: "Opposite sequence",
                minLookBackCount: 5,
                maxLookBackCount: 5,
                listPredicate: Comparisons<bool>.OppositeSequence
            );

            #endregion
        }
    }
}