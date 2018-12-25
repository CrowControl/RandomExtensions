using System;

namespace Chinchillada.RandomExtensions.Utilities
{
    /// <summary>
    /// Object Container object that only initializes the object it contains when first requested.
    /// </summary>
    /// <typeparam name="T">The type of object.</typeparam>
    internal class LazyContainer<T>
    {
        /// <summary>
        /// The function that creates the value this property encompasses.
        /// </summary>
        private readonly Func<T> _factoryMethod;

        /// <summary>
        /// The value this property contains.
        /// </summary>
        private T _object;

        /// <summary>
        /// Flag for if the object is initialized yet or not.
        /// </summary>
        private bool _initialized;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="factoryMethod">The method that creates the value when it is requested the first time.</param>
        public LazyContainer(Func<T> factoryMethod)
        {
            _factoryMethod = factoryMethod;
        }

        /// <summary>
        /// Initializes the property.
        /// </summary>
        private void Initialize()
        {
            _object = _factoryMethod();
            _initialized = true;
        }

        /// <summary>
        /// Get the value for this property. 
        /// </summary>
        /// <remarks>This property is initialized the first time this is called.</remarks>
        /// <returns>The value.</returns>
        public T Get()
        {
            if (!_initialized)
                Initialize();
            
            return _object;
        }

        /// <summary>
        /// Sets the value. 
        /// </summary>
        /// <param name="value">The new value.</param>
        public void Set(T value)
        {
            _object = value;
            _initialized = true;
        }
    }
}
