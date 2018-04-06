using System.Data;
using System.Data.Common;

namespace Database
{
    public class Command
    {
        public DbCommand GetDbCommand(string sqlQuery, int timeOut = 60, CommandType commandType = CommandType.Text)
        {
            var conn = new DbServices();
            DbCommand dbCmd = conn.Connection.CreateCommand();
            dbCmd.CommandText = sqlQuery;
            dbCmd.CommandType = commandType;
            dbCmd.CommandTimeout = timeOut;
            return dbCmd;
        }
    }
}
