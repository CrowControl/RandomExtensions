using Chinchillada.RandomExtensions.FilteredRandom;

namespace Chinchillada.RandomExtensions.Components
{
    /// <summary>
    /// Component wrapper for <see cref="FilteredRandomBool"/>.
    /// </summary>
    public class FilteredBoolComponent : FilteredRandomComponent<bool>
    {
        /// <summary>
        /// The <see cref="FilteredRandomBool"/> that this component wraps.
        /// </summary>
        private FilteredRandomBool _filteredRandomBool;
        
        /// <inheritdoc />
        protected override IFilteredRandom<bool> FilteredRandom => _filteredRandomBool;

        /// <inheritdoc />
        public override void InitializeGenerator(RandomFilter<bool> filter = null)
        {
            _filteredRandomBool = new FilteredRandomBool(filter: filter);
        }
    }
}
