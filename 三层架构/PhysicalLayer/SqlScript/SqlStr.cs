using CommonLayer;

namespace PhysicalLayer
{
    public class SqlStr
    {
        public static string GetHomePageCount
        {
            get
            {
                return string.Format(@"
                    SELECT COUNT(1) AS UrlCount FROM U_URL_CLICKRATE where CLICKDATE like '{0}%' --当日访问                      
                    UNION ALL
                    SELECT COUNT(1) FROM U_URL_CLICKRATE --历史访问
                    UNION ALL
                    SELECT COUNT(L.URL) FROM U_URL_LIST L
                    LEFT JOIN U_URL_TYPE T ON L.TYPES=T.ID
                    WHERE L.STATUS=1 AND T.STATUS=1    --有效网址
                    UNION ALL
                    SELECT COUNT(1) FROM (SELECT DISTINCT T.NAME FROM U_URL_LIST L
                    LEFT JOIN U_URL_TYPE T ON L.TYPES=T.ID
                    WHERE L.STATUS=1 AND T.STATUS=1 )  --有效网址类型", GetStr.GetCurrentDay);
            }
        }
        public static string GetHomeUrlList
        {
            get
            {
                return @"SELECT DISTINCT A.ID,A.NAME,A.URL,A.ICONIMG,A.SORTDESC,A.SOURCE,A.TITLE,A.STATUS,B.NAME AS TYPES,A.CREATE_ID,A.CREATE_TIME,A.LASTUPDATE_ID,A.LASTUPDATE_TIME 
                            FROM U_URL_LIST A 
                            JOIN U_URL_TYPE B ON A.TYPES=B.ID 
                            LEFT JOIN U_URL_CHECK C ON A.ID=C.URLID
                            WHERE A.STATUS=1
                            AND B.ID='{0}'
                            ORDER BY B.SORTDESC DESC,A.SORTDESC DESC,C.WEBSTATE";
            }
        }
        public static string GetmyModalModify
        {
            get
            {
                return @"SELECT DISTINCT
                    U.ID,U.NAME,U.URL,U.Types,U.Title,U.ICONIMG,C.RESULT AS CREATE_TIME
                    FROM U_URL_LIST U
                    LEFT JOIN U_URL_CHECK C ON U.ID = C.URLID
                    WHERE U.ID='{0}'
                    ORDER BY C.Create_Time DESC";
            }
        }

        #region 统计点击率
        public static string GetCuurentClickRate
        {
            get
            {
                return @"SELECT U.URL AS ID,U.NAME AS URLID,C.USERAGENTS,C.CLICKDATE FROM U_URL_CLICKRATE C
                        LEFT JOIN U_URL_LIST U ON C.URLID=U.ID
                        WHERE date(C.CLICKDATE)>='{0}' AND date(C.CLICKDATE)<='{1}'
                        ORDER BY C.CLICKDATE DESC";
            }
        }
        /// <summary>
        /// 指定日统计点击率
        /// </summary>
        public static string GetClickRateDay
        {
            get
            {
                return @"SELECT U.URL,U.NAME,DATE(C.CLICKDATE) AS CLICKDATE,COUNT(*) COUNT FROM  U_URL_CLICKRATE C
                        LEFT JOIN U_URL_LIST U ON C.URLID=U.ID
                        WHERE strftime('%Y-%m-%d',ClickDate)>=strftime('%Y-%m-%d','{0}') AND strftime('%Y-%m-%d',ClickDate)<=strftime('%Y-%m-%d','{1}')
                        GROUP BY U.URL,U.NAME,DATE(C.CLICKDATE)
                        ORDER BY DATE(C.CLICKDATE),COUNT(*) DESC";
            }
        }
        /// <summary>
        /// 指定月份统计点击量
        /// </summary>
        public static string GetClickRateMonth
        {
            get
            {
                return @"SELECT U.URL,U.NAME,STRFTIME('%Y-%m',CLICKDATE) AS CLICKDATE,COUNT(*) COUNT FROM  U_URL_CLICKRATE C
                            LEFT JOIN U_URL_LIST U ON C.URLID=U.ID
                            WHERE STRFTIME('%Y-%m',CLICKDATE)>=STRFTIME('%Y-%m','{0}') AND STRFTIME('%Y-%m',CLICKDATE)<=STRFTIME('%Y-%m','{1}')
                            GROUP BY U.URL,U.NAME,STRFTIME('%Y-%m',CLICKDATE)
                            ORDER BY STRFTIME('%Y-%m',CLICKDATE),COUNT(*) DESC";
            }
        }
        /// <summary>
        /// 指定时间类统计点击量
        /// </summary>
        public static string GetClickRate
        {
            get
            {
                return @"SELECT U.URL,U.NAME,STRFTIME('%Y-%m',CLICKDATE) AS CLICKDATE,COUNT(*) COUNT FROM  U_URL_CLICKRATE C
                            LEFT JOIN U_URL_LIST U ON C.URLID=U.ID
                            WHERE STRFTIME('%Y-%m',CLICKDATE)>=STRFTIME('%Y-%m','{0}') AND STRFTIME('%Y-%m',CLICKDATE)<=STRFTIME('%Y-%m','{1}')
                            GROUP BY U.URL,U.NAME,STRFTIME('%Y-%m',CLICKDATE)
                            ORDER BY STRFTIME('%Y-%m',CLICKDATE),COUNT(*) DESC";
            }
        }
        #endregion
        /// <summary>
        /// 获取网址安全检测SQL
        /// </summary>
        public static string GetCheckResult
        {
            get
            {
                return @"SELECT C.URL,L.NAME URLID,C.WEBSTATE,C.RESULT,C.ID,C.MSG,C.CREATE_TIME
                        FROM U_URL_CHECK C
                        LEFT JOIN U_URL_LIST L ON C.URLID=L.ID
                        WHERE 
                        DATE(C.CREATE_TIME)>=DATE('{0}') AND 
                        DATE(C.CREATE_TIME)<=DATE('{1}')
                        ORDER BY DATE(C.CREATE_TIME) DESC,C.WEBSTATE";
            }
        }
        /// <summary>
        /// 留言管理
        /// </summary>
        public static string LeaveAMessage
        {
            get { return @"SELECT * FROM LEAVEAMESSAGE WHERE STRFTIME('%Y-%m',Create_Time)>=STRFTIME('%Y-%m','{0}') AND STRFTIME('%Y-%m',Create_Time)<=STRFTIME('%Y-%m','{1}')"; }
        }
        /// <summary>
        /// 日志管理
        /// </summary>
        public static string LogSql
        {
            get { return @"SELECT * FROM S_Log  WHERE date(Create_Time)>='{0}' AND date(Create_Time)<='{1}'"; }
        }
    }
}
