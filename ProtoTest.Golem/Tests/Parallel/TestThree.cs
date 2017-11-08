using Golem.WebDriver;
using NUnit.Framework;

namespace Golem.Tests.Parallel
{
    [Parallelizable]
    internal class ParallelTestThree : WebDriverTestBase
    {

        [Test]
        public void TestOne()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
        }

        [Test]
        public void TestTwo()
        {
            driver.Navigate().GoToUrl("http://www.gmail.com");
        }

    }
}