using CommonLayer;
using CommonLayer.Enum;
using DataLayer;
using PhysicalLayer;
using System;
using System.Threading.Tasks;

namespace Business
{
    public class S_LogBLL : BaseBLL<S_Log>
    {
        public override void SetCurrentDAL()
        {
            base.CurrentDAL = new S_LogDAL();
        }
        /// <param name="msg">记录的消息</param>
        /// <param name="logType">日志类型</param>
        /// <param name="module">模块</param>
        /// <param name="category">目录</param>
        /// <param name="subCategory">子目录</param>
        public void AddLog(string msg, EnumLogType logType, string module = null, string category = null, string subCategory = null)
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
                    log.Category = category;
                    log.SubCategory = subCategory;
                    new S_LogDAL().Add(new System.Collections.Generic.List<S_Log> { log });
                }
                catch { }
            });
        }
        /// <param name="ex">Exception</param>
        /// <param name="module">模块</param>
        /// <param name="category">目录</param>
        /// <param name="subCategory">子目录</param>
        public void AddLog(Exception ex, string module = null, string category = null, string subCategory = null)
        {
            AddLog(string.Format("StackTrace：{0}，Exception：{1}", ex.StackTrace, ex.ToString()), EnumLogType.Erro, module, category, subCategory);
        }

        /// <param name="ex">Exception</param>
        /// <param name="msg">记录的消息</param>
        /// <param name="module">模块</param>
        /// <param name="category">目录</param>
        /// <param name="subCategory">子目录</param>
        public void AddLog(Exception ex, string msg, string module = null, string category = null, string subCategory = null)
        {
            AddLog(string.Format("Result Msg：{0}，Exception：【{{1}}】，StackTrace：{2}", msg, ex.ToString(), ex.StackTrace), EnumLogType.Erro, module, category, subCategory);
        }
        public void AddLog(Exception ex, string msg)
        {
            AddLog(string.Format("Result Msg：{0}，Exception：【{{1}}】，StackTrace：{2}", msg, ex.ToString(), ex.StackTrace), EnumLogType.Erro);
        }
    }
}
