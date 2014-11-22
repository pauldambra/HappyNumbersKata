using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        //[Test]
        //public void RunParallelForFiveSeconds()
        //{
        //    var results = new ConcurrentDictionary<int, bool>(4, 5000000);

        //    var cancellationTokenSource = new CancellationTokenSource();
        //    cancellationTokenSource.CancelAfter(5000);

        //    var parallelOptions = new ParallelOptions
        //    {
        //        CancellationToken = cancellationTokenSource.Token
        //    };

        //    var partitioner = Partitioner.Create(0, 5000000, 1000000);
        //    try
        //    {
        //        Parallel.ForEach(partitioner, parallelOptions, range =>
        //        {
        //            var theseNumbers = Enumerable.Range(range.Item1, range.Item2);
        //            var theseResults = theseNumbers.AreHappyNumbers();
        //            var max = theseResults.Keys.Max();
        //            results.TryAdd(max, theseResults[max]);
        //            Debug.WriteLine("Added {0} to results", max);
        //            parallelOptions.CancellationToken.ThrowIfCancellationRequested();
        //        });
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        Debug.WriteLine("The highest number was {0}", results.Keys.Max());
        //    }
        //}

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
