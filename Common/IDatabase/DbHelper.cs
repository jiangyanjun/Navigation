using Database;
using System;
using System.Data;
using System.Data.Common;

namespace Database
{
    /// <summary>
    /// 说 明: .NET通用数据库操作帮助类,可支持Odbc、OleDb、OracleClient、SqlClient、SqlServerCe等多种数据库提供程序操作
    /// </summary>
    public sealed class DbHelper
    {
        #region 方法函数

        #region 创建DbProviderFactory对象(静态方法)
        /// <summary>
        /// 根据参数名称创建一个数据库提供程序DbProviderFactory对象
        /// </summary>
        /// <param name="dbProviderName">数据库提供程序的名称</param>
        public static DbProviderFactory CreateDbProviderFactory(string dbProviderName)
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(dbProviderName);

            return dbFactory;
        }
        #endregion

        #region 创建DbConnection对象(静态方法)
        /// <summary>
        /// 根据数据库连接字符串参数来创建数据库链接.
        /// </summary>
        /// <param name="connectionString">数据库连接配置字符串</param>
        /// <param name="dbProviderName">数据库提供程序的名称</param>
        /// <returns></returns>
        public static DbConnection CreateDbConnection(string connectionString, string dbProviderName)
        {
            DbProviderFactory dbFactory = DbHelper.CreateDbProviderFactory(dbProviderName);

            DbConnection dbConn = dbFactory.CreateConnection();
            dbConn.ConnectionString = connectionString;

            return dbConn;
        }
        #endregion

        #region 添加DbCommand参数
        /// <summary>
        /// 把参数集合添加到DbCommand对象中
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="dbParameterCollection">数据库操作集合</param>
        public void AddParameterCollection(DbCommand cmd, DbParameterCollection dbParameterCollection)
        {
            if (cmd != null)
            {
                foreach (DbParameter dbParameter in dbParameterCollection)
                {
                    cmd.Parameters.Add(dbParameter);
                }
            }
        }

        /// <summary>
        /// 把输出参数添加到DbCommand对象中
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数的类型</param>
        /// <param name="size">参数的大小</param>
        public void AddOutParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
        {
            if (cmd != null)
            {
                DbParameter dbParameter = cmd.CreateParameter();

                dbParameter.DbType = dbType;
                dbParameter.ParameterName = parameterName;
                dbParameter.Size = size;
                dbParameter.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(dbParameter);
            }
        }

        /// <summary>
        /// 把输入参数添加到DbCommand对象中
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数的类型</param>
        /// <param name="value">参数值</param>
        public void AddInParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
        {
            if (cmd != null)
            {
                DbParameter dbParameter = cmd.CreateParameter();

                dbParameter.DbType = dbType;
                dbParameter.ParameterName = parameterName;
                dbParameter.Value = value;
                dbParameter.Direction = ParameterDirection.Input;

                cmd.Parameters.Add(dbParameter);
            }
        }

        /// <summary>
        /// 把返回参数添加到DbCommand对象中
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数的类型</param>
        public void AddReturnParameter(DbCommand cmd, string parameterName, DbType dbType)
        {
            if (cmd != null)
            {
                DbParameter dbParameter = cmd.CreateParameter();

                dbParameter.DbType = dbType;
                dbParameter.ParameterName = parameterName;
                dbParameter.Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.Add(dbParameter);
            }
        }

        /// <summary>
        /// 根据参数名称从DbCommand对象中获取相应的参数对象
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="parameterName">参数名称</param>
        public DbParameter GetParameter(DbCommand cmd, string parameterName)
        {
            if (cmd != null && cmd.Parameters.Count > 0)
            {
                DbParameter param = cmd.Parameters[parameterName];

                return param;
            }

            return null;
        }
        #endregion

        #region 执行SQL脚本语句
        /// <summary>
        /// 执行相应的SQL命令，返回一个DataSet数据集合
        /// </summary>
        /// <param name="sqlQuery">需要执行的SQL语句</param>
        /// <returns>返回一个DataSet数据集合</returns>
        public static DataSet ExecuteDataSet(string sqlQuery)
        {
            using (DbCommand cmd = new Command().GetDbCommand(sqlQuery))
            {
                return ExecuteDataSet(cmd);
            }
        }

