// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEasyRandom.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The easy random interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DotNetEasyRandom;

/// <summary>
/// The easy random interface.
/// </summary>
public interface IEasyRandom
{
    /// <summary>
    /// Returns the next random <see cref="int"/> value.
    /// </summary>
    /// <returns>The next random <see cref="int"/>.</returns>
    int Next();

    /// <summary>
    /// Returns the next random <see cref="int"/> value.
    /// </summary>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>The next random <see cref="int"/>.</returns>
    int Next(int maxValue);

    /// <summary>
    /// Returns the next random <see cref="T:byte[]"/> value.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    void NextBytes(byte[] buffer);

    /// <summary>
    /// Returns the next random <see cref="double"/> value.
    /// </summary>
    /// <returns>The next random <see cref="double"/>.</returns>
    double NextDouble();

    /// <summary>
    /// Returns the next random <see cref="double"/> value excluding both borders.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>The next <see cref="double"/> value excluding both borders.</returns>
    double NextDoubleExcludedBoth(double minValue, double maxValue);

    /// <summary>
    /// Returns the next random <see cref="int"/> value.
    /// </summary>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <param name="type">The include type.</param>
    /// <returns>The next random <see cref="int"/>.</returns>
    int Next(int minValue, int maxValue, IncludeType type);
}
