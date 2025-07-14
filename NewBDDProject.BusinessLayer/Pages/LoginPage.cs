using NewBDDProject.CoreLayer.Helpers;
using NewBDDProject.CoreLayer.Locators;
using NewBDDProject.CoreLayer.LogClass;
using NewBDDProject.CoreLayer.UI;
using System;

namespace NewBDDProject.BusinessLayer.Pages
{
    public class LoginPage : BasePage
    {
        private readonly ILoginPageElements loc = new LoginPageLocators();
        private readonly IActionWrapper _ui;
        public LoginPage(IActionWrapper ui) => _ui = ui;

        public override void Navigate() =>
            Driver.Navigate().GoToUrl(ConfigHelper.Instance.BaseUrl);

        public void Login(string username, string password) =>
            Login(username, password, useJs: false);

        public void Login(string username, string password, bool useJs)
        {
            var driver = Driver;  // uses BasePage.Driver

            try
            {
                _ui.Type(loc.UsernameInput, username);
                _ui.Type(loc.PasswordInput, password);
                //driver.FindElement(loc.UsernameInput).SendKeys(username);
                //driver.FindElement(loc.PasswordInput).SendKeys(password);

                var btn = driver.FindElement(loc.LoginButton);
                if (useJs)
                {
                    JsExecutor.ExecuteScript("arguments[0].click();", btn);
                }
                else
                {
                    //btn.Click();
                    _ui.Click(loc.LoginButton);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error during login", ex);
                throw;
            }
        }

        public override bool IsAt() =>
            Driver.Url.Contains("inventory");
    }
}
