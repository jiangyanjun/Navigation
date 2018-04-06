using DataLayer;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class U_Url_TypeBLL : BaseBLL<U_Url_Type>
    {
        public override void SetCurrentDAL()
        {
            base.CurrentDAL = new U_Url_TypeDAL();
        }
        public List<UrlType> GetTypes()
        {
            return new U_Url_TypeDAL().GetTypes();
        }
    }
}
