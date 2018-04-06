using DataLayer;
using PhysicalLayer;

namespace Business
{
    public class U_Url_ClickRateBLL : BaseBLL<U_Url_ClickRate>
    {
        public override void SetCurrentDAL()
        {
            base.CurrentDAL = new U_Url_ClickRateDAL();
        }
    }
}
