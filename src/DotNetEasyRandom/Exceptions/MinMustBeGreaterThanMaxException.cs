// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MinMustBeGreaterThanMaxException.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The minimum must be greater than the maximum exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DotNetEasyRandom.Exceptions;

/// <inheritdoc cref="Exception"/>
/// <summary>
/// The minimum must be greater than the maximum exception.
/// </summary>
/// <seealso cref="Exception"/>
[Serializable]
public class MinMustBeGreaterThanMaxException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MinMustBeGreaterThanMaxException"/> class.
    /// </summary>
    public MinMustBeGreaterThanMaxException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MinMustBeGreaterThanMaxException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public MinMustBeGreaterThanMaxException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MinMustBeGreaterThanMaxException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="inner">The inner exception.</param>
    public MinMustBeGreaterThanMaxException(string message, Exception inner) : base(message, inner)
    {
    }
}
