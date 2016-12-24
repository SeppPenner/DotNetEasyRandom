using System;

namespace EasyRandom.Exceptions
{
    [Serializable]
    public class ParametersMustNotBeEqualException : Exception
    {
        public ParametersMustNotBeEqualException()
        {
        }

        public ParametersMustNotBeEqualException(string message)
            : base(message)
        {
        }

        public ParametersMustNotBeEqualException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}