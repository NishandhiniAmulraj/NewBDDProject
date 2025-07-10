using OpenQA.Selenium;
using System;
using System.IO;

namespace NewBDDProject.CoreLayer.Screenshot
{
    public static class ScreenshotHelper
    {
        public static string Capture(IWebDriver driver, string name)
        {
            var folder = Path.Combine("Reports", "Screenshot");
            Directory.CreateDirectory(folder);

            var file = Path.Combine(folder, $"{name}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            var ss = ((ITakesScreenshot)driver).GetScreenshot();

            ss.SaveAsFile(file);

            return file;
        }
    }
}
