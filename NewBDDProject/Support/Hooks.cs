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
        private static ExtentReports _extent;
        private static ExtentTest _featureTest;
        private ExtentTest _scenarioTest;

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
            Logger.Info("[REPORT] Initialized");
        }

        [BeforeFeature]
        public static void FeatureStart()
        {
            _featureTest = _extent.CreateTest("Feature");
            Logger.Info("[FEATURE START]");
        }

        [BeforeScenario]
        [Scope(Tag = "Login")]
        public void ScenarioStart()
        {
            _scenarioTest = _featureTest.CreateNode("Scenario");
            _ = DriverManager.Instance;
            Logger.Info("[SCENARIO START]");
        }

        [BeforeStep]
        [Scope(Tag = "Login")]
        public void LogStep()
        {
            Logger.Info("[STEP START]");
            _scenarioTest?.Info("→ Step");
        }

        [AfterStep]
        [Scope(Tag = "Login")]
        public void AfterStep(ScenarioContext sc)
        {
            var step = sc.StepContext.StepInfo;
            if (sc.TestError != null)
            {
                Logger.Error($"[STEP FAIL] {step.Text}", sc.TestError);
                var img = ScreenshotHelper.Capture(DriverManager.Instance, sc.ScenarioInfo.Title);
                _scenarioTest.Fail(sc.TestError.Message).AddScreenCaptureFromPath(img);
            }
            else
            {
                Logger.Info($"[STEP PASS] {step.Text}");
            }
        }

        [AfterScenario]
        [Scope(Tag = "Login")]
        public void ScenarioTeardown()
        {
            Logger.Info("[SCENARIO END]");
            DriverManager.Quit();
        }

        [AfterTestRun]
        public static void FinalizeReport()
        {
            Logger.Info("[REPORT] Flushing");
            _extent.Flush();
        }
    }
}