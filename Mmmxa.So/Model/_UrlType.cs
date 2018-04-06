using CommonLayer;
using CommonLayer.Enum;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mmmxa.So
{
    public class _UrlType
    {
        public static List<_UrlType> UrlTypeList
        {
            get
            {
                U_Url_TypeDAL bllType = new U_Url_TypeDAL();
                List<_UrlType> list = new List<_UrlType>();
                var result = bllType.GetTypes();
                foreach (var i in result)
                {
                    list.Add(new _UrlType { Id = i.Id, Name = i.Name, ParentID = i.ParentID, SortDesc = i.SortDesc, Status = i.Status.ToEnumDiscription<EnumType>(), UrlCount = i.UrlCount });
                }
                return list;
            }
        }
        public static List<_UrlType> UrlTypeListManager
        {
            get
            {
                U_Url_TypeDAL bllType = new U_Url_TypeDAL();
                List<_UrlType> list = new List<_UrlType>();
                var result = bllType.GetTypeManager();
                foreach (var i in result)
                {
                    list.Add(new _UrlType { Id = i.Id, Name = i.Name, ParentID = i.ParentID, SortDesc = i.SortDesc, Status = i.Status.ToEnumDiscription<EnumType>(), UrlCount = i.UrlCount });
                }
                return list;
            }
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public int SortDesc { get; set; }
        public string Status { get; set; }
        public string ParentID { get; set; }
        public long UrlCount { get; set; }
    }
}