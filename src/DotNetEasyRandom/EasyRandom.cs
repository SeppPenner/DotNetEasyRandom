// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EasyRandom.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The easy random class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DotNetEasyRandom;

/// <inheritdoc cref="IEasyRandom"/>
/// <summary>
/// The easy random class.
/// </summary>
/// <seealso cref="IEasyRandom"/>
public class EasyRandom : IEasyRandom
{
    /// <summary>
    /// The random.
    /// </summary>
    private readonly Random random;

    /// <summary>
    /// Initializes a new instance of the <see cref="EasyRandom"/> class.
    /// </summary>
    public EasyRandom()
    {
        this.random = new Random();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EasyRandom"/> class.
    /// </summary>
    /// <param name="seed">The seed.</param>
    public EasyRandom(int seed)
    {
        this.random = new Random(seed);
    }

    /// <inheritdoc cref="IEasyRandom"/>
    /// <summary>
    /// Returns the next random <see cref="int"/> value.
    /// </summary>
    /// <returns>The next random <see cref="int"/>.</returns>
    /// <seealso cref="IEasyRandom"/>
    public int Next()
    {
        return this.random.Next();
    }

    /// <inheritdoc cref="IEasyRandom"/>
    /// <summary>
    /// Returns the next random <see cref="int"/> value.
    /// </summary>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>The next random <see cref="int"/>.</returns>
    /// <seealso cref="IEasyRandom"/>
    public int Next(int maxValue)
    {
        return this.random.Next(maxValue);
    }

    /// <inheritdoc cref="IEasyRandom"/>
    /// <summary>
    /// Returns the next random <see cref="T:byte[]"/> value.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <seealso cref="IEasyRandom"/>
    public void NextBytes(byte[] buffer)
    {
        this.random.NextBytes(buffer);
    }

    /// <inheritdoc cref="IEasyRandom"/>
    /// <summary>
    /// Returns the next random <see cref="double"/> value.
    /// </summary>
    /// <returns>The next random <see cref="double"/>.</returns>
    /// <seealso cref="IEasyRandom"/>
    public double NextDouble()
    {
        return this.random.NextDouble();
    }

    /// <inheritdoc cref="IEasyRandom"/>
    /// <summary>
    /// Returns the next random <see cref="double"/> value excluding both borders.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>The next <see cref="double"/> value excluding both borders.</returns>
    /// <seealso cref="IEasyRandom"/>
    public double NextDoubleExcludedBoth(double minValue, double maxValue)
    {
        CheckDoubles(minValue, maxValue);
        return (this.NextDoubleWithoutZero() * (maxValue - minValue)) + minValue;
    }

    /// <inheritdoc cref="IEasyRandom"/>
    /// <summary>
    /// Returns the next random <see cref="int"/> value.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <param name="type">The include type.</param>
    /// <returns>The next random <see cref="int"/>.</returns>
    /// <seealso cref="IEasyRandom"/>
    public int Next(int minValue, int maxValue, IncludeType type)
    {
        CheckIntegers(minValue, maxValue);

        return type switch
        {
            IncludeType.None => this.NextWithoutBothBorders(minValue, maxValue),
            IncludeType.StartOnly => this.random.Next(minValue, maxValue),
            IncludeType.EndOnly => this.random.Next(minValue + 1, maxValue + 1),
            IncludeType.Both => this.random.Next(minValue, maxValue + 1),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "The include type is not supported")
        };
    }

    /// <summary>
    /// Returns the next random <see cref="int"/> value between both values, excluding both of them.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>The next random <see cref="int"/>.</returns>
    private int NextWithoutBothBorders(int minValue, int maxValue)
    {
        CheckIfNeighbours(minValue, maxValue);
        return this.random.Next(minValue + 1, maxValue);
    }

    /// <summary>
    /// Returns the next random <see cref="double"/> value greater than zero and less than one.
    /// </summary>
    /// <returns>The next random <see cref="double"/> value without the zero.</returns>
    private double NextDoubleWithoutZero()
    {
        double value;

        do
        {
            value = this.random.NextDouble();
        }
        while (value <= 0.0);

        return value;
    }

