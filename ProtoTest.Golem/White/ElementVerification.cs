﻿using System.Linq;
using Gallio.Framework;
using MbUnit.Framework;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.White.Elements;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WPFUIItems;
using Image = System.Drawing.Image;

namespace ProtoTest.Golem.White
{
    /// <summary>
    /// Methods for performing non-terminating validations, and Wait commands.
    /// </summary>
    public class ElementVerification
    {
        private const string errorMessage = "{0}: {1}({2}): {3} after {4} seconds";
        private readonly IWhiteElement element;
        private readonly bool failTest;
        private readonly bool isTrue = true;
        private bool condition;
        private string message;
        private string notMessage;
        private int timeoutSec;

        public ElementVerification(IWhiteElement element, int timeoutSec = 0, bool failTest = false, bool isTrue = true)
        {
            this.element = element;
            this.timeoutSec = timeoutSec;
            this.failTest = failTest;
            this.isTrue = isTrue;
        }

        public ElementVerification Not()
        {
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
                    TestBase.AddVerificationError(message);
                else
                    TestBase.AddVerificationError(message, image);
            }
        }


        private string GetErrorMessage()
        {
            string newMessage;
            newMessage = isTrue ? notMessage : message;

            return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), element.description, element.criteria,
                newMessage, timeoutSec);
        }

        private string GetSuccessMessage()
        {
            string newMessage;
            string correctMessage = "{0}: {1}({2}): {3}";
            newMessage = isTrue ? message : notMessage;

            return string.Format(correctMessage, TestBase.GetCurrentClassAndMethodName(), element.description, element.criteria,
                newMessage);
        }


        public IWhiteElement HasChildElement(SearchCriteria criteria)
        {
            message = "has child with criteria " + criteria;
            notMessage = "no child with criteria " + criteria;

            for (int i = 0; i <= timeoutSec; i++)
            {
                condition = element.getItem().Present() && element.getItem().GetMultiple(criteria).Length > 0;
                if (condition == isTrue)
                {
                    TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }


        public IWhiteElement Present()
        {
            message = "is present";
            notMessage = "not present";
            
            for (int i = 0; i <= timeoutSec; i++)
            {
                condition = element.getItem().Present();
                if (condition == isTrue)
                {
                    TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }

        public IWhiteElement Visible()
        {
            message = "is visible";
            notMessage = "not visible";
            if (timeoutSec == 0) timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
            for (int i = 0; i <= timeoutSec; i++)
            {
                condition = element.getItem().Present() && element.getItem().Visible;
                if (condition == isTrue)
                {
                    TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }

        public IWhiteElement Name(string value)
        {
            message = "contains text '" + value + "'";
            notMessage = "doesn't contain text '" + value + "'";
            for (int i = 0; i <= timeoutSec; i++)
            {

                condition = element.getItem().Present() && (element.getItem().Name.Contains(value));
                if (condition == isTrue)
                {
                    TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }

        public IWhiteElement Image()
        {
            message = "image matches";
            notMessage = "image is {0} different";
            var comparer = new ElementImageComparer(element);
            condition = element.getItem().Present() && comparer.ImagesMatch();
            if (condition == isTrue)
            {
                TestContext.CurrentContext.IncrementAssertCount();
                TestBase.LogEvent(GetSuccessMessage());
            }

            else
            {
                notMessage = string.Format(notMessage, comparer.differenceString);
                VerificationFailed(
                    string.Format("{0}: {1}({2}): {3}", TestBase.GetCurrentClassAndMethodName(), element.description,
                        element.criteria,
                        notMessage), comparer.GetMergedImage());
            }

            return element;
        }
    }
}