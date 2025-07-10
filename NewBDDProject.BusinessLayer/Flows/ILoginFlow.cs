using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBDDProject.BusinessLayer.Flows
{
    public interface ILoginFlow
    {
        void DoLogin(string u, string p);
        bool IsLoggedIn();
        void NavigateToUrl();
    }
}
