using CommonLayer;
using DataLayer;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Business
{
    public class U_Url_ListBLL : BaseBLL<U_Url_List>
    {
        public U_Url_List Query<U_Url_List>(string where) where U_Url_List : class
        {
            var dal = base.CurrentDAL;
            return dal.Query<U_Url_List>(where);
        }
        public override void SetCurrentDAL()
        {
            base.CurrentDAL = new U_Url_ListDAL();
        }
        public List<U_Url_List> QuerySql(string sql)
        {
            return base.CurrentDAL.QuerySql<U_Url_List>(sql);
        }
        public int UpdateStatus(string id, int status, string user)
        {
            return new U_Url_ListDAL().UpdateStatus(id, status, user);
        }

        public List<U_Url_List> FindPrimaryId(string Id)
        {
            return new U_Url_ListDAL().Find<U_Url_List>(string.Format("@Id='{0}'", Id));
        }
        public int UpdateTypes(string pTypeId, string pUrlId, string user)
        {
            return new U_Url_ListDAL().UpdateTypes(pTypeId, pUrlId, user);
        }
        public List<U_Url_List> JoinTypeFind()
        {
            return new U_Url_ListDAL().JoinTypeFind();
        }
    }
}
