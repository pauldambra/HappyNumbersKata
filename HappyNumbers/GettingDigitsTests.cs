using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace HappyNumbers
{
    class GettingDigitsTests
    {
        [Test]
        public void CanGetDigits()
        {
            Assert.AreEqual(new []{1,2,3,4,5,6,7,8,9}, 123456789.GetDigits());
        }
    }
}
