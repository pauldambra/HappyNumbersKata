using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace HappyNumbers
{
    // Choose a two-digit number (eg. 23), square each digit and add them together. 
    // Keep repeating this until you reach 1 or the cycle carries on in a continuous loop. 
    // If you reach 1 then the number you started with is a “happy number”.
    public static class HappyNumbers
    {
        private static readonly ConcurrentDictionary<int, HashSet<int>> NumberChain = new ConcurrentDictionary<int, HashSet<int>>();
        private static readonly ConcurrentDictionary<string, bool> HappyNumberResults = new ConcurrentDictionary<string, bool>();

        public static Dictionary<int, bool> AreHappyNumbers(this IEnumerable<int> numbersToProcess, CancellationToken cancellationToken)
        {
            var numbers = numbersToProcess as int[] ?? numbersToProcess.ToArray();
            var results = new Dictionary<int, bool>(numbers.Count());
            foreach (var number in numbers)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.WriteLine("returning before processing {0} on cancel", number);
                    return results;
                }
                results.Add(number, number.IsAHappyNumber());
            }
            return results;
        } 

        public static bool IsAHappyNumber(this int startingNumber)
        {
            var digitsToTest = startingNumber.GetDigits()
                                             .StripZeroes()
                                             .OrderedByDigit();

            var happyNumberKey = string.Join(",", digitsToTest);

            if (AlreadyKnowThatThisNumberIsHappy(happyNumberKey))
            {
                return HappyNumberResults[happyNumberKey];
            }

            GuardAgainstStrangeness(startingNumber);
            NumberChain.TryAdd(startingNumber, new HashSet<int> { startingNumber });
            TestForHappiness(startingNumber, happyNumberKey);
            return HappyNumberResults[happyNumberKey];
        }

        private static void GuardAgainstStrangeness(int startingNumber)
        {
            if (NumberChain.ContainsKey(startingNumber))
            {
                Debug.WriteLine("this number {0} has been processed but doesn't have a result?!", startingNumber);
            }
        }

        private static bool AlreadyKnowThatThisNumberIsHappy(string happyNumberKey)
        {
            return HappyNumberResults.ContainsKey(happyNumberKey);
        }

        private static void TestForHappiness(int startingNumber, string happyNumberKey)
        {
            var nextInChain = startingNumber;
            while (!HappyNumberCalculationIsCompleteFor(happyNumberKey))
            {
                nextInChain = nextInChain.GetSumOfSquaredDigits();
                if (nextInChain == 1)
                {
                    HappyNumberResults.TryAdd(happyNumberKey, true);
                    break;
                }
                if (TheCalculationChainHasLoopedAround(startingNumber, nextInChain))
                {
                    HappyNumberResults.TryAdd(happyNumberKey, false);
                    break;
                }
            }
        }

        /// <summary>
        /// If the last calculated sum of the squares of the digits is already in the set of calculated numbers
        /// then this number chain has looped around
        /// </summary>
        private static bool TheCalculationChainHasLoopedAround(int startingNumber, int sumOfSquaredDigits)
        {
            var canAddToTheNumbersInThisChain = NumberChain[startingNumber].Add(sumOfSquaredDigits);
            return !canAddToTheNumbersInThisChain;
        }

        private static bool HappyNumberCalculationIsCompleteFor(string happyNumberKey)
        {
            return HappyNumberResults.ContainsKey(happyNumberKey);
        }

        private static int GetSumOfSquaredDigits(this int number)
        {
            return number.GetDigits()
                         .Sum(digit => digit*digit);
        }

        private static IEnumerable<int> GetDigits(this int number)
        {
            return number.ToString(CultureInfo.InvariantCulture)
                         .Select(digit => int.Parse(digit.ToString(CultureInfo.InvariantCulture)));
        }

        private static IEnumerable<int> StripZeroes(this IEnumerable<int> numbers)
        {
            return numbers.Where(digit => digit != 0);
        }

        private static IEnumerable<int> OrderedByDigit(this IEnumerable<int> numbers)
        {
            return numbers.OrderBy(i=>i);
        }
    }
}
