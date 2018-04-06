using CommonLayer;
using DataBase;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class U_Url_TypeDAL : DalBase<U_Url_Type>
    {
        /// <summary>
        /// 类型管理
        /// </summary>
        /// <returns></returns>
        public List<UrlType> GetTypeManager()
        {
            string sql = @"SELECT T.ID,T.NAME,T.SORTDESC,T.STATUS,T.PARENTID,COUNT(U.URL) AS URLCOUNT
                                FROM U_URL_TYPE T
                                LEFT JOIN U_URL_LIST U ON T.ID=U.TYPES
                                GROUP BY T.ID,T.NAME,T.SORTDESC,T.STATUS,T.PARENTID
                                ORDER BY T.SORTDESC DESC,COUNT(U.URL) DESC,T.STATUS DESC";
            return DBHelper.ReadCollection<UrlType>(sql, null);
        }

        /// <summary>
        /// 前台展示
        /// </summary>
        /// <returns></returns>
        public List<UrlType> GetTypes()
        {
            string sql = @"SELECT DISTINCT L.TYPES AS ID,T.NAME AS NAME,COUNT(L.TYPES) AS UrlCount,T.SortDesc,T.STATUS,T.ParentID FROM U_Url_Type T
                                LEFT JOIN U_Url_List L ON T.ID=L.TYPES
                                WHERE L.Status=1
                                GROUP BY T.NAME,L.TYPES
                                ORDER BY T.SortDesc DESC,UrlCount DESC";
            return DBHelper.ReadCollection<UrlType>(sql, null);
        }
    }
    //public class U_Url_TypeDAL : IDBOperating<U_Url_Type>
    //{
    //    public string Sql { get; set; }
    //    public int Add(List<U_Url_Type> _entity)
    //    {
    //        Sql = string.Format(DbStatic.InsertForm, TypeTb.TableName, TypeTb.ColumnName, TypeTb._ColumnName);
    //        return DBHelper.SaveCollection(Sql, _entity);
    //    }
    //    public List<T> FinList<T>(string sql) where T : class
    //    {
    //        return DBHelper.ReadCollection<T>(sql, null);
    //    }
    //    public int Delete(U_Url_Type t)
    //    {
    //        Sql = string.Format(DbStatic.DeleteForm, TypeTb.TableName, t.Id);
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

    //    public List<U_Url_Type> Find()
    //    {
    //        string sql = string.Format(DbStatic.SelectForm, TypeTb.ColumnName, TypeTb.TableName, "");
    //        return DBHelper.ReadCollection<U_Url_Type>(sql, null);
    //    }
    //    /// <summary>
    //    /// 类型管理
    //    /// </summary>
    //    /// <returns></returns>
    //    public List<UrlType> GetTypeManager()
    //    {
    //        string sql = @"SELECT T.ID,T.NAME,T.SORTDESC,T.STATUS,T.PARENTID,COUNT(U.URL) AS URLCOUNT
    //                        FROM U_URL_TYPE T
    //                        LEFT JOIN U_URL_LIST U ON T.ID=U.TYPES
    //                        GROUP BY T.ID,T.NAME,T.SORTDESC,T.STATUS,T.PARENTID
    //                        ORDER BY T.SORTDESC DESC,COUNT(U.URL) DESC,T.STATUS DESC";
    //        return DBHelper.ReadCollection<UrlType>(sql, null);
    //    }

    //    public U_Url_Type Find(string id)
    //    {
    //        string sql = string.Format(DbStatic.SelectForm, TypeTb.ColumnName, TypeTb.TableName, id);
    //        var result = DBHelper.ReadCollection<U_Url_Type>(sql, null);
    //        return result != null && result.Count > 0 ? result[0] : null;
    //    }

    //    public int Update(U_Url_Type t)
    //    {
    //        StringBuilder str = new StringBuilder();
    //        str.AppendFormat("Status={0}", t.Status);
    //        if (t.Name.IsNotNull())
    //        {
    //            str.AppendFormat(",Name='{0}'", t.Name);
    //        }
    //        if (t.Create_Time.IsNotNull())
    //        {
    //            str.AppendFormat(",Create_Time='{0}'", t.Create_Time);
    //        }
    //        if (t.Create_Id.IsNotNull())
    //        {
    //            str.AppendFormat(",Create_Id='{0}'", t.Create_Id);
    //        }
    //        if (t.LastUpdate_Time.IsNotNull())
    //        {
    //            str.AppendFormat(",LastUpdate_Time='{0}'", t.LastUpdate_Time);
    //        }
    //        if (t.LastUpdate_Id.IsNotNull())
    //        {
    //            str.AppendFormat(",LastUpdate_Id='{0}'", t.LastUpdate_Id);
    //        }
    //        if (t.ParentID.IsNotNull())
    //        {
    //            str.AppendFormat(",ParentID='{0}'", t.ParentID);
    //        }
    //        if (t.SortDesc > 0)
    //        {
    //            str.AppendFormat(",SortDesc='{0}'", t.SortDesc);
    //        }
    //        if (t.SortDesc > 0)
    //        {
    //            str.AppendFormat(",SortDesc='{0}'", t.SortDesc);
    //        }
    //        Sql = String.Format(DbStatic.UpdateForm, TypeTb.TableName, str.ToString(), t.Id);
    //        return DBHelper.ExcuteSQL(Sql, null);
    //    }
    //}
}
