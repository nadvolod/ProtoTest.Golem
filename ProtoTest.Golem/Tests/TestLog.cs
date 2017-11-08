using NUnit.Framework;
using Golem.Core;
using Golem.WebDriver;

namespace Golem.Tests
{
    internal class TestLog : WebDriverTestBase
    {
        [Test]
        public void TestLogFile()
        {
            Log.Message("This is a test");
        }

        [Test]
        public void TestLogImage()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
            
            Log.Image(driver.GetScreenshot());
        }


        [Test]
        public void TestLogVideo()
        {
            driver.Navigate().GoToUrl("http://www.google.com");

            Log.Video(testData.recorder.Video);
        }
    }
}