    /// <summary>
    /// Checks the <see cref="double"/> input values.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    private static void CheckDoubles(double minValue, double maxValue)
    {
        CheckIfEqual(minValue, maxValue);
        CheckIfBigger(minValue, maxValue);
        CheckIfMaximum(minValue, maxValue);
        CheckIfMinimum(minValue, maxValue);
    }

    /// <summary>
    /// Checks whether both values are equal.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    private static void CheckIfEqual(double minValue, double maxValue)
    {
        if (Math.Abs(minValue - maxValue) < 0.000000000000000000000000000000000000000000001)
        {
            throw new ParametersMustNotBeEqualException($"The parameters {minValue} must not be equal to {maxValue}");
        }
    }

    /// <summary>
    /// Checks whether the minimum is bigger than the maximum value.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    private static void CheckIfBigger(double minValue, double maxValue)
    {
        if (minValue > maxValue)
        {
            throw new MinMustBeGreaterThanMaxException($"The parameter {minValue} must be less than the parameter {maxValue}");
        }
    }

    /// <summary>
    /// Checks whether the values are the maximum of the <see cref="double"/>.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    private static void CheckIfMaximum(double minValue, double maxValue)
    {
        CheckIfMaximum(minValue);
        CheckIfMaximum(maxValue);
    }

    /// <summary>
    /// Checks whether the value is the maximum of the <see cref="double"/>.
    /// </summary>
    /// <param name="toCheck">The value.</param>
    private static void CheckIfMaximum(double toCheck)
    {
        if (Math.Abs(toCheck - double.MaxValue) < 0.000000000000000000000000000000000000000000001)
        {
            throw new EqualToIntMaxValueException($"The parameter {toCheck} must not be equal to the double max value");
        }
    }

    /// <summary>
    /// Checks whether the values are the minimum of the <see cref="double"/>.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    private static void CheckIfMinimum(double minValue, double maxValue)
    {
        CheckIfMinimum(minValue);
        CheckIfMinimum(maxValue);
    }

    /// <summary>
    /// Checks whether the value is the maximum of the <see cref="double"/>.
    /// </summary>
    /// <param name="toCheck">The value.</param>
    private static void CheckIfMinimum(double toCheck)
    {
        if (Math.Abs(toCheck - double.MinValue) < 0.000000000000000000000000000000000000000000001)
        {
            throw new EqualToIntMaxValueException($"The parameter {toCheck} must not be equal to the double min value");
        }
    }

    /// <summary>
    /// Checks the <see cref="int"/> input values.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    private static void CheckIntegers(int minValue, int maxValue)
    {
        CheckIfEqual(minValue, maxValue);
        CheckIfBigger(minValue, maxValue);
        CheckIfMaximum(minValue, maxValue);
    }

    /// <summary>
    /// Checks whether both values are equal.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    private static void CheckIfEqual(int minValue, int maxValue)
    {
        if (minValue == maxValue)
        {
            throw new ParametersMustNotBeEqualException($"The parameters {minValue} must not be equal to {maxValue}");
        }
    }

    /// <summary>
    /// Checks whether the minimum is bigger than the maximum value.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    private static void CheckIfBigger(int minValue, int maxValue)
    {
        if (minValue > maxValue)
        {
            throw new MinMustBeGreaterThanMaxException("The parameter " + minValue + " must be less than the parameter " + maxValue);
        }
    }

    /// <summary>
    /// Checks whether the values are the maximum of the <see cref="int"/>.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    private static void CheckIfMaximum(int minValue, int maxValue)
    {
        CheckIfMaximum(minValue);
        CheckIfMaximum(maxValue);
    }

    /// <summary>
    /// Checks whether the value is the maximum of the <see cref="int"/>.
    /// </summary>
    /// <param name="toCheck">The value.</param>
    private static void CheckIfMaximum(int toCheck)
    {
        if (toCheck == int.MaxValue)
        {
            throw new EqualToIntMaxValueException("The parameter " + toCheck + " must not be equal to the int max value");
        }
    }

    /// <summary>
    /// Checks whether both values are direct neighbours, so that no value is left between them.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    private static void CheckIfNeighbours(int minValue, int maxValue)
    {
        if (minValue + 1 == maxValue)
        {
            throw new NoValueBetweenParametersException("There is no value between the parameter " + minValue + " and the parameter " + maxValue);
        }
    }
}
