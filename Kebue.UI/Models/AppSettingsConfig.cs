using System.Configuration;
using System.Runtime.CompilerServices;

namespace Kebue.UI.Models
{
    public class AppSettingsConfig
    {
        public static string WebApiUrl
        {
            get
            {
                return AppSettingValue();
            }
        }
        public static string GetTokenApi
        {
            get
            {
                return WebApiUrl + AppSettingValue();
            }
        }

        public static string StaffId
        {
            get
            {
                return AppSettingValue();
            }
        }

        private static string AppSettingValue([CallerMemberName] string key = null)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}