using DataBase;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class S_ConfigDAL : DalBase<S_Config>
    {
    }
    //public class S_ConfigDAL : IDBOperating<S_Config>
    //{
    //    public List<T> FinList<T>(string sql) where T : class
    //    {
    //        return DBHelper.ReadCollection<T>(sql, null);
    //    }
    //    public string Sql { get; set; }
    //    public int Add(List<S_Config> _entity)
    //    {
    //        Sql = string.Format(DbStatic.InsertForm, S_ConfigTb.TableName, S_ConfigTb.ColumnName, S_ConfigTb._ColumnName);
    //        return DBHelper.SaveCollection(Sql, _entity);
    //    }

    //    public int Delete(S_Config _entity)
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

    //    public List<S_Config> Find()
    //    {
    //        string sql = string.Format(DbStatic.SelectForm, S_ConfigTb.ColumnName, S_ConfigTb.TableName, "");
    //        return DBHelper.ReadCollection<S_Config>(sql, null);
    //    }

    //    public S_Config Find(string Id)
    //    {
    //        string sql = string.Format(DbStatic.SelectForm, S_ConfigTb.ColumnName, S_ConfigTb.TableName, "AND Id='" + Id + "'");
    //        var result = DBHelper.ReadCollection<S_Config>(sql, null);
    //        return result != null && result.Count > 0 ? result[0] : null;
    //    }

    //    public int Update(S_Config t)
    //    {
    //        Sql = string.Format(DbStatic.UpdateForm, S_ConfigTb.TableName, string.Format("Value='{0}'", t.Value), t.Id);
    //        return DBHelper.ExcuteSQL(Sql, null);
    //    }
    //}
}
