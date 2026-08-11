// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NoValueBetweenParametersException.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The no value between parameters exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DotNetEasyRandom.Exceptions;

/// <inheritdoc cref="Exception"/>
/// <summary>
/// The no value between parameters exception.
/// </summary>
/// <seealso cref="Exception"/>
[Serializable]
public class NoValueBetweenParametersException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NoValueBetweenParametersException"/> class.
    /// </summary>
    public NoValueBetweenParametersException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoValueBetweenParametersException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public NoValueBetweenParametersException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoValueBetweenParametersException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="inner">The inner exception.</param>
    public NoValueBetweenParametersException(string message, Exception inner) : base(message, inner)
    {
    }
}
