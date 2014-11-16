using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace HappyNumbers
{
    [TestFixture]
    internal class HappyNumberTests
    {
        [Test]
        [TestCase(31, true)]
        [TestCase(4, false)]
        [TestCase(2, false)]
        public void CanIdentifyHappyNumber(int i, bool expected)
        {
            Assert.AreEqual(expected, i.IsAHappyNumber());
        }

        // Taken from http://oeis.org/A007770
        private static readonly int[] HappyNumbersUpTo1000 =
        {
            1, 7, 10, 13, 19, 23, 28, 31, 32, 44, 49, 68, 70, 79, 82, 86, 91, 94, 97, 100, 103, 109, 129, 130, 133, 139,
            167, 176, 188, 190, 192, 193, 203, 208, 219, 226, 230, 236, 239, 262, 263, 280, 291, 293, 301, 302, 310, 313,
            319, 320, 326, 329, 331, 338, 356, 362, 365, 367, 368, 376, 379, 383, 386, 391, 392, 397, 404, 409, 440, 446,
            464, 469, 478, 487, 490, 496, 536, 556, 563, 565, 566, 608, 617, 622, 623, 632, 635, 637, 638, 644, 649, 653,
            655, 656, 665, 671, 673, 680, 683, 694, 700, 709, 716, 736, 739, 748, 761, 763, 784, 790, 793, 802, 806, 818,
            820, 833, 836, 847, 860, 863, 874, 881, 888, 899, 901, 904, 907, 910, 912, 913, 921, 923, 931, 932, 937, 940,
            946, 964, 970, 973, 989, 998, 1000
        };

        [Test]
        public void CanTestAThousandNumbers()
        {
            for (var i = 0; i < 1000; i++)
            {
                Assert.AreEqual(HappyNumbersUpTo1000.Contains(i), i.IsAHappyNumber());
            }
        }
    }
}