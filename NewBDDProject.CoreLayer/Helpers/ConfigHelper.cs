using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBDDProject.CoreLayer.Helpers
{
    public class BrowserConfig { public string[] Arguments { get; set; } }

    public sealed class ConfigHelper
    {
        private static readonly Lazy<ConfigHelper> _inst = new Lazy<ConfigHelper>(() => new ConfigHelper());

        private readonly IConfiguration _cfg;
        private ConfigHelper()
        {
            _cfg = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
              .SetBasePath(AppContext.BaseDirectory)
              .AddJsonFile("appsettings.json")
              .Build();
        }
        public static ConfigHelper Instance => _inst.Value;
        public string Browser => _cfg["Browser"];
        public BrowserConfig GetBrowserConfig(string name) => _cfg.GetSection($"Browsers:{name}").Get<BrowserConfig>();

        public string BaseUrl => _cfg["SauceDemo:BaseUrl"];
        public string ReportRoot => _cfg["ReportRoot"];

    }
}