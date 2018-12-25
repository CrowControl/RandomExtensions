using System;

namespace Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers
{
    /// <summary>
    /// Interface for predicate builders, which builds a predicate used by the <see cref="IPatternMatcher{T}"/>.
    /// These are necessary because more data is necessary for the predicates than is available on instantiation of the pattern matchers,
    /// so these can be given at a later point to the predicate builder before it builds the predicate.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TParameter"></typeparam>
    internal interface IPredicateBuilder<in TValue, in TParameter>
    {
        /// <summary>
        /// Set the range of possible values.
        /// </summary>
        /// <remarks>Only some pattern matchers require this.</remarks>
        /// <param name="min">The underbound of the allowed range.</param>
        /// <param name="max">The upperbound of the allowed range.</param>
        void SetRange(TValue min, TValue max);

        /// <summary>
        /// Builds the predicate.
        /// </summary>
        /// <returns>The predicate.</returns>
        Func<TParameter, bool> BuildPredicate();
    }
}
