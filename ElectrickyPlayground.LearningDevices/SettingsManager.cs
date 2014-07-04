using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrickyPlayground.LearningDevices
{
    static class SettingsManager
    {
        public static string Port
        {
            get
            {
                return ConfigurationManager.AppSettings["Port"];
            }
        }
    }
}
