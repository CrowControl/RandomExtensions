using UnityEngine;
using Chinchillada.RandomExtensions.FilteredRandom;

namespace Chinchillada.RandomExtensions.Components
{
    /// <summary>
    /// Component wrapper for <see cref="FilteredRandomFloat"/>.
    /// </summary>
    internal class FilteredGaussianComponent : FilteredRandomComponent<float>
    {
        /// <summary>
        /// The mean of the gaussian distribution to generate values in.
        /// </summary>
        [SerializeField] private float _mean;

        /// <summary>
        /// The standard deviation of the distribution to generate values in.
        /// </summary>
        [SerializeField] private float _standardDeviation;
        
        /// <summary>
        /// The <see cref="FilteredGuassian"/> that this component wraps.
        /// </summary>
        private FilteredGuassian _filteredGuassian;

        /// <inheritdoc />
        protected override IFilteredRandom<float> FilteredRandom => _filteredGuassian;

        /// <inheritdoc />
        public override void InitializeGenerator(RandomFilter<float> filter = null)
        {
            _filteredGuassian = new FilteredGuassian(_mean, _standardDeviation, filter: filter);
        }
    }
}
