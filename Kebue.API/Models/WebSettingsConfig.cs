using System.Configuration;
using System.Runtime.CompilerServices;

namespace Kebue.API.Models
{
    public class WebSettingsConfig
    {
        #region 跨域
        public static string Cors_origins
        {
            get
            {
                return AppSettingValue();
            }
        }
        public static string Cors_headers
        {
            get
            {
                return AppSettingValue();
            }
        }
        public static string Cors_methods
        {
            get
            {
                return AppSettingValue();
            }
        }
        #endregion

        public static string UrlExpireTime
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