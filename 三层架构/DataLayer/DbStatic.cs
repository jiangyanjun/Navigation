namespace DataLayer
{
    public class DbStatic
    {
        public const string U_Url_ListTable = "U_Url_List";
        public const string SelectForm = "SELECT {0} FROM {1} WHERE 1=1 {2}";
        public const string UpdateForm = "UPDATE {0} SET {1} WHERE ID='{2}'";
        public const string InsertForm = "INSERT INTO {0} ({1}) VALUES({2})";
        public const string DeleteForm = "DELETE {0} WHERE {1}";
    }
    #region U_Url_List
    public class UrlListTb
    {
        public const string SelectForm = "SELECT A.ID,A.NAME,A.URL,A.IconImg,A.SortDesc,A.Source,A.Title,A.Status,B.NAME AS Types,A.Create_Id,A.Create_Time,A.LastUpdate_Id,A.LastUpdate_Time,A.IcomStream FROM U_Url_List A LEFT JOIN U_Url_Type B ON A.Types=B.ID WHERE B.STATUS=1 {0}";
        public const string TableName = "U_Url_List";
        public const string ColumnName = "ID,NAME,URL,IconImg,SortDesc,Source,Title,Status,Types,Create_Id,Create_Time,LastUpdate_Id,LastUpdate_Time,IcomStream";
        public const string _ColumnName = "@Id,@Name,@Url,@IconImg,@SortDesc,@Source,@Title,@Status,@Types,@Create_Id,@Create_Time,@LastUpdate_Id,@LastUpdate_Time,@IcomStream";
    }
    #endregion

    #region S_UserInfo
    public class S_UserInfoTb
    {
        public const string TableName = "S_UserInfo";
        public const string ColumnName = "Id,UserAccount,Password";
        public const string _ColumnName = "@Id,@UserAccount,@Password";
    }
    #endregion  
    #region U_Url_Type
    public class TypeTb
    {
        public const string TableName = "U_Url_Type";
        public const string ColumnName = "ID,NAME,SortDesc,STATUS,ParentID,Create_Id,Create_Time,LastUpdate_Id,LastUpdate_Time";
        public const string _ColumnName = "@Id,@Name,@SortDesc,@Status,@ParentID,@Create_Id,@Create_Time,@LastUpdate_Id,@LastUpdate_Time";
    }
    #endregion
    #region U_Url_Type
    public class U_Url_ClickRateTb
    {
        public const string TableName = "U_Url_ClickRate";
        public const string ColumnName = "Id,UrlId,UserAgents,Ip,Msg,ClickDate";
        public const string _ColumnName = "@Id,@UrlId,@UserAgents,@Ip,@Msg,@ClickDate";
    }
    #endregion

    #region U_Url_Check
    public class U_Url_CheckTb
    {
        public const string TableName = "U_Url_Check";
        public const string ColumnName = "Id,UrlId,Url,Result,Create_Time,Webstate,Msg";
        public const string _ColumnName = "@Id,@UrlId,@Url,@Result,@Create_Time,@Webstate,@Msg";
    }
    #endregion

    #region S_Log
    public class S_LogTb
    {
        public const string TableName = "S_Log";
        public const string ColumnName = "Id,Log_Type,Msg,Module,Category,SubCategory,Create_Time";
        public const string _ColumnName = "@Id,@Log_Type,@Msg,@Module,@Category,@SubCategory,@Create_Time";
    }
    #endregion

    #region S_Config
    public class S_ConfigTb
    {
        public const string TableName = "S_Config";
        public const string ColumnName = "Id,Types,Keys,Value,Create_Id,Create_Time,LastUpdate_Id,LastUpdate_Time";
        public const string _ColumnName = "@Id,@Types,@Keys,@Value,@Create_Id,@Create_Time,@LastUpdate_Id,@LastUpdate_Time";
    }
    #endregion

    #region S_Config
    public class LeaveAMessageTb
    {
        public const string TableName = "LeaveAMessage";
        public const string ColumnName = "Id,Name,Content,UserAgent,Create_Time";
        public const string _ColumnName = "@Id,@Name,@Content,@UserAgent,@Create_Time";
    }
    #endregion

    #region S_Menu
    public class S_MenuTb
    {
        public const string TableName = "S_Menu";
        public const string ColumnName = "Id,Name,Url,Types,ParentId,Sort";
        public const string _ColumnName = "@Id,@Name,@Url,@Types,@ParentId,@Sort";
    }
    #endregion
}










