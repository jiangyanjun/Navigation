using CommonLayer;
using CommonLayer.Enum;
using PhysicalLayer;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Kebue.UI.Models
{
    public class LogHelper
    {
        /// <param name="msg">错误消息</param>
        /// <param name="logType">日志类型</param>
        /// <param name="module">发生错误文件路径</param>
        /// <param name="methodName">发生错误方法名</param>
        /// <param name="LineNo">发生错误行号</param>
        public static void AddLog(string msg, EnumLogType logType, [CallerFilePath]string module = null, [CallerMemberName]string methodName = null, [CallerLineNumber]int LineNo = 0)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    S_Log log = new S_Log();
                    log.Create_Time = GetStr.GetCurrentDate;
                    log.Id = GetStr.GetGuid;
                    log.Log_Type = logType.ToEnumDiscription();
                    log.Msg = msg;
                    log.Module = module;
                    log.Category = methodName;
                    log.SubCategory = LineNo + " 行";
                    HttpHelp.Post<int, S_Log>(ActionEnum.Add_Log, log);
                }
                catch { }
            });
        }
        /// <param name="ex">Exception</param>
        /// <param name="msg">记录的消息</param>
        /// <param name="module">模块</param>
        /// <param name="methodName">目录</param>
        /// <param name="subCategory">子目录</param>
        public static void AddLogMsg(Exception ex,string msg,[CallerFilePath]string module = null, [CallerMemberName]string methodName = null, [CallerLineNumber]int LineNo = 0)
        {
            AddLog(string.Format("Result Msg：{0}，Exception：【{{1}}】，StackTrace：{2}", msg, ex.ToString(), ex.StackTrace), EnumLogType.Erro, module, methodName, LineNo);
        }
        /// <param name="ex">Exception</param>
        /// <param name="module">模块</param>
        /// <param name="methodName">目录</param>
        /// <param name="subCategory">子目录</param>
        public static void AddLog(Exception ex,[CallerFilePath]string module = null, [CallerMemberName]string methodName = null, [CallerLineNumber]int LineNo = 0)
        {
            AddLog(string.Format("StackTrace：{0}，Exception：{1}", ex.StackTrace, ex.ToString()), EnumLogType.Erro, module, methodName, LineNo);
        }

    }
}