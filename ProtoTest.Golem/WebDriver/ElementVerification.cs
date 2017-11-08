﻿using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using Golem.Core;

namespace Golem.WebDriver
{
    /// <summary>
    ///     Methods for performing non-terminating validations, and Wait commands.
    /// </summary>
    public class ElementVerification
    {
        private const string errorMessage = "{0}: {1}({2}): {3} after {4} seconds";
        private readonly Element element;
        private readonly bool failTest;
        private readonly bool isTrue = true;
        private bool condition;
        private string message;
        private string notMessage;
        private int timeoutSec;

        public ElementVerification(Element element, int timeoutSec, bool failTest = false, bool isTrue = true)
        {
            this.element = element;
            this.timeoutSec = timeoutSec;
            this.failTest = failTest;
            this.isTrue = isTrue;
        }

        public ElementVerification Not()
        {
            element.timeoutSec = 1;
            if (element.root != null)
            {
                element.root.timeoutSec = 1;
            }
            if (element.frame != null)
            {
                element.frame.timeoutSec = 1;
            }
            return new ElementVerification(element, timeoutSec, failTest, false);
        }

        public ElementVerification Not(int timeoutSec)
        {
            element.timeoutSec = 1;
            if (element.root != null)
            {
                element.root.timeoutSec = 1;
            }
            if (element.frame != null)
            {
                element.frame.timeoutSec = 1;
            }
            this.timeoutSec = timeoutSec;
            return new ElementVerification(element, timeoutSec, failTest, false);
        }

        private void VerificationFailed(string message = "", Image image = null)
        {
            if (message == "") message = GetErrorMessage();
            if (failTest)
            {
                Assert.Fail(message);
            }
            else
            {
                if (image == null)
                {
                    TestBase.AddVerificationError(message);
                }
                else
                {
                    TestBase.AddVerificationError(message, image);
                }
            }
        }

        private string GetErrorMessage()
        {
            string newMessage;
            newMessage = isTrue ? notMessage : message;
            return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), element.name, element.by,
                newMessage, timeoutSec);
        }

        private string GetSuccessMessage()
        {
            string newMessage;
            var correctMessage = "{0}: {1}({2}): {3}";
            newMessage = isTrue ? message : notMessage;
            return string.Format(correctMessage, TestBase.GetCurrentClassAndMethodName(), element.name, element.by,
                newMessage);
        }

        public Element ChildElement(By bylocator)
        {
            message = "is found";
            notMessage = "not found";
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                condition = element.FindElements(bylocator).Count > 0;
                if (condition == isTrue)
                {
                    Log.Message("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }

        public Element Present()
        {
            message = "is present";
            notMessage = "not present";

            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                condition = element.Present;
                if (condition == isTrue)
                {
                    Log.Message("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }

        public Element Visible()
        {
            message = "is visible";
            notMessage = "not visible";
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                condition = element.Displayed;
                if (condition == isTrue)
                {
                    Log.Message("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }

        public Element Count(int value)
        {
            message = "count not '" + value + "'";
            notMessage = "count was not '" + value + "'";
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var newText = element.Text;
                condition = (element.Present) && (element.Count() == value);
                if (condition == isTrue)
                {
                    Log.Message("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            notMessage += ". It was : '" + element.Count() + "'";
            VerificationFailed();
            return element;
        }

        public Element CountGreaterThan(int value)
        {
            message = "count not greater than '" + value + "'";
            notMessage = "count was not less than '" + value + "'";
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var newText = element.Text;
                condition = (element.Present) && (element.Count() > value);
                if (condition == isTrue)
                {
                    Log.Message("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            notMessage += ". It was : '" + element.Count() + "'";
            VerificationFailed();
            return element;
        }

        public Element CountLessThan(int value)
        {
            message = "count not less than '" + value + "'";
            notMessage = "count was not greater than '" + value + "'";
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var newText = element.Text;
                condition = (element.Present) && (element.Count() < value);
                if (condition == isTrue)
                {
                    Log.Message("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            notMessage += ". It was : '" + element.Count() + "'";
            VerificationFailed();
            return element;
        }

        public Element Text(string text)
        {
            message = "contains text '" + text + "'";
            notMessage = "doesn't contain text '" + text + "'";
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var newText = element.Text;
                condition = (element.Present) && (element.Text.Contains(text));
                if (condition == isTrue)
                {
                    Log.Message("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            notMessage += ". Actual : " + element.Text;
            VerificationFailed();
            return element;
        }

        public Element Value(string text)
        {
            message = "has value " + text;
            notMessage = "doesn't have value " + text;
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                condition = (element.Present) && (element.GetAttribute("value").Contains(text));
                if (condition == isTrue)
                {
                    Log.Message("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            notMessage += ". Actual : " + element.GetAttribute("value");
            VerificationFailed();
            return element;
        }

        public Element Attribute(string attribute, string value)
        {
            message = "has attribute " + attribute + " with value " + value;
            notMessage = "doesn't have attribute " + attribute + " with value " + value;
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                condition = (element.Present) && (element.GetAttribute(attribute).Contains(value));
                if (condition == isTrue)
                {
                    Log.Message("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            notMessage += ". Actual : " + element.GetAttribute(attribute);
            VerificationFailed();
            return element;
        }

        public Element CSS(string attribute, string value)
        {
            message = "has css " + attribute + " with value " + value;
            notMessage = "doesn't have css " + attribute + " with value " + value;
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                condition = (element.Present) && (element.GetAttribute(attribute).Contains(value));
                if (condition == isTrue)
                {
                    Log.Message("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            notMessage += ". Actual : " + element.GetCssValue(attribute);
            VerificationFailed();
            return element;
        }

        public Element Selected()
        {
            message = "is selected";
            notMessage = "isn't selected";
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                condition = (element.Present) && (element.Selected);
                if (condition == isTrue)
                {
                    Log.Message("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }

        public Element Image()
        {
            message = "image matches";
            notMessage = "image is {0} different";
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                condition = (element.Present) && (element.Images.ImagesMatch());
                if (condition == isTrue)
                {
//                    TestContext.CurrentContext.IncrementAssertCount();
                    Log.Message(GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }

            notMessage = string.Format(notMessage, element.Images.differenceString);
            VerificationFailed(
                string.Format("{0}: {1}({2}): {3}", TestBase.GetCurrentClassAndMethodName(), element.name,
                    element.by,
                    notMessage), element.Images.GetMergedImage());

            return element;
        }
    }
}