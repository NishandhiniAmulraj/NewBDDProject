using NewBDDProject.CoreLayer.Helpers;
using OpenQA.Selenium;
using System;
using System.IO;

namespace NewBDDProject.CoreLayer.Screenshot
{
    public static class ScreenshotHelper
    {
        public static string Capture(IWebDriver driver, string name)
        {
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            var folder = Path.Combine(projectRoot, "Screenshots");
            Directory.CreateDirectory(folder);
            var file = Path.Combine(folder, $"{name}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(file);
            return file;
            /*  var root = ConfigHelper.Instance.ReportRoot;
              var folder = Path.Combine(AppContext.BaseDirectory, root, "Screenshots");
              Directory.CreateDirectory(folder);

              var file = Path.Combine(folder, $"{name}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
              ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(file);
              return file; */
        }
    }
}
