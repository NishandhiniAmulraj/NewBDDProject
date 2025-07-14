using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBDDProject.CoreLayer.LogClass
    {
        public static class Log
        {
            private static readonly NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();
            public static void Info(string msg) => _log.Info(msg);
            public static void Error(string msg, Exception ex) => _log.Error(ex, msg);
        }
    }

