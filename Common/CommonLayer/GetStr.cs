using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class GetStr
    {
        #region 时间格式
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public const string yyyyMMdd = "yyyy-MM-dd";
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        public const string yyyyMMddmmHHss = "yyyy-MM-dd HH:mm:ss"; 
        #endregion
        #region  获取当前时间 （yyyy-MM-dd HH:mm:ss）格式
        /// <summary>
        /// 获取当前时间 （yyyy-MM-dd HH:mm:ss）格式
        /// </summary>
        public static string GetCurrentDate
        {
            get
            {
                return System.DateTime.Now.ToString(yyyyMMddmmHHss);
            }
        }
        /// <summary>
        /// 获取当前时间 （yyyy-MM-dd）格式
        /// </summary>
        public static string GetCurrentDay
        {
            get
            {
                return System.DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        /// <summary>
        /// 获取当前分钟
        /// </summary>
        public static int GetCurrentMinute
        {
            get
            {
                return System.DateTime.Now.Minute;
            }
        }
        /// <summary>
        /// 获取当前秒钟
        /// </summary>
        public static int GetCurrentSecond
        {
            get
            {
                return System.DateTime.Now.Second;
            }
        }  /// <summary>
           /// 获取当前时钟
           /// </summary>
        public static int GetCurrentHour
        {
            get
            {
                return System.DateTime.Now.Hour;
            }
        }
        #endregion

        #region 获取GUID
        /// <summary>
        /// 获取GUID
        /// </summary>
        public static string GetGuid { get { return System.Guid.NewGuid().ToString("N"); } }
        #endregion
        public static int GetStrLength(string str)
        {
            int len = 0;
            byte[] b;

            for (int i = 0; i < str.Length; i++)
            {
                b = System.Text.Encoding.Default.GetBytes(str.Substring(i, 1));
                if (b.Length > 1)
                    len += 2;
                else
                    len++;
            }
            return len;
        }
    }
}
