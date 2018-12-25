using UnityEngine;
using Chinchillada.RandomExtensions.FilteredRandom;

namespace Chinchillada.RandomExtensions.Components
{
    /// <summary>
    /// Component wrapper for <see cref="FilteredRandomInt"/>.
    /// </summary>
    public class FilteredIntComponent : FilteredRandomComponent<int>
    {
        /// <summary>
        /// The lowest value of the range that can be generated.
        /// </summary>
        [SerializeField] private int _rangeMin;

        /// <summary>
        /// The highest value of the range that can be generated.
        /// </summary>
        [SerializeField] private int _rangeMax;

        /// <summary>
        /// The <see cref="FilteredRandomInt"/> that this component wraps.
        /// </summary>
        private FilteredRandomInt _filteredRandomInt;

        /// <inheritdoc />
        protected override IFilteredRandom<int> FilteredRandom => _filteredRandomInt;

        /// <inheritdoc />
        public override void InitializeGenerator(RandomFilter<int> filter = null)
        {
            _filteredRandomInt = new FilteredRandomInt(_rangeMin, _rangeMax, filter: filter);
        }
    }
}
