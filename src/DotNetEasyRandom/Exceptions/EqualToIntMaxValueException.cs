// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EqualToIntMaxValueException.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The equal to int max value exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DotNetEasyRandom.Exceptions
{
    using System;

    /// <inheritdoc cref="Exception"/>
    /// <summary>
    /// The equal to int max value exception.
    /// </summary>
    /// <seealso cref="Exception"/>
    [Serializable]
    public class EqualToIntMaxValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualToIntMaxValueException"/> class.
        /// </summary>
        public EqualToIntMaxValueException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualToIntMaxValueException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public EqualToIntMaxValueException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualToIntMaxValueException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public EqualToIntMaxValueException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}