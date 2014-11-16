using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HappyNumbers
{
    // Choose a two-digit number (eg. 23), square each digit and add them together. 
    // Keep repeating this until you reach 1 or the cycle carries on in a continuous loop. 
    // If you reach 1 then the number you started with is a “happy number”.
    public static class HappyNumbers
    {
        private static readonly Dictionary<int, HashSet<int>> NumberChain = new Dictionary<int, HashSet<int>>(); 
        private static readonly Dictionary<int[], bool> HappyNumberResults = new Dictionary<int[], bool>();

        public static bool IsAHappyNumber(this int startingNumber)
        {
            var digitsToTest = startingNumber.GetDigits().ToArray();

            if (HappyNumberResults.ContainsKey(digitsToTest))
            {
                return HappyNumberResults[digitsToTest];
            }

            if (NumberChain.ContainsKey(startingNumber))
            {
                throw new Exception(
                    string.Format(
                        "I didn't think we could have a number ({0}) without a result that was in the number chain at this point",
                        startingNumber));
            }

            NumberChain.Add(startingNumber, new HashSet<int>{startingNumber});

            TestForHappiness(startingNumber, digitsToTest);

            return HappyNumberResults[digitsToTest];
        }

        private static void TestForHappiness(int startingNumber, int[] happyNumberKey)
        {
            var nextInChain = startingNumber;
            while (!HappyNumberCalculationIsCompleteFor(happyNumberKey))
            {
                nextInChain = nextInChain.GetSumOfSquaredDigits();
                if (nextInChain == 1)
                {
                    HappyNumberResults.Add(happyNumberKey, true);
                    break;
                }
                if (TheCalculationChainHasLoopedAround(startingNumber, nextInChain))
                {
                    HappyNumberResults.Add(happyNumberKey, false);
                    break;
                }
            }
        }

        public static void ResetHappyNumberRecords()
        {
            NumberChain.Clear();
            HappyNumberResults.Clear();
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

        private static bool HappyNumberCalculationIsCompleteFor(int[] happyNumberKey)
        {
            return HappyNumberResults.ContainsKey(happyNumberKey);
        }

        private static int GetSumOfSquaredDigits(this int number)
        {
            return number.GetDigits()
                         .Sum(digit => digit*digit);
        }

        public static IEnumerable<int> GetDigits(this int number)
        {
            return number.ToString(CultureInfo.InvariantCulture)
                         .Select(digit => int.Parse(digit.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
