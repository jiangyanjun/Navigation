using CommonLayer;
using DataBase;
using PhysicalLayer;
using System.Collections.Generic;

namespace DataLayer
{
    public class U_Url_ListDAL : DalBase<U_Url_List>
    {
        public string Sql { get; set; }
        public int UpdateTypes(string pTypeId, string pUrlId, string user)
        {
            Sql = string.Format(DbStatic.UpdateForm, UrlListTb.TableName, string.Format("Types = '{0}',LastUpdate_Time='{1}',LastUpdate_Id='{2}'", pTypeId, GetStr.GetCurrentDate, user), pUrlId);
            return DBHelper.ExcuteSQL(Sql, null);
        }
        public int SortValue(string id, int SortValue, string user)
        {
            Sql = string.Format("update U_Url_List set SortDesc={0},LastUpdate_Id='{3}',LastUpdate_Time='{2}' where id='{1}'", SortValue, id, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user);
            return DBHelper.ExcuteSQL(Sql, null);
        }
        public int UpdateStatus(string id, int status, string user)
        {
            Sql = string.Format("update U_Url_List set STATUS={0},LastUpdate_Id='{3}',LastUpdate_Time='{2}' where id='{1}'", status, id, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user);
            return DBHelper.ExcuteSQL(Sql, null);
        }
        public List<U_Url_List> JoinTypeFind()
        {
            string sql = string.Format(UrlListTb.SelectForm, "");
            return DBHelper.ReadCollection<U_Url_List>(sql, null);
        }
    }
    //public class U_Url_ListDAL : IDBOperating<U_Url_List>
    //{
    //    public string Sql { get; set; }
    //    public int Add(List<U_Url_List> t)
    //    {
    //        Sql = string.Format(DbStatic.InsertForm, UrlListTb.TableName, UrlListTb.ColumnName, UrlListTb._ColumnName);
    //        return DBHelper.SaveCollection(Sql, t);
    //    }
    //    public int Delete(U_Url_List _entity)
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

    //    public List<U_Url_List> FindSql(string sql)
    //    {
    //        return DBHelper.ReadCollection<U_Url_List>(sql, null);
    //    }
    //    public List<U_Url_List> Find()
    //    {
    //        string sql = string.Format(UrlListTb.SelectForm, "");
    //        return DBHelper.ReadCollection<U_Url_List>(sql, null);
    //    }
    //    public U_Url_List Find(string Id)
    //    {
    //        string sql = string.Format(UrlListTb.SelectForm, Id);
    //        var result = DBHelper.ReadCollection<U_Url_List>(sql, null);
    //        return result != null && result.Count > 0 ? result[0] : null;
    //    }
    //    public List<U_Url_List> Find_New(string Id)
    //    {
    //        string sql = string.Format(DbStatic.SelectForm, UrlListTb.ColumnName, UrlListTb.TableName, Id);
    //        return DBHelper.ReadCollection<U_Url_List>(sql, null);
    //    }
    //    public int Update(U_Url_List t)
    //    {
    //        StringBuilder str = new StringBuilder();
    //        str.AppendFormat(",LastUpdate_Time='{0}'", t.LastUpdate_Time);
    //        if (t.Name.IsNotNull())
    //        {
    //            str.AppendFormat(",Name='{0}'", t.Name);
    //        }
    //        if (t.LastUpdate_Id.IsNotNull())
    //        {
    //            str.AppendFormat(",LastUpdate_Id='{0}'", t.LastUpdate_Id);
    //        }
    //        if (t.IcomStream.IsNotNull())
    //        {
    //            str.AppendFormat(",IcomStream='{0}'", t.IcomStream);
    //        }
    //        if (t.Url.IsNotNull())
    //        {
    //            str.AppendFormat(",Url='{0}'", t.Url);
    //        }
    //        if (t.SortDesc > 0)
    //        {
    //            str.AppendFormat(",SortDesc='{0}'", t.SortDesc);
    //        }
    //        if (t.Source.IsNotNull())
    //        {
    //            str.AppendFormat(",Source='{0}'", t.Source);
    //        }
    //        if (t.Title.IsNotNull())
    //        {
    //            str.AppendFormat(",Title='{0}'", t.Title);
    //        }
    //        if (t.Types.IsNotNull())
    //        {
    //            str.AppendFormat(",Types='{0}'", t.Types);
    //        }
    //        if (t.IconImg.IsNotNull())
    //        {
    //            str.AppendFormat(",IconImg='{0}'", t.IconImg);
    //        }
    //        Sql = string.Format(DbStatic.UpdateForm, UrlListTb.TableName, str.ToString().Substring(1), t.Id);
    //        return DBHelper.ExcuteSQL(Sql, null);
    //    }


    //    public int UpdateTypes(string pTypeId, string pUrlId, string user)
    //    {
    //        Sql = string.Format(DbStatic.UpdateForm, UrlListTb.TableName, string.Format("Types = '{0}',LastUpdate_Time='{1}',LastUpdate_Id='{2}'", pTypeId, GetStr.GetCurrentDate, user), pUrlId);
    //        return DBHelper.ExcuteSQL(Sql, null);
    //    }

    //    public int SortValue(string id, int SortValue, string user)
    //    {
    //        Sql = string.Format("update U_Url_List set SortDesc={0},LastUpdate_Id='{3}',LastUpdate_Time='{2}' where id='{1}'", SortValue, id, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user);
    //        return DBHelper.ExcuteSQL(Sql, null);
    //    }

    //    public List<T> FinList<T>(string sql) where T : class
    //    {
    //        return DBHelper.ReadCollection<T>(sql, null);
    //    }
    //}
}
