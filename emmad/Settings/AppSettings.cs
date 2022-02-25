namespace emmad.Settings
{
    public class AppSettings
    {
        public string JWTSecretCode { get; set; }
        public string SRV { get; set; }
        public string DB { get; set; }
        public string ACC { get; set; }
        public string EmailFrom { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
    }
}
