using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBDDProject.CoreLayer.UI
{
    public interface IActionWrapper
    {
        void Click(By locator);
        void Type(By locator, string text);
        string GetText(By locator);
    }
}
