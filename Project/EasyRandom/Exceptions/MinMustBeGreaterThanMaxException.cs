using System;

namespace EasyRandom.Exceptions
{
    [Serializable]
    public class MinMustBeGreaterThanMaxException : Exception
    {
        public MinMustBeGreaterThanMaxException()
        {
        }

        public MinMustBeGreaterThanMaxException(string message)
            : base(message)
        {
        }

        public MinMustBeGreaterThanMaxException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}