using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace Database
{
    public class DbServices : IDisposable
    {
        private DbConnection connection = null;
        public DbConnection Connection
        {
            get
            {
                return connection;
            }
        }
        public void ConnClose()
        {
            if (connection != null)
                connection.Close();
        }
        public static DbProviderFactory dbProviderFactory { get; set; }

        /// <summary>
        /// 获取连接数据类型
        /// </summary>
        public static string ConnectionDatabaseType = DbType.SqlLite.ToString();
        public static DbType DBTypeConnection
        {
            get
            {
                if (ConnectionDatabaseType == DbType.SqlServer.ToString())
                {
                    return DbType.SqlServer;
                }
                else if (ConnectionDatabaseType == DbType.MySql.ToString())
                {
                    return DbType.MySql;
                }
                else if (ConnectionDatabaseType == DbType.PostgreSQL.ToString())
                {
                    return DbType.PostgreSQL;
                }
                else
                    return DbType.SqlLite;
            }
        }
        public DbServices()
        {
            var config = ConfigurationManager.ConnectionStrings["connectionName"];
            if (connection == null && config != null && !string.IsNullOrEmpty(config.ProviderName) && !string.IsNullOrEmpty(config.ConnectionString))
            {
                switch (DBTypeConnection)
                {
                    #region SqlLite
                    case DbType.SqlLite:
                        connection = new SQLiteConnection(config.ConnectionString);
                        dbProviderFactory = DbProviderFactories.GetFactory(config.ProviderName);
                        connection.Open();
                        break;
                        #endregion
                }
            }
        }

        #region DbType
        public enum DbType
        {
            SqlServer,
            SqlLite,
            MySql,
            PostgreSQL,
            Oracle,
            Aceess
        }
        #endregion
        #region 实现接口IDisposable
        /// <释放资源接口>
        /// 实现接口IDisposable
        /// </释放资源接口>
        public void Dispose()
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)//判断数据库连接池是否打开
                {
                    connection.Close();
                }
                connection.Dispose();//释放连接池资源
                GC.SuppressFinalize(this);//垃圾回收
            }
        }
        #endregion
    }
}
