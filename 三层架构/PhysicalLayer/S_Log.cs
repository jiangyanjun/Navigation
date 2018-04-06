using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalLayer
{
   public class S_Log
    {
        public string Id { get; set; }
        public string Log_Type { get; set; }
        public string Msg { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// 目录
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 子目录
        /// </summary>
        public string SubCategory { get; set; }
        public string Create_Time { get; set; }
    }
}
