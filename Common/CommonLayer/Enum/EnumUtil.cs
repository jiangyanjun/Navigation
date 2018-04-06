using CommonLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public static class EnumUtil
    {
        #region 公共扩展方法
            /// <summary>
            /// 通过枚举值获取描述信息
            /// </summary>
            /// <param name="enumValue">枚举值</param>
            /// <returns>描述信息</returns>
        public static string ToEnumDiscription<T>(this int? enumValue)
        {
            if (enumValue.HasValue)
                return GetDescriptionByValue<T>(enumValue.ToString());
            else
                return string.Empty;
        }

        /// <summary>
        /// 通过枚举值获取描述信息
        /// </summary>
        /// <param name="enumValue">枚举值</param>
        /// <returns>描述信息</returns>
        public static string ToEnumDiscription<T>(this int enumValue)
        {
            return GetDescriptionByValue<T>(enumValue.ToString());
        }

        /// <summary>
        /// 通过枚举值获取描述信息
        /// </summary>
        /// <param name="enumValue">枚举值</param>
        /// <returns>描述信息</returns>
        public static string ToEnumDiscription<T>(this string enumValue)
        {
            return GetDescriptionByValue<T>(enumValue);
        }

        /// <summary>
        /// 通过枚举项获取描述信息
        /// </summary>
        /// <param name="enumInstance">枚举项</param>
        /// <returns>描述信息</returns>
        public static string ToEnumDiscription<T>(this T enumInstance)
        {
            return GetDescriptionByEnum<T>(enumInstance);
        }

        /// <summary>
        /// 通过枚举项获取描述信息
        /// </summary>
        /// <param name="enumInstance">枚举项</param>
        /// <returns>描述信息</returns>
        public static string ToEnumDiscriptionEnglish<T>(this T enumInstance)
        {
            return GetDescriptionEnglishByEnum<T>(enumInstance);
        }

        /// <summary>
        /// 通过枚举名得到枚举值
        /// </summary>
        /// <param name="enumName">枚举名</param>
        /// <returns>枚举值</returns>
        public static int ToEnumValue<T>(this string enumName)
        {
            return GetEnumByValue<T>(enumName).GetHashCode();
        }

        /// <summary>
        /// 通过枚举名得到枚举
        /// </summary>
        /// <param name="enumName">枚举名</param>
        /// <returns>枚举</returns>
        public static T ToEnum<T>(this string enumName)
        {
            return GetEnumByValue<T>(enumName);
        }
        /// <summary>
        /// 通过枚举值得到枚举
        /// </summary>
        /// <param name="enumName">枚举值</param>
        /// <returns>枚举</returns>
        public static T ToEnum<T>(this int enumValue)
        {
            return GetEnumByValue<T>(enumValue);
        }

        /// <summary>
        /// 通过枚举描述得到枚举值
        /// </summary>
        /// <param name="description">枚举描述</param>
        /// <returns>枚举</returns>
        public static int ToEnumByDescription<T>(this string description)
        {
            var dataList = EnumUtilData<T>.enumDataList;
            var data = dataList.Where(r => r.Description.IsEqual(description)).SingleOrDefault();

            return ToEnumValue<T>(data.Name);
        }


        #endregion 公共扩展方法

        #region 私有方法
        /// <summary>
        /// 获取枚举的数据源
        /// </summary>
        /// <returns>数据源</returns>
        public static List<EnumDataModel> GetEnumDataList<T>()
        {
            return EnumUtilData<T>.enumDataList;
        }

        /// <summary>
        /// 通过枚举获取描述信息
        /// </summary>
        /// <param name="enumValue">枚举值</param>
        /// <returns>描述信息</returns>
        private static string GetDescriptionByValue<T>(string enumValue)
        {
            T t = GetEnumByValue<T>(enumValue);

            return GetDescriptionByEnum<T>(t);
        }

        /// <summary>
        /// 通过枚举获取描述信息
        /// </summary>
        /// <param name="enumInstance">枚举</param>
        /// <returns>描述信息</returns>
        private static string GetDescriptionByEnum<T>(T enumInstance)
        {
            List<EnumDataModel> enumDataList = GetEnumDataList<T>();
            EnumDataModel enumData = enumDataList.Find(m => m.Value == enumInstance.GetHashCode());
            if (enumData != null)
            {
                return enumData.Description;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 通过枚举获取描述信息,下单需传入
        /// </summary>
        /// <param name="enumInstance">枚举</param>
        /// <returns>描述信息</returns>
        private static string GetDescriptionEnglishByEnum<T>(T enumInstance)
        {
            List<EnumDataModel> enumDataList = GetEnumDataList<T>();
            EnumDataModel enumData = enumDataList.Find(m => m.Value == enumInstance.GetHashCode());
            if (enumData != null)
            {
                return enumData.DescriptionEnglish;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 通过枚举值得到枚举
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>枚举</returns>
        public static T GetEnumByValue<T>(int value)
        {
            return GetEnumByValue<T>(value.ToString());
        }

        /// <summary>
        /// 通过枚举值得到枚举
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>枚举</returns>
        private static T GetEnumByValue<T>(string value)
        {
            string msg = string.Empty;
            try
            {
                Type t = typeof(T);
                return (T)System.Enum.Parse(t, value);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return default(T);
            }
        }

        /// <summary>
        /// 内部实现类，缓存
        /// </summary>
        /// <typeparam name="Tenum">枚举类型</typeparam>
        private static class EnumUtilData<Tenum>
        {
            /// <summary>
            /// 缓存数据
            /// </summary>
            internal static readonly List<EnumDataModel> enumDataList;

            static EnumUtilData()
            {
                enumDataList = InitData();
            }

            /// <summary>
            /// 初始化数据，生成枚举和描述的数据表
            /// </summary>
            private static List<EnumDataModel> InitData()
            {
                List<EnumDataModel> enumDataList = new List<EnumDataModel>();

                EnumDataModel enumData = new EnumDataModel();
                Type t = typeof(Tenum);
                FieldInfo[] fieldInfoList = t.GetFields();
                foreach (FieldInfo tField in fieldInfoList)
                {
                    if (!tField.IsSpecialName)
                    {
                        enumData = new EnumDataModel();
                        enumData.Name = tField.Name;
                        enumData.Value = ((Tenum)System.Enum.Parse(t, enumData.Name)).GetHashCode();

                        DescriptionAttribute[] enumAttributelist = (DescriptionAttribute[])tField.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (enumAttributelist != null && enumAttributelist.Length > 0)
                        {
                            enumData.Description = enumAttributelist[0].Description;
                        }
                        else
                        {
                            enumData.Description = tField.Name;
                        }
                        DescriptionEnglishAttribute[] enumAttributeEnglishlist = (DescriptionEnglishAttribute[])tField.GetCustomAttributes(typeof(DescriptionEnglishAttribute), false);
                        if (enumAttributeEnglishlist != null && enumAttributeEnglishlist.Length > 0)
                        {
                            enumData.DescriptionEnglish = enumAttributeEnglishlist[0].DescriptionEnglish;
                        }
                        else
                        {
                            enumData.DescriptionEnglish = tField.Name;
                        }
                        enumDataList.Add(enumData);
                    }
                }
                return enumDataList;
            }
        }

        /// <summary>
        /// 枚举数据实体
        /// </summary>
        public class EnumDataModel
        {
            /// <summary>
            /// get or set 枚举名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// get or set 枚举值
            /// </summary>
            public int Value { get; set; }

            /// <summary>
            /// get or set 枚举描述
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// get or set 枚举描述
            /// </summary>
            public string DescriptionEnglish { get; set; }
        }
        #endregion 私有方法
    }
    public class DescriptionEnglishAttribute : Attribute
    {
        public virtual string DescriptionEnglish
        {
            get
            {
                return this.DescriptionEnglishValue;
            }
        }
        protected string DescriptionEnglishValue { get; set; }
        public DescriptionEnglishAttribute(string descriptionEnglish)
        {
            this.DescriptionEnglishValue = descriptionEnglish;
        }
    }
}
