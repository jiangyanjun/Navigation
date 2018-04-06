using DataLayer;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class S_ConfigBLL : BaseBLL<S_Config>
    {
        public override void SetCurrentDAL()
        {
            base.CurrentDAL = new S_ConfigDAL();
        }
        #region 获取配置文件
        private static List<S_Config> _configlist;
        public static List<S_Config> GetConfigList
        {
            get
            {
                if (_configlist == null)
                    _configlist = new S_ConfigBLL().Find();
                return _configlist;
            }
            set { _configlist = value; }
        }
        #endregion
    }
}
