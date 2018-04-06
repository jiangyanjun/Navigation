using System.ComponentModel;

namespace CommonLayer.Enum
{
    public enum EnumUrlStatus
    {
        /// <summary>
        /// 不通
        /// </summary>
        [Description("不通")]
        Unreasonable = 0,

        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,

        /// <summary>
        /// 涉黄
        /// </summary>
        [Description("涉黄")]
        Jurisprudence = 2,

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 3,

        /// <summary>
        /// 危险
        /// </summary>
        [Description("危险")]
        Danger = 4,

        /// <summary>
        /// 新建
        /// </summary>
        [Description("新建")]
        New = 5,

        /// <summary>
        /// 未定义
        /// </summary>
        [Description("未定义")]
        NotDefined = 6
    }
}
