using NewBDDProject.CoreLayer.Screenshot;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NewBDDProject.CoreLayer.UI
{
    public class ActionWrapper : IActionWrapper
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private const int MaxRetries = 3;

        public ActionWrapper(IWebDriver driver, TimeSpan timeout)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, timeout);
                
        }
        public void Click(By locator)
        {
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    var element = _wait.Until(ExpectedConditions.ElementToBeClickable(locator));
                    ScrollIntoView(element);
                    element.Click();
                    return;
                }
                catch (ElementClickInterceptedException)
                {
                    Console.WriteLine($"Attempt {attempt}: click intercepted → retrying JS click");
                    Thread.Sleep(500);
                    TryClickWithJs(locator);
                }
                catch (StaleElementReferenceException)
                {
                    Console.WriteLine($"Attempt {attempt}: stale element → retry");
                    Thread.Sleep(500);
                }
            }
            CaptureAndFail("ClickError", locator);
        }

        public void Type(By locator, string text)
        {
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    var el = _wait.Until(ExpectedConditions.ElementIsVisible(locator));
                    el.Clear();
                    el.SendKeys(text);
                    return;
                }
                catch (StaleElementReferenceException)
                {
                    Console.WriteLine($"Attempt {attempt}: stale typing → retry");
                    Thread.Sleep(500);
                }
            }
            CaptureAndFail("TypeError", locator);
        }

        public string GetText(By locator)
        {
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    var el = _wait.Until(ExpectedConditions.ElementIsVisible(locator));
                    return el.Text;
                }
                catch (StaleElementReferenceException)
                {
                    Console.WriteLine($"Attempt {attempt}: stale text → retry");
                    Thread.Sleep(500);
                }
            }
            CaptureAndFail("GetTextError", locator);
            return string.Empty;
        }

        /// <summary>
        /// Hover over an element before further actions.
        /// </summary>
        public void Hover(By locator)
        {
            var element = _wait.Until(ExpectedConditions.ElementIsVisible(locator));
            ScrollIntoView(element);
            new Actions(_driver).MoveToElement(element).Perform();
            Console.WriteLine($"Hovered over element: {locator}");
        }

        private void TryClickWithJs(By locator)
        {
            var el = _driver.FindElement(locator);
            ScrollIntoView(el);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", el);
        }

        private void ScrollIntoView(IWebElement el)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", el);
        }

        private void CaptureAndFail(string errorPrefix, By locator)
        {
            var path = ScreenshotHelper.Capture(_driver, $"{errorPrefix}_{locator}");
            Assert.Fail($"{errorPrefix}: failed on {locator}. Screenshot: {path}");
        }     
    }
}
