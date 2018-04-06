namespace PhysicalLayer
{
    public class U_Url_Type
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int SortDesc { get; set; }
        public int Status { get; set; }
        public string ParentID { get; set; }
        public string Create_Id { get; set; }
        public string Create_Time { get; set; }
        public string LastUpdate_Id { get; set; }
        public string LastUpdate_Time { get; set; }
    }
    public class UrlType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int SortDesc { get; set; }
        public int Status { get; set; }
        public string ParentID { get; set; }
        public long UrlCount { get; set; }
    }
}
