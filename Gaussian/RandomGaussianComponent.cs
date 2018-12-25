using UnityEngine;

namespace Chinchillada.RandomExtensions.Guassian
{
    /// <summary>
    /// Component wrapper for <see cref="GaussianGenerator"/>.
    /// </summary>
    public class RandomGaussianComponent : MonoBehaviour, IGaussianGenerator
    {
        /// <summary>
        /// The mean of the gaussian distribution to generate values in.
        /// </summary>
        [SerializeField] private float _mean = 0;

        /// <summary>
        /// The standard deviation of the distribution to generate values in.
        /// </summary>
        [SerializeField] private float _standardDeviation = 1;

        /// <summary>
        /// The <see cref="GaussianGenerator"/> that this component wraps.
        /// </summary>
        private GaussianGenerator _gaussianGenerator;

        /// <inheritdoc />
        public float Mean => _mean;

        /// <inheritdoc />
        public float StandardDeviation => _standardDeviation;

        /// <summary>
        /// Called when the gameObject this component is on first awakes.
        /// Initializes the generator.
        /// </summary>
        private void Awake()
        {
            InitializeGenerator();
        }

        /// <summary>
        /// Called anytime the editor values of this component are changed.
        /// Initializes the generator.
        /// </summary>
        private void OnValidate()
        {
            InitializeGenerator();
        }

        /// <summary>
        /// Initializes the generator.
        /// </summary>
        private void InitializeGenerator()
        {
            _gaussianGenerator = new GaussianGenerator(_mean, _standardDeviation);
        }

        /// <inheritdoc />
        public float Generate()
        {
            return _gaussianGenerator.Generate();
        }
    }
}