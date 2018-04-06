using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Enum
{
    public enum EnumType
    {
        /// <summary>
        /// 无效
        /// </summary>
        [Description("无效")]
        invalid = 0,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1
    }
}
