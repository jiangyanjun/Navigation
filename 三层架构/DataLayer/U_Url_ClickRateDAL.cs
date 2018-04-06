using DataBase;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class U_Url_ClickRateDAL : DalBase<U_Url_ClickRate>
    {
    }
    //public class U_Url_ClickRateDAL : IDBOperating<U_Url_ClickRate>
    //{
    //    public List<T> FinList<T>(string sql) where T : class
    //    {
    //        return DBHelper.ReadCollection<T>(sql, null);
    //    }
    //    public string Sql { get; set; }
    //    public int Add(List<U_Url_ClickRate> _entity)
    //    {
    //        Sql = string.Format(DbStatic.InsertForm, U_Url_ClickRateTb.TableName, U_Url_ClickRateTb.ColumnName, U_Url_ClickRateTb._ColumnName);
    //        return DBHelper.SaveCollection(Sql, _entity);
    //    }

    //    public int Delete(U_Url_ClickRate t)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public long Excute(string _sql)
    //    {
    //        return DBHelper.ExecuteLong(_sql, null);
    //    }

    //    public string ExecuteScalarString(string _sql)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public List<U_Url_ClickRate> Find()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public U_Url_ClickRate Find(string id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public int Update(U_Url_ClickRate t)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
