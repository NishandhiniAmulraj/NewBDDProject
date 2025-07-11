using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using Microsoft.Extensions.Logging;
using NewBDDProject.CoreLayer.Drivers;
using NewBDDProject.CoreLayer.Helpers;
using NewBDDProject.CoreLayer.Logs;
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
            var projectRoot = Path.GetFullPath(
         Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            var reportDir = Path.Combine(projectRoot, "Reports");
            Directory.CreateDirectory(reportDir);

            var reportPath = Path.Combine(reportDir, "Report.html");
            Logger.Info($"Report will be created at {reportPath}");

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
            Logger.Info($"[FEATURE START] {fc.FeatureInfo.Title}");
        }

        [BeforeScenario(Order = 3)]
        [Scope(Tag = "Login")]
        public void ScenarioStart()
        {
            var title = _sc.ScenarioInfo.Title;

            _scenarioTest = _featureTest.CreateNode(title);
            _ = DriverManager.Instance;
            Logger.Info($"[SCENARIO START] {title}");
        }

        [BeforeStep, AfterStep(Order = 4)]
        [Scope(Tag = "Login")]
        public void LogStep()
        {
            var info = _sc.StepContext.StepInfo;
            //_scenarioTest.Info($"→ {_sc.StepContext.StepInfo.Text}");
            Logger.Info($"[STEP START] {info.StepDefinitionType} {info.Text}");
            _scenarioTest.Info($"→ {info.Text}");
        }

        [AfterStep(Order = 5)]
        [Scope(Tag = "Login")]
        public void OnFailure()
        {
            var info = _sc.StepContext.StepInfo;
            if (_sc.TestError != null)
            {
                Logger.Error($"[STEP FAIL] {info.Text}", _sc.TestError);
                var img = ScreenshotHelper.Capture(DriverManager.Instance, _sc.ScenarioInfo.Title);
                _scenarioTest.Fail(_sc.TestError.Message).AddScreenCaptureFromPath(img);
            }
            else
            {
                Logger.Info($"[STEP PASS] {info.Text}");
            }
            
        }

        [AfterScenario(Order = 6)]
        [Scope(Tag = "Login")]
        public void ScenarioTeardown()
        {
            var title = _sc.ScenarioInfo.Title;
            Logger.Info($"[SCENARIO END] {title}");
            DriverManager.Quit();
        }

        [AfterTestRun(Order = 7)]
        public static void FinalizeReport()
        {
            Logger.Info("[REPORT FLUSH]");
            _extent.Flush();
        }
    }
}