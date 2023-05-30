namespace Sales.API.Helpers
{
    public class SendMailConfiguration
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string UrlWEB { get; set; }
        public string Password { get; set; }
    }
}
