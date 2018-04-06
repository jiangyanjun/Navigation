using System;

namespace Mmmxa.Entity.Core
{
    public class EntityAttribute : Attribute
    {
        /// <summary>
        /// 实体映射TableName和PrimaryKey
        /// </summary>
        /// <param name="tableName">映射数据库表名</param>
        /// <param name="primaryKey">主键</param>
        public EntityAttribute(string tableName, string primaryKey)
        {
            TableName = tableName;
            PrimaryKey = primaryKey;
        }
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }
    }

    public class ColumnMappingAttribute : Attribute
    {
        /// <summary>
        /// 实体映射TableName和PrimaryKey
        /// </summary>
        /// <param name="tableName">映射数据库表名</param>
        /// <param name="primaryKey">主键</param>
        public ColumnMappingAttribute(string columnName)
        {
            ColumnName = columnName;
        }
        public string ColumnName { get; set; }
    }
}
