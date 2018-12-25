using UnityEngine;
using Chinchillada.RandomExtensions.FilteredRandom;

namespace Chinchillada.RandomExtensions.Scripts
{
    /// <summary>
    /// Component wrapper for <see cref="FilteredRandomFloat"/>.
    /// </summary>
    internal class FilteredFloatComponent : FilteredRandomComponent<float>
    {
        /// <summary>
        /// The lowest value of the range that can be generated.
        /// </summary>
        [SerializeField] private float _rangeMin;

        /// <summary>
        /// The highest value of the range that can be generated.
        /// </summary>
        [SerializeField] private float _rangeMax;

        /// <summary>
        /// The <see cref="FilteredRandomFloat"/> that this component wraps.
        /// </summary>
        private FilteredRandomFloat _filteredRandomFloat;
        
        /// <inheritdoc />
        protected override IFilteredRandom<float> FilteredRandom => _filteredRandomFloat;

        /// <inheritdoc />
        public override void InitializeGenerator(RandomFilter<float> filter = null)
        {
            _filteredRandomFloat = new FilteredRandomFloat(_rangeMin, _rangeMax, filter: filter);
        }
    }
}
