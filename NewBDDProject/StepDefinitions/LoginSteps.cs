using NewBDDProject.BusinessLayer.Flows;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBDDProject.StepDefinitions
{
    [Binding]
    public class LoginSteps
    {
        private readonly ILoginFlow _flow = new StandardLoginFlow();
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;

        public LoginSteps(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;

            // Log some info using context:
            var feature = _featureContext.FeatureInfo.Title;
            var scenario = _scenarioContext.ScenarioInfo.Title;
            Console.WriteLine($"Starting Feature: {feature}, Scenario: {scenario}");
        }

        [Given(@"I navigate to the login page")]
        public void GivenNavigate()
        {
            _scenarioContext["StartTime"] = DateTime.UtcNow;  
            _flow.NavigateToUrl();
        }

        [When(@"I login with ""(.*)"" and ""(.*)""")]
        public void WhenLogin(string u, string p)
        {
            _scenarioContext["Username"] = u;
            _flow.DoLogin(u, p);
        }

        [Then(@"I should see the products page")]
        public void ThenVerify()
        {
            //Assert task no: 6
            Assert.True(_flow.IsLoggedIn(), "Login failed");
            var duration = DateTime.UtcNow - (DateTime)_scenarioContext["StartTime"];
            Console.WriteLine($"User '{_scenarioContext["Username"]}' login took {duration.TotalSeconds} seconds");
        }
    }
}
