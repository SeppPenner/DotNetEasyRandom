namespace EasyRandom
{
    public interface IEasyRandom
    {
        int Next();

        int Next(int maxValue);

        void NextBytes(byte[] buffer);

        double NextDouble();

        double NextDoubleExcludedBoth(double minValue, double maxValue);

        int Next(int minValue, int maxValue, IncludeType type);
    }
}