using FluentAssertions;
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

        [Given(@"I navigate to the login page")]
        public void GivenNavigate() => _flow.NavigateToUrl();

        [When(@"I login with ""(.*)"" and ""(.*)""")]
        public void WhenLogin(string u, string p) => _flow.DoLogin(u, p);

        [Then(@"I should see the products page")]
        public void ThenVerify()
        {
            //Assert task no: 6
            Assert.True(_flow.IsLoggedIn(), "Login failed");
            //Fluent Assertions:
            _flow.IsLoggedIn().Should().BeTrue("user should be logged in after valid credentials");

        }
    }
}
