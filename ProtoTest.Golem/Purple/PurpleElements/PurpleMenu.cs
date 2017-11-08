﻿using System.Linq;
using System.Threading;
using System.Windows.Automation;

namespace Golem.Purple.PurpleElements
{
    public class PurpleMenu : PurpleElementBase
    {
        private string _pathAfterMenu;
        private string[] _pathtoMenuSelection;

        public PurpleMenu(string name, string locatorPath, string targetPath) : base(name, locatorPath)
        {
            _pathAfterMenu = targetPath;
        }

        public new void Click()
        {
            //This is not used since we can invoke menu directly
            var menuNums = _pathtoMenuSelection.Count();
            AutomationElement menu = null;
            for (var x = 0; x < menuNums - 1; x++)
            {
                menu = PurpleElement.FindFirst(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.NameProperty, _pathtoMenuSelection[x]));
                ((ExpandCollapsePattern) menu.GetCurrentPattern(ExpandCollapsePattern.Pattern)).Expand();
                Thread.Sleep(50);
            }
            if (menu != null)
            {
                var MenuItem = menu.FindFirst(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.NameProperty, _pathtoMenuSelection[menuNums - 1]));
                ((InvokePattern) MenuItem.GetCurrentPattern(InvokePattern.Pattern)).Invoke();
            }
        }
    }
}