using Hsiaye.Extensions;
using NUnit.Framework;

namespace Hsiaye.NUnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            HexCalc calc = new HexCalc(3);
            string res = calc.Add("2", "1");
        }
    }
}