using DataLayer;
using PhysicalLayer;

namespace Business
{
    public class S_UserInfoBusiness : BaseBLL<S_UserInfo>
    {
        public override void SetCurrentDAL()
        {
            base.CurrentDAL = new S_UserInfoDAL();
        }
        public S_UserInfo Login(string user, string pwd)
        {
            return new S_UserInfoDAL().Login(user, pwd);
        }
    }
}
