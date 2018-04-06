using DataLayer;
using PhysicalLayer;
using System;
using System.Collections.Generic;

namespace Business
{
    public abstract class BaseBLL<T> where T : class, new()
    {
        public DalBase<T> CurrentDAL;
        public BaseBLL() { SetCurrentDAL(); }
        public abstract void SetCurrentDAL();
        public int Add(List<T> obj)
        {
            Func<List<T>, int> func = FuncAdd;
            return func(obj);
        }
        private int FuncAdd(List<T> obj)
        {
            return CurrentDAL.Add(obj);
        }
        public int Add(T obj)
        {
            Func<T, int> func = FuncAdd;
            return func(obj);
        }
        private int FuncAdd(T obj)
        {
            List<T> list = new List<T>();
            list.Add(obj);
            return CurrentDAL.Add(list);
        }
        public int Delete(T obj)
        {
            Func<T, int> func = FuncDelete;
            return func(obj);
        }
        private int FuncDelete(T obj)
        {
            return CurrentDAL.Delete(obj);
        }
        public long Excute(string _sql)
        {
            Func<string, long> func = FuncExcute;
            return func(_sql);
        }
        private long FuncExcute(string _sql)
        {
            return CurrentDAL.Excute(_sql);
        }
        public string ExecuteScalarString(string _sql)
        {
            Func<string, string> func = FuncExecuteScalarString;
            return func(_sql);
        }
        private string FuncExecuteScalarString(string _sql)
        {
            return CurrentDAL.ExecuteScalarString(_sql);
        }
        public List<T> Find()
        {
            Func<List<T>> func = FuncFind;
            return func();
        }
        public List<T> FindList(string where)
        {
            return CurrentDAL.Find<T>(where);
        }
        private List<T> FuncFind()
        {
            return CurrentDAL.Find<T>();
        }
        public T Find(string Id)
        {
            Func<string, T> func = FuncFind;
            return func(Id);
        }
        private T FuncFind(string Id)
        {
            return CurrentDAL.Query<T>(Id);
        }
        public int Update(T t)
        {
            Func<T, int> func = FuncUpdate;
            return func(t);
        }
        public int Update(List<T> t)
        {
            Func<List<T>, int> func = FuncUpdate;
            return func(t);
        }
        private int FuncUpdate(T t)
        {
            return CurrentDAL.Update(t);
        }
        private int FuncUpdate(List<T> t)
        {
            return CurrentDAL.Update(t);
        }
        public List<T> FinList<T>(string sql) where T : class
        {
            Func<string, List<T>> func = FuncFinList<T>;
            return func(sql);
        }
        private List<T> FuncFinList<T>(string sql) where T : class
        {
            return CurrentDAL.QuerySql<T>(sql);
        }
    }
}
