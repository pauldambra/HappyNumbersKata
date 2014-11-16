using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace HappyNumbers
{
    class CalculationTimer
    {
        [Test]
        public void RunForFiveSeconds()
        {
            var maximums = new List<int>(20);
            for (var i = 0; i < 20; i++)
            {
                maximums.Add(GetMaxReachedInFiveSeconds());
            }
            Debug.WriteLine("In five seconds the highest number checked was {0}", maximums.Average());
        }

        [Test]
        public void ParallelRunForFiveSeconds()
        {
            var maximums = new List<int>(20);

            Parallel.For(0, 200, i =>
            {
                maximums.Add(GetMaxReachedInFiveSeconds()); 
                HappyNumbers.ResetHappyNumberRecords();
            });

            Debug.WriteLine("In five seconds the highest number checked was {0}", maximums.Average());
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
