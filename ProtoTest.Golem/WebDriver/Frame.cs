﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Golem.WebDriver
{
    public class Frame : Element
    {
        public Frame(By by) : base(by)
        {
        }
    }
}
