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
        private static readonly Dictionary<string, bool> HappyNumberResults = new Dictionary<string, bool>();

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
            NumberChain.Add(startingNumber, new HashSet<int>{startingNumber});
            TestForHappiness(startingNumber, happyNumberKey);
            return HappyNumberResults[happyNumberKey];
        }

        private static void GuardAgainstStrangeness(int startingNumber)
        {
            if (NumberChain.ContainsKey(startingNumber))
            {
                throw new Exception(
                    string.Format(
                        "I didn't think we could have a number ({0}) without a result that was in the number chain at this point",
                        startingNumber));
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
