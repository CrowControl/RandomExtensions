using System.Collections.Generic;
using Chinchillada.RandomExtensions.FilteredRandom.PatternMatchers;
using Chinchillada.RandomExtensions.List;

namespace Chinchillada.RandomExtensions.FilteredRandom
{
    /// <summary>
    /// Base abstract class for the Filtered randoms.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FilteredRandom<T> : IFilteredRandom<T>
    {
        /// <summary>
        /// The filter.
        /// </summary>
        public RandomFilter<T> Filter { get; }

        /// <summary>
        /// The random number generator used to generate values.
        /// </summary>
        protected IRandomGenerator RNG { get; }

        /// <summary>
        /// The generator used to generate the values we filter.
        /// </summary>
        protected abstract IGenerator<T> Generator { get; }
        
        /// <summary>
        /// Get all pattern matchers.
        /// </summary>
        /// <returns></returns>
        public abstract List<IPatternMatcher<T>> GetAllPatternMatchers();

        /// <summary>
        /// Get all pattern matcher prototypes.
        /// </summary>
        /// <returns></returns>
        public abstract List<IPrototype<T>> GetAllPatternMatcherProtoTypes();

        #region Initialization

        /// <summary>
        /// Initializes the Filtered Random generator. 
        /// </summary>
        /// <param name="generator">The number generator to generate numbers with. <see cref="RandomGenerator.Instance"/> used as default.</param>
        /// <param name="filter">The filter to use. Uses the full set of defined binary filters if no filters are provided.</param>
        protected FilteredRandom(IRandomGenerator generator = null, RandomFilter<T> filter = null)
        {
            RNG = generator ?? RandomGenerator.Instance;
            Filter = filter ?? GetFullFilter();
        }

        /// <summary>
        /// Returns a filter with all the default pattern matchers.
        /// </summary>
        /// <returns>The full filter.</returns>
        private RandomFilter<T> GetFullFilter()
        {
            List<IPatternMatcher<T>> patternMatchers = GetAllPatternMatchers();
            return new RandomFilter<T>(patternMatchers);
        }

        /// <summary>
        /// Passes the range to the filter.
        /// </summary>
        /// <param name="min">The underbound of the range.</param>
        /// <param name="max">The upper bound of the range.s</param>
        protected void SetRange(T min, T max)
        {
            Filter.SetRange(min, max);
        }

        #endregion

        /// <summary>
        /// Generates a valid value.
        /// </summary>
        public T Generate()
        {
            T value = ListRandom.GenerateValid(Generator.Generate, Filter.MatchNewValue);
            Filter.RegisterValue(value);

            //Return.
            return value;
        }
    }
}
