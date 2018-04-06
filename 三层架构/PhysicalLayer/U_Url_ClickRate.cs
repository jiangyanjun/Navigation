namespace PhysicalLayer
{
    public class U_Url_ClickRate
    {
        public string Id { get; set; }
        public string UrlId { get; set; }
        public string UserAgents { get; set; }
        public string Ip { get; set; }
        public string Msg { get; set; }
        public string ClickDate { get; set; }
    }
    public class _ClickRate
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public long Count { get; set; }
        public string ClickDate { get; set; }
    }
}
