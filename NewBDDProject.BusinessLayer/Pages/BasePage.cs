using NewBDDProject.CoreLayer.Drivers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBDDProject.BusinessLayer.Pages
{
    public abstract class BasePage
    {
        // Always get the current driver instance
        protected IWebDriver Driver => DriverManager.Instance;

        protected IJavaScriptExecutor JsExecutor =>
            (IJavaScriptExecutor)Driver;

        /// <summary>
        /// Navigates to the page's URL
        /// </summary>
        public abstract void Navigate();

        /// <summary>
        /// Verifies the page's unique identifier (URL fragment, element, title, etc.)
        /// </summary>
        public abstract bool IsAt();
    }
}
