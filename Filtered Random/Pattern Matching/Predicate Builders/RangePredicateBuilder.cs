using System;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    internal class RangePredicateBuilder<TValue, TParameter> : IPredicateBuilder<TValue, TParameter>
    {
        /// <summary>
        /// The function that takes the range and builds the predicate.
        /// </summary>
        private readonly Func<TValue, TValue, Func<TParameter, bool>> _predicateBuilderFunction;

        /// <summary>
        /// The lower bound of the range of allowed values.
        /// </summary>
        private TValue _min;

        /// <summary>
        /// The upper bound of the range of allowed values.
        /// </summary>
        private TValue _max;

        /// <summary>
        /// Constructs a new range-predicate builder.
        /// </summary>
        /// <param name="predicateBuilderFunction">The function that takes the range and builds the predicate.</param>
        public RangePredicateBuilder(Func<TValue, TValue, Func<TParameter, bool>> predicateBuilderFunction)
        {
            _predicateBuilderFunction = predicateBuilderFunction;
        }

        /// <inheritdoc />
        public void SetRange(TValue min, TValue max)
        {
            _min = min;
            _max = max;
        }

        /// <inheritdoc />
        public Func<TParameter, bool> BuildPredicate()
        {
            return _predicateBuilderFunction(_min, _max);
        }
    }
}
