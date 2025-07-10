using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBDDProject.CoreLayer.Locators
{
    public interface ILoginPageElements
    {
        By UsernameInput { get; }
        By PasswordInput { get; }
        By LoginButton { get; }
    }
    public class LoginPageLocators : ILoginPageElements
    {
        public By UsernameInput => By.Id("user-name");
        public By PasswordInput => By.Id("password");
        public By LoginButton => By.Id("login-button");
    }
}