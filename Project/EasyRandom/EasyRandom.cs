using System;
using EasyRandom.Exceptions;

namespace EasyRandom
{
    public class EasyRandom : IEasyRandom
    {
        private readonly Random _rand;

        public EasyRandom()
        {
            _rand = new Random();
        }

        public EasyRandom(int seed)
        {
            _rand = new Random(seed);
        }

        public int Next()
        {
            return _rand.Next();
        }

        public int Next(int maxValue)
        {
            return _rand.Next(maxValue);
        }

        public void NextBytes(byte[] buffer)
        {
            _rand.NextBytes(buffer);
        }

        public double NextDouble()
        {
            return _rand.NextDouble();
        }

        public double NextDoubleExcludedBoth(double minValue, double maxValue)
        {
            CheckDoubles(minValue, maxValue);
            return _rand.NextDouble() * (maxValue - minValue) + minValue;
        }

        public int Next(int minValue, int maxValue, IncludeType type)
        {
            CheckInts(minValue, maxValue);
            var returnVal = 0;
            switch (type)
            {
                case IncludeType.None:
                    returnVal = _rand.Next(minValue + 1, maxValue);
                    break;
                case IncludeType.StartOnly:
                    returnVal = _rand.Next(minValue, maxValue);
                    break;
                case IncludeType.EndOnly:
                    returnVal = _rand.Next(minValue + 1, maxValue + 1);
                    break;
                case IncludeType.Both:
                    returnVal = _rand.Next(minValue, maxValue + 1);
                    break;
            }
            return returnVal;
        }

        private void CheckDoubles(double minValue, double maxValue)
        {
            CheckIfEqual(minValue, maxValue);
            CheckIfBigger(minValue, maxValue);
            CheckIfMaximum(minValue, maxValue);
            CheckIfMinimum(minValue, maxValue);
        }

        private void CheckIfEqual(double minValue, double maxValue)
        {
            if (Math.Abs(minValue - maxValue) < 0.000000000000000000000000000000000000000000001)
                throw new ParametersMustNotBeEqualException(
                    "The parameters " + minValue + " must not be equal to " + maxValue,
                    new Exception("The parameters " + minValue + " must not be equal to " + maxValue));
        }

        private void CheckIfBigger(double minValue, double maxValue)
        {
            if (minValue > maxValue)
                throw new MinMustBeGreaterThanMaxException(
                    "The parameter " + minValue + " must be greater than the parameter " + maxValue,
                    new Exception("The parameter " + minValue + " must be greater than the parameter " + maxValue));
        }

        private void CheckIfMaximum(double minValue, double maxValue)
        {
            CheckIfMaximum(minValue);
            CheckIfMaximum(maxValue);
        }

        private void CheckIfMaximum(double toCheck)
        {
            if (Math.Abs(toCheck - double.MaxValue) < 0.000000000000000000000000000000000000000000001)
                throw new EqualToIntMaxValueException(
                    "The parameter " + toCheck + " must not be equal to the double max value",
                    new Exception("The parameter " + toCheck + " must not be equal to the double max value"));
        }

        private void CheckIfMinimum(double minValue, double maxValue)
        {
            CheckIfMinimum(minValue);
            CheckIfMinimum(maxValue);
        }

        private void CheckIfMinimum(double toCheck)
        {
            if (Math.Abs(toCheck - double.MinValue) < 0.000000000000000000000000000000000000000000001)
                throw new EqualToIntMaxValueException(
                    "The parameter " + toCheck + " must not be equal to the double min value",
                    new Exception("The parameter " + toCheck + " must not be equal to the double min value"));
        }

        private void CheckInts(int minValue, int maxValue)
        {
            CheckIfEqual(minValue, maxValue);
            CheckIfBigger(minValue, maxValue);
            CheckIfMaximum(minValue, maxValue);
        }

        private void CheckIfEqual(int minValue, int maxValue)
        {
            if (minValue == maxValue)
                throw new ParametersMustNotBeEqualException(
                    "The parameters " + minValue + " must not be equal to " + maxValue,
                    new Exception("The parameters " + minValue + " must not be equal to " + maxValue));
        }

        private void CheckIfBigger(int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new MinMustBeGreaterThanMaxException(
                    "The parameter " + minValue + " must be greater than the parameter " + maxValue,
                    new Exception("The parameter " + minValue + " must be greater than the parameter " + maxValue));
        }

        private void CheckIfMaximum(int minValue, int maxValue)
        {
            CheckIfMaximum(minValue);
            CheckIfMaximum(maxValue);
        }

        private void CheckIfMaximum(int toCheck)
        {
            if (toCheck == int.MaxValue)
                throw new EqualToIntMaxValueException(
                    "The parameter " + toCheck + " must not be equal to the int max value",
                    new Exception("The parameter " + toCheck + " must not be equal to the int max value"));
        }
    }
}