using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;


namespace NET.DataAccessLayer.Common
{
    public class ConnService : IDisposable
    {
        public static string ConnectionStrPath
        {
            get
            {
                var config =ConfigurationManager.ConnectionStrings["connectionName"];
                return config.ConnectionString;
            }
        }
        public static string ConnectionDatabaseType { get { return "SqlLite"; } }
        public static DbType DBTypeConnection
        {
            get
            {
                if (ConnectionDatabaseType.Contains("SqlServer"))
                {
                    return DbType.SqlServer;
                }
                else if (ConnectionDatabaseType.Contains("MySql"))
                {
                    return DbType.MySql;
                }
                else if (ConnectionDatabaseType.Contains("PostgreSQL"))
                {
                    return DbType.PostgreSQL;
                }
                else
                    return DbType.SqlLite;

            }
        }
        #region 数据库连接对象
        private DbConnection connection = null;
        public DbConnection Connection
        {
            get
            {
                return connection;
            }
        }
        #endregion
        public ConnService()
        {
            if (connection == null)
            {
                switch (DBTypeConnection)
                {
                    case DbType.SqlLite:
                        connection = new SQLiteConnection(ConnectionStrPath);
                        connection.Open();
                        break;
                }
            }
        }
        public void Dispose()
        {
            if (connection != null)
                connection.Close();
        }
        public enum DbType
        {
            SqlServer,
            SqlLite,
            MySql,
            PostgreSQL
        }
    }

}
