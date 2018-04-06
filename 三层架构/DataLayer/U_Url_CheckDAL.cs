using DataBase;
using PhysicalLayer;
using System;
using System.Collections.Generic;

namespace DataLayer
{
    public class U_Url_CheckDAL : DalBase<U_Url_Check>
    {
    }
    //public class U_Url_CheckDAL : IDBOperating<U_Url_Check>
    //{
    //    public List<T> FinList<T>(string sql) where T : class
    //    {
    //        return DBHelper.ReadCollection<T>(sql, null);
    //    }
    //    public string Sql { get; set; }
    //    public int Add(List<U_Url_Check> _entity)
    //    {
    //        Sql = string.Format(DbStatic.InsertForm, U_Url_CheckTb.TableName, U_Url_CheckTb.ColumnName, U_Url_CheckTb._ColumnName);
    //        return DBHelper.SaveCollection(Sql, _entity);
    //    }

    //    public int Delete(U_Url_Check t)
    //    {
    //        Sql = string.Format(DbStatic.DeleteForm, U_Url_CheckTb.TableName, t.Id);
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

    //    public List<U_Url_Check> Find()
    //    {
    //        string sql = string.Format(DbStatic.SelectForm, U_Url_CheckTb.ColumnName, U_Url_CheckTb.TableName, "");
    //        return DBHelper.ReadCollection<U_Url_Check>(sql, null);
    //    }

    //    public U_Url_Check Find(string id)
    //    {
    //        string sql = string.Format(DbStatic.SelectForm, U_Url_CheckTb.ColumnName, U_Url_CheckTb.TableName, id);
    //        var result = DBHelper.ReadCollection<U_Url_Check>(sql, null);
    //        return result != null && result.Count > 0 ? result[0] : null;
    //    }

    //    public int Update(U_Url_Check t)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
