namespace SjxLogistics.Models
{
    public class AccessConfig
    {
        public string AccessKey { get; set; }
        public int Expiration { get; set; }
        public string Issuer { get; set; }
        public string Audiance { get; set; }
        public double RefreshExpiration { get; set; }
        public string RefreshAccessKey { get; set; }
    }
}
