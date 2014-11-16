using System.Diagnostics;
using NUnit.Framework;

namespace HappyNumbers
{
    class CalculationTimer
    {
        [Test]
        public void RunForFiveSeconds()
        {
            Debug.WriteLine("In five seconds the highest number checked was {0}", GetMaxReachedInFiveSeconds());
        }

        private static int GetMaxReachedInFiveSeconds()
        {
            var stopwatch = Stopwatch.StartNew();
            var numberReached = 0;
            while (stopwatch.Elapsed.Seconds < 5)
            {
                numberReached++.IsAHappyNumber();
            }
            return numberReached;
        }
    }
}
