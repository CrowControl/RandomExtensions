using System;
using System.Linq;
using Chinchillada.RandomExtensions.List;

namespace Chinchillada.RandomExtensions.PerlinNoise
{
    /// <summary>
    /// An interval made up of multiple randomized sub-intervals.
    /// </summary>
    internal class Octave : IInterval
    {
        /// <summary>
        /// The amount of points that make up the octave.
        /// </summary>
        private readonly int _pointCount;

        /// <summary>
        /// The weight of this octave in the perlin noise interval it belongs to.
        /// </summary>
        private readonly double _amplitude;

        /// <summary>
        /// The function that defines the splines that make up the octave.
        /// </summary>
        private readonly Func<float, float> _splineDefinition;

        /// <summary>
        /// The random number generator used to generate the points.
        /// </summary>
        private readonly IRandomGenerator _rng;

        /// <summary>
        /// The splines that make up the octave.
        /// </summary>
        private readonly Spline[] _splines;

        /// <summary>
        /// The size of the range of a single spline.
        /// </summary>
        private readonly float _splineRange;

        /// <inheritdoc />
        public float StartPoint { get; }

        /// <inheritdoc />
        public float EndPoint { get; }

        #region Constructors

        /// <summary>
        /// Constructs a new octave.
        /// </summary>
        /// <param name="pointCount">The amount of random points to define this octave.</param>
        /// <param name="amplitude">The amplitude if this octave.</param>
        /// <param name="splineDefinition">The function that defines the spline shape.</param>
        /// <param name="rng">The random number generator used.</param>
        public Octave(int pointCount, double amplitude, IRandomGenerator rng = null, Func<float, float> splineDefinition = null)
        {
            // Cache the parameters.
            _pointCount = pointCount;
            _amplitude = amplitude;
            _splineDefinition = splineDefinition ?? Spline.Predefined.SCurve;
            _rng = rng ?? RandomGenerator.Instance;

            //Generate the points.
            float[] points = GeneratePoints(pointCount, rng);

            //Save the bounds.
            StartPoint = points.First();
            EndPoint = points.Last();

            //Initialize the splines.
            _splines = InitializeSplines(points, splineDefinition);

            // Calculate how big the range of a single spline is.
            _splineRange = 1f / _splines.Length;
        }

        /// <summary>
        /// Constructs an octave from the <paramref name="splines"/>.
        /// </summary>
        /// <param name="amplitude">The amplitude.</param>
        /// <param name="splines">The splines.</param>
        /// <param name="rng">The rng used to generat points.</param>
        public Octave(double amplitude, Spline[] splines, IRandomGenerator rng = null)
        {
            //Cache parameters.
            _splines = splines;
            _amplitude = amplitude;
            _rng = rng ?? RandomGenerator.Instance;

            //Extract data from splines.
            _pointCount = splines.Length - 1;
            _splineDefinition = splines.First().DefinitionFunction;

            //Save the bounds.
            StartPoint = _splines.First().StartPoint;
            EndPoint = _splines.Last().EndPoint;

            // Calculate how big the range of a single spline is.
            _splineRange = 1f / _splines.Length;
        }

        /// <summary>
        /// Constructs a new octave with a given first point.
        /// </summary>
        /// <param name="firstPoint">The first point.</param>
        /// <param name="pointCount">The amount of points that make up the octave.</param>
        /// <param name="amplitude">The weight of this octave in the perlin noise interval it belongs to.</param>
        /// <param name="rng">The random number generator used to generate the points.</param>
        /// <param name="samplingFunction">The function that defines the splines that make up the octave.</param>
        private Octave(float firstPoint, int pointCount, double amplitude,
                       IRandomGenerator rng, Func<float, float> samplingFunction)
            : this(pointCount, amplitude, rng, samplingFunction)
        {
            //Override the first spline with a new one that uses the given first point.
            Spline firstSpline = _splines.First();
            Spline spline = new Spline(firstPoint, firstSpline.EndPoint);

            //Save the overriding spline.
            _splines[0] = spline;
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Generates the random points that define this octave.
        /// </summary>
        /// <param name="pointCount">The amount of points.</param>
        /// <param name="rng">The random number generator used to generate the points.</param>
        /// <returns>The generated points.</returns>
        private static float[] GeneratePoints(int pointCount, IRandomGenerator rng)
        {
            return ListRandom.GenerateRandom(() => rng.RandomRange(), pointCount).ToArray();
        }

        /// <summary>
        /// Initializes the splines that interpolates between the subsequent points that define this octave.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="splineDefinition">The function that the splines use to interpolate.</param>
        private static Spline[] InitializeSplines(float[] points, Func<float, float> splineDefinition)
        {
            //Initialize array.
            Spline[] splines = new Spline[points.Length - 1];

            // Get first point.
            float fromPoint = points.First();

            for (int index = 0; index < splines.Length; index++)
            {
                // Get second point.
                float toPoint = points[index + 1];

                //Construct and cache spline.
                Spline spline = new Spline(fromPoint, toPoint, splineDefinition);
                splines[index] = spline;

                //Save second point to use as first for the next spline.
                fromPoint = toPoint;
            }

            return splines;
        }

        #endregion

        #region Sampling

        /// <summary>
        /// Samples the point between 0 and 1.
        /// </summary>
        public float SampleWeighted(float point)
        {
            float sample = Sample(point);
            return (float) (sample * _amplitude);
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float Sample(float point)
        {
            //Special case for absolute final value.
            if (point >= 1)
                return _splines.Last().Sample(point);
            
            //Calculate spline index.
            float x = point / _splineRange;
            int splineIndex = (int)Math.Floor(x);

            //Get spline.
            Spline spline = _splines[splineIndex];

            //Calculate the point's relative value in the interval of the spline.
            float intervalStart = splineIndex * _splineRange;
            float pointRemainder = point - intervalStart;
            float pointInSplineInterval = pointRemainder * _splines.Length;

            //Sample the spline.
            return spline.Sample(pointInSplineInterval);
        }

        #endregion

        /// <summary>
        /// Generates a succesor octave.
        /// </summary>
        public Octave GenerateSuccesor()
        {
            Spline lastSpline = _splines.Last();
            float lastPoint = lastSpline.EndPoint;

            return new Octave(lastPoint, _pointCount, _amplitude, _rng, _splineDefinition);
        }
    }
}