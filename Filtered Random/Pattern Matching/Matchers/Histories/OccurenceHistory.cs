using System;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    /// <summary>
    /// History for the <see cref="OccurencePatternMatcher{T}"/>.
    /// Keeps track of the amount of matches in the current history.
    /// </summary>
    internal class OccurenceHistory<T> : PatternMatcherHistory<T> where T : IComparable
    {
        public OccurenceHistory(int memory) : base(memory) { }

        /// <inheritdoc />
        protected override void OnHistoryValueDequeued()
        {
            bool match = HistoryMatches.Dequeue();

            if (match)
                Matches--;
        }
    }
}
