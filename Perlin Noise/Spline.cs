using System;
using UnityEngine;

namespace Chinchillada.RandomExtensions.PerlinNoise
{
    /// <summary>
    /// A spline that is defined by a simple function.
    /// </summary>
    internal class Spline : IInterval
    {
        //The function that defines the spline.
        public Func<float, float> DefinitionFunction { get; }

        /// <inheritdoc />
        public float StartPoint { get; }

        /// <inheritdoc />
        public float EndPoint { get; }

        /// <summary>
        /// Construct a new spline.
        /// </summary>
        /// <param name="startPoint">The starting point of the interval.</param>
        /// <param name="endPoint">The final point of the spline.</param>
        /// <param name="definitionFunction">The function that defines the shape of the spline. If left null, <see cref="Predefined.SCurve"/> is used.</param>
        public Spline(float startPoint, float endPoint, Func<float, float> definitionFunction = null)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;

            DefinitionFunction = definitionFunction ?? Predefined.SCurve;
        }

        /// <summary>
        /// Samples the spline at the given point between 0 and 1.
        /// </summary>
        /// <remarks>Expects a value between 0 and 1. Will clamp values outside of that range.</remarks>
        /// <param name="point">The point endPoint sample at.</param>
        /// <returns>The sampled value.</returns>
        public float Sample(float point)
        {
            float sample = DefinitionFunction(point);
            return Mathf.Lerp(StartPoint, EndPoint, sample);
        }

        public class Predefined
        {
            /// <summary>
            /// A Nice S-like curve shape.
            /// </summary>
            public static Func<float, float> SCurve = point =>
                (float) (6 * Math.Pow(point, 5) -
                         15 * Math.Pow(point, 4) +
                         10 * Math.Pow(point, 3));
        }
    }
}