        /// <summary>
        /// 执行相应的SQL命令，返回一个DataTable数据集
        /// </summary>
        /// <param name="sqlQuery">需要执行的SQL语句</param>
        /// <returns>返回一个DataTable数据集</returns>
        public static DataTable ExecuteDataTable(string sqlQuery)
        {
            using (DbCommand cmd = new Command().GetDbCommand(sqlQuery))
            {
                return ExecuteDataTable(cmd);
            }
        }

        /// <summary>
        /// 执行相应的SQL命令，返回一个DbDataReader数据对象，如果没有则返回null值
        /// </summary>
        /// <param name="sqlQuery">需要执行的SQL命令</param>
        /// <returns>返回一个DbDataReader数据对象，如果没有则返回null值</returns>
        public static DbDataReader ExecuteReader(string sqlQuery)
        {
            using (DbCommand cmd = new Command().GetDbCommand(sqlQuery))
            {
                return ExecuteReader(cmd);
            }
        }

        /// <summary>
        /// 执行相应的SQL命令，返回影响的数据记录数，如果不成功则返回-1
        /// </summary>
        /// <param name="sqlQuery">需要执行的SQL命令</param>
        /// <returns>返回影响的数据记录数，如果不成功则返回-1</returns>
        public static int ExecuteNonQuery(string sqlQuery)
        {
            using (DbCommand cmd = new Command().GetDbCommand(sqlQuery))
            {
                return ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// 执行相应的SQL命令，返回结果集中的第一行第一列的值，如果不成功则返回null值
        /// </summary>
        /// <param name="sqlQuery">需要执行的SQL命令</param>
        /// <returns>返回结果集中的第一行第一列的值，如果不成功则返回null值</returns>
        public static object ExecuteScalar(string sqlQuery)
        {
            using (DbCommand cmd = new Command().GetDbCommand(sqlQuery))
            {
                return ExecuteScalar(cmd);
            }
        }

        #endregion

        #region 执行DbCommand命令
        /// <summary>
        /// 执行相应的命令，返回一个DataSet数据集合
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回一个DataSet数据集合</returns>
        public static DataSet ExecuteDataSet(DbCommand cmd)
        {
            DataSet ds = new DataSet();
            if (cmd != null)
            {
                using (DbDataAdapter dbDataAdapter = DbServices.dbProviderFactory.CreateDataAdapter())
                {
                    dbDataAdapter.SelectCommand = cmd;
                    dbDataAdapter.Fill(ds);
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行相应的命令，返回一个DataTable数据集合
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回一个DataTable数据集合</returns>
        public static DataTable ExecuteDataTable(DbCommand cmd)
        {
            DataTable dataTable = new DataTable();
            if (cmd != null)
            {
                using (DbDataAdapter dbDataAdapter = DbServices.dbProviderFactory.CreateDataAdapter())
                {
                    dbDataAdapter.SelectCommand = cmd;
                    dbDataAdapter.Fill(dataTable);
                }
            }
            return dataTable;
        }

        /// <summary>
        /// 执行相应的命令，返回一个DbDataReader数据对象，如果没有则返回null值
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回一个DbDataReader数据对象，如果没有则返回null值</returns>
        public static DbDataReader ExecuteReader(DbCommand cmd)
        {
            if (cmd != null && cmd.Connection != null)
            {
                DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);//当reader读取结束时自动关闭数据库链接
                return reader;
            }

            return null;
        }

        /// <summary>
        /// 执行相应的命令，返回影响的数据记录数，如果不成功则返回-1
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回影响的数据记录数，如果不成功则返回-1</returns>
        public static int ExecuteNonQuery(DbCommand cmd)
        {
            if (cmd != null && cmd.Connection != null)
            {
                return cmd.ExecuteNonQuery();
            }
            return -1;
        }

        /// <summary>
        /// 执行相应的命令，返回结果集中的第一行第一列的值，如果不成功则返回null值
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回结果集中的第一行第一列的值，如果不成功则返回null值</returns>
        public static object ExecuteScalar(DbCommand cmd)
        {
            if (cmd != null && cmd.Connection != null)
            {
                return cmd.ExecuteScalar();
            }
            return null;
        }
        #endregion

        #region 执行DbTransaction事务
        /// <summary>
        /// 以事务的方式执行相应的命令，返回一个DataSet数据集合
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回一个DataSet数据集合</returns>
        public DataSet ExecuteDataSet(DbCommand cmd, Trans trans)
        {
            DataSet ds = new DataSet();

            if (cmd != null)
            {
                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;
                using (DbDataAdapter dbDataAdapter = DbServices.dbProviderFactory.CreateDataAdapter())
                {
                    dbDataAdapter.SelectCommand = cmd;
                    dbDataAdapter.Fill(ds);
                }
            }

            return ds;
        }

        /// <summary>
        /// 以事务的方式执行相应的命令，返回一个DataTable数据集合
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回一个DataTable数据集合</returns>
        public DataTable ExecuteDataTable(DbCommand cmd, Trans trans)
        {
            DataTable dataTable = new DataTable();

            if (cmd != null)
            {
                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;
                using (DbDataAdapter dbDataAdapter = DbServices.dbProviderFactory.CreateDataAdapter())
                {
                    dbDataAdapter.SelectCommand = cmd;
                    dbDataAdapter.Fill(dataTable);
                }

            }
            return dataTable;
        }

        /// <summary>
        /// 以事务的方式执行相应的命令，返回一个DbDataReader数据对象，如果没有则返回null值
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回一个DbDataReader数据对象，如果没有则返回null值</returns>
        public DbDataReader ExecuteReader(DbCommand cmd, Trans trans)
        {
            if (cmd != null)
            {
                cmd.Connection.Close();

                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;

                DbDataReader reader = cmd.ExecuteReader();

                return reader;
            }
            return null;
        }

        /// <summary>
        /// 以事务的方式执行相应的命令，返回影响的数据记录数，如果不成功则返回-1
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回影响的数据记录数，如果不成功则返回-1</returns>
        public int ExecuteNonQuery(DbCommand cmd, Trans trans)
        {
            if (cmd != null)
            {
                cmd.Connection.Close();
                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;
                int retVal = cmd.ExecuteNonQuery();
                return retVal;
            }
            return -1;
        }

        /// <summary>
        /// 以事务的方式执行相应的命令，返回结果集中的第一行第一列的值，如果不成功则返回null值
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回结果集中的第一行第一列的值，如果不成功则返回null值</returns>
        public object ExecuteScalar(DbCommand cmd, Trans trans)
        {
            if (cmd != null)
            {
                cmd.Connection.Close();
                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;
                object retVal = cmd.ExecuteScalar();
                return retVal;
            }
            return null;
        }
        #endregion
        #endregion
    }

    /// 说 明: 数据库事务操作对象
    public sealed class Trans : IDisposable
    {
        #region 字段属性
        private DbConnection connection = null;
        /// <summary>
        /// 获取当前数据库链接对象
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                return this.connection;
            }
        }

        private DbTransaction transaction = null;
        /// <summary>
        /// 获取当前数据库事务对象
        /// </summary>
        public DbTransaction Transaction
        {
            get
            {
                return this.transaction;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 根据配置的数据库提供程序和连接字符串来创建此事务对象
        /// </summary>
        //public Trans()
        //    : this(DbHelper.DbConnectionString, DbHelper.DbProviderName)
        //{
        //}

        /// <summary>
        /// 根据数据库连接字符串来创建此事务对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="dbProviderName">数据库提供程序的名称</param>
        public Trans(string connectionString, string dbProviderName)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                this.connection = DbHelper.CreateDbConnection(connectionString, dbProviderName);
                this.Connection.Open();

                this.transaction = this.Connection.BeginTransaction();
            }
            else
            {
                throw new ArgumentNullException("connectionString", "数据库链接串参数值不能为空!");
            }
        }
        #endregion

        #region 方法函数
        /// <summary>
        /// 提交此数据库事务操作
        /// </summary>
        public void Commit()
        {
            this.Transaction.Commit();

            this.Close();
        }

        /// <summary>
        /// 回滚此数据库事务操作
        /// </summary>
        public void RollBack()
        {
            this.Transaction.Rollback();

            this.Close();
        }

        /// <summary>
        /// 关闭此数据库事务链接
        /// </summary>
        public void Close()
        {
            if (this.Connection.State != System.Data.ConnectionState.Closed)
            {
                this.Connection.Close();
            }
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }
        #endregion
    }
}
