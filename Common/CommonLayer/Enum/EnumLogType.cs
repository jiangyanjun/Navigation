using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Enum
{
    public enum EnumLogType
    {  /// <summary>
       /// 错误
       /// </summary>
        [Description("错误")]
        Erro = 0,
        /// <summary>
        /// 业务
        /// </summary>
        [Description("业务")]
        Info = 1
    }
}
