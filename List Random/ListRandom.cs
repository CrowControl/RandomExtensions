using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada.RandomExtensions.Utilities;

namespace Chinchillada.RandomExtensions.List
{
    /// <summary>
    /// Class that adds extension methods for IEnumerable for choosing and taking random elements.
    /// </summary>
    public static class ListRandom
    {
        /// <summary>
        /// The amount of times random operations that might fail will retry before throwing an <see cref="TooManyTriesException"/>.
        /// </summary>
        public static int AttemptCount { get; set; } = 50;

        /// <summary>
        /// Private accessor to the global RNG instance.
        /// </summary>
        private static IRandomGenerator Random => RandomGenerator.Instance;

        /// <summary>
        /// Generates an <see cref="IEnumerable{T}"/> with the specified <paramref name="generationFunction"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements to generate.</typeparam>
        /// <param name="generationFunction">The function used to generate the elements.</param>
        /// <param name="amount">The amount of elements to generate.</param>
        /// <returns>A generated enumerable.</returns>
        public static IEnumerable<T> GenerateRandom<T>(Func<T> generationFunction, int amount)
        {
            T[] generated = new T[amount];

            for (int index = 0; index < amount; index++)
                generated[index] = generationFunction();

            return generated;
        }

        /// <summary>
        /// Uses the <paramref name="generationFunction"/> to generate a value that satisfies the <paramref name="validationPredicate"/>.
        /// </summary>
        /// <param name="generationFunction">The function that generates values.</param>
        /// <param name="validationPredicate">The predicate that validates.</param>
        /// <returns>A generated value that satisfies the <paramref name="validationPredicate"/>.</returns>
        public static T GenerateValid<T>(Func<T> generationFunction, Predicate<T> validationPredicate)
        {
            T value;
            int attempts = 0;

            do
            {
                //Throw exception if we tried to often.
                if(attempts > AttemptCount)
                    throw new TooManyTriesException();

                //Generate.
                value = generationFunction();

                //Update counter.
                attempts++;

                //Validate.
            } while (!validationPredicate(value));

            return value;
        }

        /// <summary>
        /// Generates multiple values that satisft the <paramref name="validationPredicate"/>.
        /// </summary>
        /// <param name="generationFunction">The function that generates values.</param>
        /// <param name="validationPredicate">The predicate that validates.</param>
        /// <param name="amount">The amount of values to generate.</param>
        /// <returns>The generated values.</returns>
        public static IEnumerable<T> GenerateValid<T>(Func<T> generationFunction, Predicate<T> validationPredicate, 
                                                      int amount)
        {
            Func<T> generator = () => GenerateValid(generationFunction, validationPredicate);
            return GenerateRandom(generator, amount);
        }

        #region Choosing

        /// <summary>
        /// Chooses a random element.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to choose an element of.</param>
        /// <returns>A randomly chosen element.</returns>
        public static T ChooseRandom<T>(this IEnumerable<T> enumerable)
        {
            IEnumerable<T> safeEnumerable = enumerable as T[] ?? enumerable.ToArray();

            int index = safeEnumerable.ChooseRandomIndex();
            return safeEnumerable.ElementAt(index);
        }

        /// <summary>
        /// Chooses multiple random elements.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to choose an element of.</param>
        /// <param name="amount">The amount of elements to choose.</param>
        /// <remarks>The same element may be chosen multiple times. If Distinct elements are needed, use <see cref="ChooseRandomDistinct{T}"/>.</remarks>
        /// <returns>The randomly chosen elements.</returns>
        public static IEnumerable<T> ChooseRandom<T>(this IEnumerable<T> enumerable, int amount)
        {
            return GenerateRandom(enumerable.ChooseRandom, amount);
        }

