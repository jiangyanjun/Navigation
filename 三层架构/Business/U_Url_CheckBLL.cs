using DataLayer;
using PhysicalLayer;

namespace Business
{
    public class U_Url_CheckBLL : BaseBLL<U_Url_Check>
    {
        public override void SetCurrentDAL()
        {
            base.CurrentDAL = new U_Url_CheckDAL();
        }
    }
}
