using UnityEngine;

namespace Chinchillada.RandomExtensions.FilteredRandom
{
    /// <summary>
    /// Base class for Component wrappers for <see cref="FilteredRandom"/> implementations.
    /// </summary>
    /// <typeparam name="T">Type of value to generate.</typeparam>
    public abstract class FilteredRandomComponent<T> : MonoBehaviour, IFilteredRandomComponent<T>
    {
        /// <summary>
        /// The filtered random implementation.
        /// </summary>
        protected abstract IFilteredRandom<T> FilteredRandom { get; }
        
        /// <inheritdoc />
        public abstract void InitializeGenerator(RandomFilter<T> filter = null);

        /// <inheritdoc cref="IFilteredRandomComponent{T}.Filter" />
        public RandomFilter<T> Filter
        {
            get { return FilteredRandom.Filter; }
            set { InitializeGenerator(value); }
        }

        /// <summary>
        /// Called when the object awakes for the first time.
        /// Initializes the generator.
        /// </summary>
        protected virtual void Awake()
        {
            InitializeGenerator();
        }

        /// <summary>
        /// Called when the values in the editor have changed.
        /// Initializes the generator.
        /// </summary>
        protected virtual void OnValidate()
        {
            InitializeGenerator();
        }

        /// <summary>
        /// Generates a filtered random value of Type <see cref="T"/>.
        /// </summary>
        /// <returns>The generated value.</returns>
        public T Generate()
        {
            return FilteredRandom.Generate();
        }
    }
}
