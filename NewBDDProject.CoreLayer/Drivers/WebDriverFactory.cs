using Microsoft.Extensions.Options;
using NewBDDProject.CoreLayer.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;

namespace NewBDDProject.CoreLayer.Drivers
    {
        public static class WebDriverFactory
        {
            public static IWebDriver CreateDriver()
            {
                // Read desired browser type and its options
                var browser = ConfigHelper.Instance.Browser.ToLower();
                var cfg = ConfigHelper.Instance.GetBrowserConfig(browser);

                switch (browser.ToLower())
                {
                    case "chrome":                      
                        return CreateChromeDriver(cfg.Arguments);
                    case "firefox":
                        return CreateFirefoxDriver(cfg.Arguments);
                    case "edge":
                        return CreateEdgeDriver(cfg.Arguments);
                    default:
                        throw new NotSupportedException($"Browser '{browser}' is not supported.");
                }

            }

            private static IWebDriver CreateChromeDriver(string[] args)
            {
                var options = new ChromeOptions();
                if (args != null) options.AddArguments(args);
                return new ChromeDriver(options);
            }

            private static IWebDriver CreateFirefoxDriver(string[] args)
            {
                var options = new FirefoxOptions();
                if (args != null) options.AddArguments(args);
                return new FirefoxDriver(options);
            }

            private static IWebDriver CreateEdgeDriver(string[] args)
            {
                var options = new EdgeOptions();
                if (args != null) options.AddArguments(args);
                return new EdgeDriver(options);
            }
        }
    }