        /// <summary>
        /// Randomly chooses multiple distinct elements.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to choose an element of.</param>
        /// <param name="amount">The amount of elements to choose.</param>
        /// <remarks>If the <paramref name="amount"/> is more than the total amount of elements in <paramref name="enumerable"/>, the <paramref name="enumerable"/> is returned as is.</remarks>
        /// <returns>Multiple randomly chosen elements.</returns>
        public static IEnumerable<T> ChooseRandomDistinct<T>(this IEnumerable<T> enumerable, int amount)
        {
            List<T> list = enumerable.ToList();
            return list.Count <= amount ? list : list.GrabRandom(amount);
        }

        /// <summary>
        /// Chooses a random element from the <paramref name="enumerable"/> where the weights as determined by the <paramref name="weightFunction"/> bias the selection.
        /// </summary> 
        public static T ChooseRandomWeighted<T>(this IEnumerable<T> enumerable, Func<T, float> weightFunction)
        {
            Dictionary<T, float> elementsWithWeights = new Dictionary<T, float>();

            foreach (T element in enumerable)
            {
                float weight = weightFunction(element);
                elementsWithWeights[element] = weight;
            }

            return ChooseRandomWeighted(elementsWithWeights);
        }

        /// <summary>
        /// Chooses a random element in the <paramref name="collectionWithWeights"/>, where the weights are used to bias the selection.
        /// </summary> 
        public static T ChooseRandomWeighted<T>(this IDictionary<T, float> collectionWithWeights)
        {
            //Sum the weights.
            float weightSum = collectionWithWeights.Values.Sum();

            //Generate a value lower than the sum.
            float randomValue = Random.RandomRange(weightSum);
            
            foreach (T element in collectionWithWeights.Keys)
            {
                //Substract weight.
                float weight = collectionWithWeights[element];
                randomValue -= weight;

                //If zero is passed, return element.
                if (randomValue <= 0)
                    return element;
            }

            //Should not be reachable.
            throw new InvalidOperationException();
        }

        #region Where

        /// <summary>
        /// Chooses a random element that satisfies the predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">The enumerable to choose an element of.</param>
        /// <param name="predicate">The predicate the randomly chosen element should satisfy.</param>
        /// <returns>A randomly chosen element of <paramref name="enumerable"/> that satisfies <paramref name="predicate"/>.</returns>
        public static T ChooseRandomWhere<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            IEnumerable<T> validElements = enumerable.Where(predicate);
            return validElements.ChooseRandom();
        }

