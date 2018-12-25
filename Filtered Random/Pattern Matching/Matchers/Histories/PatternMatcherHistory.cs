using System;
using System.Collections.Generic;
using Chinchillada.RandomExtensions.Utilities;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    /// <summary>
    /// Object that remembers how many matches are in the history.
    /// </summary>
    /// <typeparam name="T">Type of values.</typeparam>
    internal abstract class PatternMatcherHistory<T> where T : IComparable
    {
        /// <summary>
        /// Queue of values that remembers for each value in the <see cref="RandomFilter{T}"/>'s history if it triggers a match.
        /// </summary>
        protected readonly Queue<bool> HistoryMatches = new Queue<bool>();

        /// <summary>
        /// The previously generated values.
        /// </summary>
        private readonly FixedQueue<T> _historyValues;

        /// <summary>
        /// The values in this history.
        /// </summary>
        public List<T> Values => _historyValues.ToList();

        /// <summary>
        /// How many matches are currently in the value history.
        /// </summary>
        public int Matches { get; set; }

        /// <summary>
        /// The total amount of values in the history.
        /// </summary>
        public int Size => HistoryMatches.Count;

        //The last generated value and if it's a match.
        private T _lastValue;
        private bool _lastValueMatch;

        /// <summary>
        /// Constructs a new pattern matcher history.
        /// </summary>
        /// <param name="memory">The amount of values this history should remember.</param>
        protected PatternMatcherHistory(int memory)
        {
            _historyValues = new FixedQueue<T>(memory);
            _historyValues.ExcessDequeued += (value) => OnHistoryValueDequeued();
        }

        /// <summary>
        /// Returns the element at the <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The value at <paramref name="index"/>.</returns>
        public T AtIndex(int index)
        {
            return _historyValues.AtIndex(index);
        }

        /// <summary>
        /// Remembers if this value is a match to save that if it is added to the history.
        /// </summary>
        /// <param name="value">The value to remember.</param>
        /// <param name="isMatch">If the value causes a match or not.</param>
        public void CacheValue(T value, bool isMatch)
        {
            _lastValue = value;
            _lastValueMatch = isMatch;
        }

        /// <summary>
        /// Tries to save the value to the history.
        /// Only succeeds if this value was cached before.
        /// </summary>
        /// <param name="value"></param>
        public void SaveValue(T value)
        {
            //The value must be the last value te be cached.
            if (!Comparisons<T>.AreEqual(value, _lastValue))
                throw new InvalidOperationException(" Value added to history was not registered to the pattern matchers.");

            //Add the value.
            _historyValues.Enqueue(_lastValue);
            HistoryMatches.Enqueue(_lastValueMatch);

            //Update match counter.
            if (_lastValueMatch)
                Matches++;
        }

        /// <summary>
        /// Called when a value is dequeued from the history.
        /// Updates the match count.
        /// </summary>
        protected abstract void OnHistoryValueDequeued();
    }
}
