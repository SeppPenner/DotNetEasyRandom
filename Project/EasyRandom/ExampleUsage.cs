namespace EasyRandom
{
    public class ExampleUsage
    {
        private readonly IEasyRandom _random = new EasyRandom(); //Normal construtor

        private readonly IEasyRandom _random2 = new EasyRandom(12); //Construtor with seed

        public void Test()
        {
            _random.Next(); //Function from the "normal" Random class
            _random.Next(12); //Function from the "normal" Random class
            _random.Next(1, 5, IncludeType.None); //Gives back 2, 3 or 4
            _random.Next(1, 5, IncludeType.StartOnly); //Gives back 1, 2, 3 or 4
            _random.Next(1, 5, IncludeType.EndOnly); //Gives back 2, 3, 4 or 5
            _random.Next(1, 5, IncludeType.Both); //Gives back 1, 2, 3, 4 or 5
            _random.NextBytes(new byte[] {1, 2, 3, 4}); //Function from the "normal" Random class
            _random.NextDouble(); //Function from the "normal" Random class
            _random.NextDoubleExcludedBoth(1.0, 2.0);
            //Gives back a double value between 1.0 and 2.0 EXCLUDING the borders
            //(from 1.0000000000000000000000000000000001 to 1.9999999999999999999999999999999991
        }
    }
}