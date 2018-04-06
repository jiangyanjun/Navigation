using System.Collections.Generic;

namespace PhysicalLayer
{
    public interface IDBOperating<T>
    {
        /// <summary>
        /// 返回集合
        /// </summary>
        /// <returns></returns>
        List<T> Find<T>() where T : class;
        List<T> Find<T>(string where) where T : class;
        /// <summary>
        /// 根据单条SQL返回集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        T Query<T>() where T : class;
        T Query<T>(string where) where T : class;
        /// <summary>
        /// 集合增加到数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Add(List<T> _entity);
        int Add(T t);
        /// <summary>
        /// 集合删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Delete(T t);
        int Delete(string Where);
        int Delete(List<T> _entity);
        /// <summary>
        /// 执行单条sql返回受影响行数
        /// </summary>
        /// <param name="_sql"></param>
        /// <returns></returns>
        long Excute(string _sql);
        /// <summary>
        /// 集合更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(T t);
        int Update(List<T> _entity);
        /// <summary>
        /// 查询字符串单个返回值
        /// </summary>
        /// <param name="_sql"></param>
        /// <returns></returns>
        string ExecuteScalarString(string _sql);
        List<T> QuerySql<T>(string sql) where T : class;
    }
}
