using CommonLayer;
using DataBase;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class S_UserInfoDAL : DalBase<S_UserInfo>
    {
        public S_UserInfo Login(string user, string pwd)
        {
            string sql = string.Format(DbStatic.SelectForm, S_UserInfoTb.ColumnName, S_UserInfoTb.TableName, string.Format("AND USERACCOUNT='{0}' AND PASSWORD = '{1}'", user, pwd));
            return QuerySql<S_UserInfo>(sql).FirstOrDefault();
        }
    }
    //public class S_UserInfoDAL : IDBOperating<S_UserInfo>
    //{
    //    public List<T> FinList<T>(string sql) where T : class
    //    {
    //        return DBHelper.ReadCollection<T>(sql, null);
    //    }
    //    public string Sql { get; set; }
    //    public int Add(List<S_UserInfo> _entity)
    //    {
    //        Sql = string.Format(DbStatic.InsertForm, S_UserInfoTb.TableName, S_UserInfoTb.ColumnName, S_UserInfoTb._ColumnName);
    //        return DBHelper.SaveCollection(Sql, _entity);
    //    }

    //    public int Delete(S_UserInfo _entity)
    //    {
    //        Sql = string.Format(DbStatic.DeleteForm, UrlListTb.TableName, _entity.Id);
    //        return DBHelper.ExcuteSQL(Sql, null);
    //    }

    //    public long Excute(string _sql)
    //    {
    //        return DBHelper.ExecuteLong(_sql, null);
    //    }

    //    public string ExecuteScalarString(string _sql)
    //    {
    //        return DBHelper.ExecuteScalarString(_sql, null);
    //    }

    //    public List<S_UserInfo> Find()
    //    {
    //        string sql = string.Format(DbStatic.SelectForm, S_UserInfoTb.ColumnName, S_UserInfoTb.TableName, "");
    //        return DBHelper.ReadCollection<S_UserInfo>(sql, null);
    //    }
    //    public S_UserInfo Login(string user, string pwd)
    //    {
    //        string sql = string.Format(DbStatic.SelectForm, S_UserInfoTb.ColumnName, S_UserInfoTb.TableName,string.Format("AND USERACCOUNT='{0}' AND PASSWORD = '{1}'", user, pwd));
    //        return FinList<S_UserInfo>(sql).FirstOrDefault();
    //    }
    //    public S_UserInfo Find(string Id)
    //    {
    //        string sql = string.Format(DbStatic.SelectForm, S_UserInfoTb.ColumnName, S_UserInfoTb.TableName, "AND Id='" + Id + "'");
    //        var result = DBHelper.ReadCollection<S_UserInfo>(sql, null);
    //        return result != null && result.Count > 0 ? result[0] : null;
    //    }

    //    public int Update(S_UserInfo t)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
