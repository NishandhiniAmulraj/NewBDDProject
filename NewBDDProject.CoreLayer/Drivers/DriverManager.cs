using NewBDDProject.CoreLayer.Drivers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBDDProject.CoreLayer.Drivers
{
    public sealed class DriverManager
    {
        private static readonly Lazy<IWebDriver> lazyDriver =
                   new Lazy<IWebDriver>(() => WebDriverFactory.CreateDriver(), isThreadSafe: true);
        private DriverManager()
        {
        }
        public static IWebDriver Instance => lazyDriver.Value;
        public static IJavaScriptExecutor Js => (IJavaScriptExecutor)Instance;
        public static void Quit()
        {
            if (lazyDriver.IsValueCreated) lazyDriver.Value.Quit();
        }
    }
}
