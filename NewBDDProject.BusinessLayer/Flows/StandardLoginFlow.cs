using NewBDDProject.BusinessLayer.Pages;
using NewBDDProject.CoreLayer.Drivers;
using NewBDDProject.CoreLayer.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBDDProject.BusinessLayer.Flows
{
    public class StandardLoginFlow : ILoginFlow
    {
        private readonly IActionWrapper _ui;

        private readonly LoginPage _loginPage;

        public StandardLoginFlow()
        {
            _ui = new ActionWrapper(DriverManager.Instance, TimeSpan.FromSeconds(10));
            _loginPage = new LoginPage(_ui);
        }

        public void DoLogin(string u, string p) => _loginPage.Login(u, p);
        public bool IsLoggedIn() => _loginPage.IsAt();
        public void NavigateToUrl() => _loginPage.Navigate();
    }
}
