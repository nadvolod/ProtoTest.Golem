﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using Golem.Core;

namespace Golem.WebDriver
{
    public class EventedWebDriver
    {
        private const string errorMessage = "{0}: {1} ({2}) {3}";
        public EventFiringWebDriver driver;

        public EventedWebDriver(IWebDriver driver)
        {
            this.driver = new EventFiringWebDriver(driver);
            RegisterEvents();
        }

        public IWebDriver RegisterEvents()
        {
            driver.ElementClicking += driver_ElementClicking;
            driver.ElementClicked += driver_ElementClicked;
            driver.ExceptionThrown += driver_ExceptionThrown;
            driver.FindingElement += driver_FindingElement;
            driver.Navigating += driver_Navigating;
            driver.Navigated += driver_Navigated;
            driver.ElementValueChanged += driver_ElementValueChanged;
            driver.ElementValueChanging += driver_ElementValueChanging;
            driver.FindElementCompleted += driver_FindElementCompleted;
            driver.FoundElement += driver_FoundElement;
            return driver;
        }

        private void driver_ElementClicked(object sender, WebElementEventArgs e)
        {
            
            Common.Delay(Config.settings.runTimeSettings.CommandDelayMs);
        }

        private void driver_Navigated(object sender, WebDriverNavigationEventArgs e)
        {
            Common.Delay(Config.settings.runTimeSettings.CommandDelayMs);
            driver.WaitForJQuery();

        }

        private void driver_FoundElement(object sender, FoundElementEventArgs e)
        {
            if (Config.settings.runTimeSettings.HighlightFoundElements)
            {
                e.Element.Highlight();
            }
        }

        private void driver_FindElementCompleted(object sender, FindElementEventArgs e)
        {
//            TestContext.CurrentContext.IncrementAssertCount();
        }

        private void driver_ElementValueChanging(object sender, WebElementEventArgs e)
        {
            try
            {
                driver.WaitForJQuery();
                e.Element.Highlight(30, "red");
            }
            catch (Exception)
            {
            }
        }

        private void driver_ElementValueChanged(object sender, WebElementEventArgs e)
        {
            try
            {

                if (Config.settings.reportSettings.commandLogging)
                {
                    Log.Message(GetLogMessage("Typing", e, e.Element.GetAttribute("value")));
                }
                Common.Delay(Config.settings.runTimeSettings.CommandDelayMs);
            }
            catch (Exception)
            {
            }
        }

        private void driver_Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            if (Config.settings.reportSettings.commandLogging)
            {
                Log.Message(string.Format("Navigating to url {0}", e.Url));
            }
            driver.WaitForJQuery();
           
        }

        private void driver_ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
        }

        private void driver_FindingElement(object sender, FindElementEventArgs e)
        {
            driver.WaitForJQuery();
            Common.Delay(Config.settings.runTimeSettings.CommandDelayMs);
            if (Config.settings.reportSettings.commandLogging)
            {
                Log.Message(GetLogMessage("Finding", e));
            }
        }

        private void driver_ElementClicking(object sender, WebElementEventArgs e)
        {
            if (Config.settings.reportSettings.commandLogging)
            {
                Log.Message(GetLogMessage("Click", e));
            }
            
            if (e.Element == null)
            {
                throw new NoSuchElementException(string.Format("Element '{0}' not present, cannot click on it",
                    e.Element));
            }

            driver.WaitForJQuery();
            e.Element.Highlight(30, "red");
        }

        private string GetLogMessage(string command, WebElementEventArgs e = null, string param = "")
        {
            if (param != "") param = "'" + param + "'";
            return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), command, e.Element.GetHtml(), param);
        }

        private string GetLogMessage(string command, FindElementEventArgs e = null, string param = "")
        {
            if (param != "") param = "'" + param + "'";
            return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), command, e.FindMethod,
                param);
        }
    }
}