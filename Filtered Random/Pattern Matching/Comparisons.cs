using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada.RandomExtensions.Utilities;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    /// <summary>
    /// Static class the has all the comparison functions used by the pattern matchers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class Comparisons<T> where T : IComparable
    {
        /// <summary>
        /// Comparer used by some of the comparisons.
        /// </summary>
        private static readonly Comparer<T> Comparer = Comparer<T>.Default;

        #region Pairs

        /// <summary>
        /// Checks if the 2 values are equal.
        /// </summary>
        public static bool AreEqual(T x, T y)
        {
            return Comparer.Compare(x, y) == 0;
        }

        /// <summary>
        /// Checks if the value is equal to it's neigbour.
        /// </summary>
        public static Func<Pair<T>, bool> Equality => pair =>
            AreEqual(pair.Item1, pair.Item2);

        /// <summary>
        /// Checks if the value is not equal to it's neighbour.
        /// </summary>
        public static Func<Pair<T>, bool> NotEqual => pair => !Equality(pair);

        /// <summary>
        /// Checks if the value is greater than it's neighbour.
        /// </summary>
        public static Func<Pair<T>, bool> GreaterThan => pair =>
            Comparer.Compare(pair.Item1, pair.Item2) > 0;

        /// <summary>
        /// Checks if the value is lesser than it's neighbour.
        /// </summary>
        public static Func<Pair<T>, bool> LessThan => pair =>
            Comparer.Compare(pair.Item1, pair.Item2) < 0;

        /// <summary>
        /// Checks if the difference is bigger than the <paramref name="treshold"/>
        /// </summary>
        /// <param name="treshold">The value the difference can't e bigger than.</param>
        /// <returns>True if the difference is bigger, false if it's smaller or equal.</returns>
        public static Func<Pair<float>, bool> Difference(float treshold)
        {
            return pair => Math.Abs(pair.Item1 - pair.Item2) < treshold;
        }

        #endregion

        #region Sequence

        /// <summary>
        /// Checks if final sequence happens a specific amount of times.
        /// </summary>
        /// <param name="sequenceLength">The length of the sequence at the end we want to check.</param>
        /// <param name="duplicateCount">The amount of duplicate sequences we want to check for.</param>
        public static Func<List<T>, bool> DuplicateSequence(int sequenceLength, int duplicateCount)
        {
            return (list) =>
            {
                //get length of list.
                int listLength = list.Count;

                if (listLength <= sequenceLength)
                    return false;

                // Build array of the final sequence.
                T[] finalSequence = new T[sequenceLength];
                for (int i = 1; i <= sequenceLength; i++)
                    finalSequence[sequenceLength - i] = list[listLength - i];

                int duplicateSequences = 0;

                //Check if the sequence occurs earlier in the list.
                for (int i = listLength - 1; i >= sequenceLength; i--)
                {
                    bool duplicate = true;
                    for (int j = 1; j <= sequenceLength; j++)
                    {
                        //While it remains equal continue.
                        if (Comparer.Compare(list[i - j], finalSequence[sequenceLength - j]) == 0)
                            continue;

                        duplicate = false;
                        break;
                    }

                    if (!duplicate)
                        continue;

                    //We found a duplicate sequence.
                    duplicateSequences++;
                    if (duplicateSequences >= duplicateCount)
                        return true;
                }

                //No sequence found.
                return false;
            };
        }

        /// <summary>
        /// Checks if there are opposite sequences.
        /// <example> 111000 </example>
        /// </summary>
        public static Func<List<bool>, bool> OppositeSequence =>
            list =>
            {
                //Get first and last value.
                bool first = list.First();
                bool last = list.Last();

                //These should be opposite.
                if (first == last)
                    return false;

                //Entire first half should be the same.
                int half = list.Count / 2;
                for (int i = 1; i < half; i++)
                {
                    if (list[i] != first)
                        return false;
                }

                //Entire second half should be the same.
                for (int i = half; i < list.Count - 1; i++)
                {
                    if (list[i] != last)
                        return false;
                }

                return true;
            };

        #endregion

        #region Range   

        /// <summary>
        /// Checks if the value's percentage (0-100) between the min and maxx is in the allowed range.
        /// </summary>
        /// <param name="minimumRangeSize">The minimum size of the range before this comparison starts trying.</param>
        /// <param name="allowedMin">The minimum allowed percentage.</param>
        /// <param name="allowedMax">The maximum allowed percentage.</param>
        public static Func<int, int, Func<int, bool>> PercentageBetween(int minimumRangeSize, 
                                                                        int allowedMin,
                                                                        int allowedMax)
        {
            return (min, max) =>
            {
                //If the size of the range is too small, never match.
                int rangeSize = Math.Abs(min - max);
                if (rangeSize < minimumRangeSize)
                    return value => false;

                return value =>
                {
                    float percentage = MathHelper.PercentageBetween(value, min, max);
                    return percentage >= allowedMin && percentage <= allowedMax;
                };
            };
        }

        /// <summary>
        /// Checks if the value's percentage(0-1) between the min and amx is in the allowed range.
        /// </summary>
        /// <param name="allowedMin">The minimum allowed percentage.</param>
        /// <param name="allowedMax">The maximum allowed percentage.</param>
        public static Func<float, float, Func<float, bool>> PercentageBetween(float allowedMin, float allowedMax)
        {
            return (min, max) => value =>
            {
                float percentage = MathHelper.PercentageBetween(value, min, max);
                return percentage >= allowedMin && percentage <= allowedMax;
            };
        }

        /// <summary>
        /// Checks if a value is below the <paramref name="percentage"/> in the given range.
        /// </summary>
        /// <param name="percentage">the percentage that the value should be below.</param>
        /// <returns>True if below percentage.</returns>
        public static Func<float, float, Func<float, bool>> BelowPercentage(float percentage)
        {
            return (min, max) => value =>
            {
                float valuePercentage = MathHelper.PercentageBetween(value, min, max);
                return valuePercentage < percentage;
            };
        }

        /// <summary>
        /// Checks if a value is above the <paramref name="percentage"/> in the given range.
        /// </summary>
        /// <param name="percentage">the percentage that the value should above.</param>
        /// <returns>True if above percentage.</returns>
        public static Func<float, float, Func<float, bool>> AbovePercentage(float percentage)
        {
            return (min, max) => value =>
            {
                float valuePercentage = MathHelper.PercentageBetween(value, min, max);
                return valuePercentage > percentage;
            };
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Combines the <paramref name="functions"/> into a single function that returns true if any of the functions would have returned true.
        /// </summary>
        /// <param name="functions">The functions to combine.</param>
        /// <returns>The combined function.</returns>
        public static Func<T, T, Func<T, bool>> CombineFunctionsAny(params Func<T, T, Func<T, bool>>[] functions)
        {
            return (min, max) => value => functions.Any(function => function(min, max)(value));
        }

        #endregion
    }
}