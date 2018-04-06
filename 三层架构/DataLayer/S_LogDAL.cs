using DataBase;
using PhysicalLayer;
using System;
using System.Collections.Generic;

namespace DataLayer
{
    public class S_LogDAL : DalBase<S_Log>
    {
    }
    //public class S_LogDAL : IDBOperating<S_Log>
    //{
    //    public List<T> FinList<T>(string sql) where T : class
    //    {
    //        return DBHelper.ReadCollection<T>(sql, null);
    //    }
    //    public string Sql { get; set; }
    //    public int Add(List<S_Log> _entity)
    //    {
    //        Sql = string.Format(DbStatic.InsertForm, S_LogTb.TableName, S_LogTb.ColumnName, S_LogTb._ColumnName);
    //        return DBHelper.SaveCollection(Sql, _entity);
    //    }

    //    public int Delete(S_Log _entity)
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

    //    public List<S_Log> Find()
    //    {
    //        string sql = string.Format(DbStatic.SelectForm, S_LogTb.ColumnName, S_LogTb.TableName, "");
    //        return DBHelper.ReadCollection<S_Log>(sql, null);
    //    }

    //    public S_Log Find(string Id)
    //    {
    //        string sql = string.Format(DbStatic.SelectForm, S_LogTb.ColumnName, S_LogTb.TableName, "AND Id='" + Id + "'");
    //        var result = DBHelper.ReadCollection<S_Log>(sql, null);
    //        return result != null && result.Count > 0 ? result[0] : null;
    //    }

    //    public int Update(S_Log t)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
