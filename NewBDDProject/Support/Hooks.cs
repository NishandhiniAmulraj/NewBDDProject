using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using NewBDDProject.CoreLayer.Drivers;
using NewBDDProject.CoreLayer.Screenshot;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBDDProject.Support
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _sc;
        private static ExtentReports _extent;
        private static ExtentTest _featureTest;
        private ExtentTest _scenarioTest;

        public Hooks(ScenarioContext sc) => _sc = sc;

        [BeforeTestRun(Order = 1)]
        public static void SetupReporting()
        {
            var baseDir = AppContext.BaseDirectory;
            var reportDir = Path.Combine(baseDir, "Reports");
            Directory.CreateDirectory(reportDir);

            var reportPath = Path.Combine(reportDir, "Report.html");
            Console.WriteLine($"[Report] Output path: {reportPath}");

            var spark = new ExtentSparkReporter(reportPath);
            spark.Config.DocumentTitle = "Automation Report";
            spark.Config.ReportName = "BDD Test Results";
            spark.Config.Theme = Theme.Standard;

            _extent = new ExtentReports();
            _extent.AttachReporter(spark);
        }

        [BeforeFeature(Order = 2)]
        public static void FeatureStart(FeatureContext fc)
        {
            _featureTest = _extent.CreateTest(fc.FeatureInfo.Title);
        }

        [BeforeScenario(Order = 3)]
        [Scope(Tag = "Login")]
        public void ScenarioStart()
        {
            _scenarioTest = _featureTest.CreateNode(_sc.ScenarioInfo.Title);
            _ = DriverManager.Instance;
        }

        [BeforeStep, AfterStep(Order = 4)]
        [Scope(Tag = "Login")]
        public void LogStep()
        {
            _scenarioTest.Info($"→ {_sc.StepContext.StepInfo.Text}");
        }

        [AfterStep(Order = 5)]
        [Scope(Tag = "Login")]
        public void OnFailure()
        {
            if (_sc.TestError != null)
            {
                var img = ScreenshotHelper.Capture(DriverManager.Instance, _sc.ScenarioInfo.Title);
                _scenarioTest.Fail(_sc.TestError.Message)
                             .AddScreenCaptureFromPath(img);
            }
        }

        [AfterScenario(Order = 6)]
        [Scope(Tag = "Login")]
        public void ScenarioTeardown() => DriverManager.Quit();

        [AfterTestRun(Order = 7)]
        public static void FinalizeReport()
        {
            Console.WriteLine("[Report] Flushing report to disk");
            _extent.Flush();
          /*  if (_extent is IDisposable disp)
            {
                Console.WriteLine("[Report] Disposing report...");
                disp.Dispose();
            }*/
        }
    }
}