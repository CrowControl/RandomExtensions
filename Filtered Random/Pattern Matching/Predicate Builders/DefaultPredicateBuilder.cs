using System;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    /// <summary>
    /// Predicate builder that takes and returns a ready-for-action predicate without doing anything with it.
    /// </summary>
    /// <typeparam name="TParameter">Type of the predicate parameter.</typeparam>
    /// <typeparam name="TValue">Type of the values we run the predicate over.</typeparam>
    internal class DefaultPredicateBuilder<TValue, TParameter> : IPredicateBuilder<TValue, TParameter>
    {
        private readonly Func<TParameter, bool> _predicate;

        /// <summary>
        /// Constructs a new default predicate builder.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        public DefaultPredicateBuilder(Func<TParameter, bool> predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public void SetRange(TValue min, TValue max){}

        /// <summary>
        /// The predicate.
        /// </summary>
        public Func<TParameter, bool> BuildPredicate()
        {
            return _predicate;
        }
    }
}