        /// <summary>
        /// Randomly chooses multiple elements that satisfy the <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">The enumerable to choose elements of.</param>
        /// <param name="predicate">The predicate the randomly chosen element should satisfy.</param>
        /// <param name="amount">The amount of random elements to select.</param>
        /// <returns>Multiple randomly chosen elements of <paramref name="enumerable"/> that satisfy the <paramref name="predicate"/>.</returns>
        public static IEnumerable<T> ChooseRandomWhere<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, int amount)
        {
            IEnumerable<T> validElements = enumerable.Where(predicate);
            return validElements.ChooseRandom(amount);
        }

        /// <summary>
        /// Randomly chooses multiple distinct elements that satisfy the <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to choose an element of.</param>
        /// <param name="predicate">The predicate that the chosen elements need to satisfy.</param>
        /// <param name="amount">The amount of elements to choose.</param>
        /// <returns>Multiple randomly chosen elements.</returns>
        public static IEnumerable<T> ChooseRandomDistinctWhere<T>(this IEnumerable<T> enumerable,
                                                                  Func<T, bool> predicate, int amount)
        {
            IEnumerable<T> validElements = enumerable.Where(predicate);
            return validElements.ChooseRandomDistinct(amount);
        }

        #endregion

        #region Preferred

        /// <summary>
        /// Chooses a random element, where elements that satisfy the <paramref name="predicate"/> are given precedence if they are 
        /// present in the <paramref name="enumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to choose elements of.</param>
        /// <param name="predicate">The predicate that preferred elements satisfy.</param>
        /// <returns>A randomly chosen element if any is present, or a completely randomly chosen element otherwise.</returns>
        public static T ChooseRandomPreferred<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            //Cast to arrays to avoid multiple enumerations.
            IEnumerable<T> asArray = enumerable.ToArray();
            IEnumerable<T> preferredElements = asArray.Where(predicate).ToArray();

            //Choose a preferred elements if there are any, otherwise any other possible element.
            return preferredElements.Any()
                ? preferredElements.ChooseRandom()
                : asArray.ChooseRandom();
        }

        /// <summary>
        /// Chooses multiple distinct elements, where elements that satisfy the <paramref name="predicate"/> are preferred.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="enumerable">The enumerable to choose elements of.</param>
        /// <param name="predicate">The predicate that preferred elements satisfy.</param>
        /// <param name="amount">The amount of elements to choose.</param>
        /// <returns>Randomly chosen elements if any are present, plus additional completely random chosen element otherwise.</returns>
        public static IEnumerable<T> ChooseRandomPreferred<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, int amount)
        {
            //Select preferred elements.
            enumerable = enumerable.ToArray();
            IEnumerable<T> preferredSelection = enumerable.ChooseRandomDistinctWhere(predicate, amount).ToArray();

            //If we have enough, return selection.
            int selectionCount = preferredSelection.Count();
            if (selectionCount == amount)
                return preferredSelection;

            //Select the remaining amount from the rest of the elements.
            int amountLeft = amount - selectionCount;
            IEnumerable<T> unpreferredSelection = enumerable.ChooseRandomDistinctWhere(x => !predicate(x), amountLeft);

            //Concat the two selections.
            return preferredSelection.Concat(unpreferredSelection);
        }

        #endregion

        #region Indices

        /// <summary>
        /// Chooses a random valid index for the <paramref name="enumerable"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to choose an index for.</param>
        /// <returns>A valid index for the enumerable.</returns>
        public static int ChooseRandomIndex<T>(this IEnumerable<T> enumerable)
        {
            int indexMax = enumerable.Count();
            return Random.RandomRange(indexMax);
        }

        /// <summary>
        /// Chooses multiple random indices.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to choose indices for.</param>
        /// <param name="amount">The amount of indices to choose.</param>
        /// <remarks>The same index may be chosen multiple times. If Distinct indices are needed, use <see cref="ChooseRandomIndicesDistinct{T}"/>.</remarks>
        /// <returns>The randomly chosen indices.</returns>
        public static IEnumerable<int> ChooseRandomIndices<T>(this IEnumerable<T> enumerable, int amount)
        {
            int indexMax = enumerable.Count();
            Func<int> generationFunction = () => Random.RandomRange(indexMax);

            return GenerateRandom(generationFunction, amount);
        }

        /// <summary>
        /// Chooses multiple random distinct indices.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to choose indices for.</param>
        /// <param name="amount">The amount of indices to choose.</param>
        /// <returns>Multiple random distinct indices.</returns>
        public static IEnumerable<int> ChooseRandomIndicesDistinct<T>(this IEnumerable<T> enumerable, int amount)
        {
            int indexMax = enumerable.Count();
            IEnumerable<int> indices = MathHelper.GetRange(0, indexMax);

            return indices.ChooseRandomDistinct(amount);
        }

        #endregion

        #endregion

        #region Grabbing

        /// <summary>
        /// Chooses and removes a random element of the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">Type of elements in the list.</typeparam>
        /// <param name="list">The list to choose elements from.</param>
        /// <returns>The randomly grabbed element.</returns>
        public static T GrabRandom<T>(this IList<T> list)
        {
            int index = list.ChooseRandomIndex();
            T element = list.ElementAt(index);
            
            list.RemoveAt(index);
            return element;
        }

        /// <summary>
        /// Randomly chooses and removes multiple elements from the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">Type of elements in the list.</typeparam>
        /// <param name="list">The list to choose elements from.</param>
        /// <param name="amount">Amount of elements to grab.</param>
        /// <returns>The grabbed elements.</returns>
        public static IEnumerable<T> GrabRandom<T>(this IList<T> list, int amount)
        {
            return GenerateRandom(list.GrabRandom, amount);
        }

        #endregion
    }
}
