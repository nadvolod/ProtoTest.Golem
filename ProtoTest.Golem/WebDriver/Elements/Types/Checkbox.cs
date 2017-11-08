﻿using OpenQA.Selenium;

namespace Golem.WebDriver.Elements.Types
{
    public class Checkbox : Element
    {
        public Checkbox(By bylocator) : base(bylocator)
        {
            @by = bylocator;
        }

        public Checkbox(string name, By bylocator) : base(name, bylocator)
        {
        }

        public new Checkbox SetCheckbox(bool isChecked)
        {
            if (element.Selected != isChecked)
            {
                element.Click();
            }
            return this;
        }
    }
}