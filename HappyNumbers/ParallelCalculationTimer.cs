using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace HappyNumbers
{
    class ParallelCalculationTimer
    {
        [Test]
        public void ParallelRunForFiveSeconds()
        {
            var watch = Stopwatch.StartNew();
            var cancellationTokenSource = new CancellationTokenSource();
            var tasks = new List<Task<Dictionary<int, bool>>>();
            var count = 0;

            //without this line the whole thing runs to completion
            cancellationTokenSource.CancelAfter(5000);

            try
            {
                foreach (var index in Enumerable.Range(0, 10))
                {
                    var cancellationToken = cancellationTokenSource.Token;
                    tasks.Add(Task.Factory.StartNew(
                        () => Enumerable.Range(index * 1000000, 1000000)
                                        .AreHappyNumbers(cancellationToken)));
                }

                //without timeout the stopwatch measures around 6.5 seconds
               Task.WaitAll(tasks.Cast<Task>().ToArray(), timeout: TimeSpan.FromSeconds(5));
               count = tasks.Select(t => t.Result).Sum(result => result.Count);
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("task was cancelled and that's ok");
            }

            Debug.WriteLine("watch has been running for {0} seconds", watch.Elapsed.TotalSeconds);

            Debug.WriteLine("In five seconds the number of numbers was {0}", count);
        }
    }
}
