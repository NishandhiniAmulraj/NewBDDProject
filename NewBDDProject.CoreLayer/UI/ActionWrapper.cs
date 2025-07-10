using NewBDDProject.CoreLayer.Screenshot;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                catch (ElementClickInterceptedException ex)
                {
                    Console.WriteLine($"Attempt {attempt}: Click intercepted. Trying JS...");

                    try
                    {
                        var el = _driver.FindElement(locator);
                        ScrollIntoView(el);
                        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", el);
                        return;
                    }
                    catch
                    {
                        //retry
                    }

                    Task.Delay(500).Wait();
                }
                catch (StaleElementReferenceException ex)
                {
                    Console.WriteLine($"Attempt {attempt}: Element stale. Retrying...");
                    Task.Delay(500).Wait();
                }
            }

            var path = ScreenshotHelper.Capture(_driver, "ClickError");
            Assert.Fail($"Failed to click {locator} after {MaxRetries} attempts. Screenshot at {path}");
        }

        public void Type(By locator, string text)
        {
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    var element = _wait.Until(ExpectedConditions.ElementIsVisible(locator));
                    element.Clear();
                    element.SendKeys(text);
                    return;
                }
                catch (StaleElementReferenceException ex)
                {
                    Console.WriteLine($"Type attempt {attempt} stale: {ex.Message}");
                    Task.Delay(500).Wait();
                }

                if (attempt == MaxRetries)
                {
                    var path = ScreenshotHelper.Capture(_driver, "TypeError");
                    Assert.Fail($"Cannot type into {locator} after {MaxRetries} attempts. Screenshot saved at {path}");
                }
            }
        }

        public string GetText(By locator)
        {
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    var element = _wait.Until(ExpectedConditions.ElementIsVisible(locator));
                    return element.Text;
                }
                catch (StaleElementReferenceException ex)
                {
                    Console.WriteLine($"GetText attempt {attempt} stale: {ex.Message}");
                }

                if (attempt == MaxRetries)
                {
                    var path = ScreenshotHelper.Capture(_driver, "GetTextError");
                    Assert.Fail($"Cannot get text from {locator} after {MaxRetries} attempts. Screenshot saved at {path}");
                }
            }

            return string.Empty; // unreachable, but required by compiler
        }

        private void ScrollIntoView(IWebElement element)
        {
            
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
           
        }
    }
}
