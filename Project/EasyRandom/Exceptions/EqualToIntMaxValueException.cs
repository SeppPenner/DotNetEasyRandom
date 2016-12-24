using System;

namespace EasyRandom.Exceptions
{
    [Serializable]
    public class EqualToIntMaxValueException : Exception
    {
        public EqualToIntMaxValueException()
        {
        }

        public EqualToIntMaxValueException(string message)
            : base(message)
        {
        }

        public EqualToIntMaxValueException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}