// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParametersMustNotBeEqualException.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The parameters must not be equal exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DotNetEasyRandom.Exceptions;

/// <inheritdoc cref="Exception"/>
/// <summary>
/// The parameters must not be equal exception.
/// </summary>
/// <seealso cref="Exception"/>
[Serializable]
public class ParametersMustNotBeEqualException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParametersMustNotBeEqualException"/> class.
    /// </summary>
    public ParametersMustNotBeEqualException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParametersMustNotBeEqualException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public ParametersMustNotBeEqualException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParametersMustNotBeEqualException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="inner">The inner exception.</param>
    public ParametersMustNotBeEqualException(string message, Exception inner) : base(message, inner)
    {
    }
}
