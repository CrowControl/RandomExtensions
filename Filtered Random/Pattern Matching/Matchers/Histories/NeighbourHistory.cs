using System;
using System.Linq;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    /// <summary>
    /// History for <see cref="NeighbourPatternMatcher{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of values to remember.</typeparam>
    internal class NeighbourHistory<T> : PatternMatcherHistory<T> where T : IComparable
    {
        /// <summary>
        /// The offset between neighbours that are compared.
        /// </summary>
        private readonly int _neighbourOffset;

        public NeighbourHistory(int memory, int neighbourOffset) : base(memory)
        {
            _neighbourOffset = neighbourOffset;
        }

        /// <inheritdoc />
        protected override void OnHistoryValueDequeued()
        {
            //Check if it was a match.
            bool match = HistoryMatches.Count > _neighbourOffset 
                      && HistoryMatches.ElementAt(_neighbourOffset);

            //Deuque.
            HistoryMatches.Dequeue();
            if (match) Matches--;
        }
    }
}