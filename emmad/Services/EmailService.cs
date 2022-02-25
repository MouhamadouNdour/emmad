using emmad.Settings;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using emmad.Interface;

namespace emmad.Services
{
    public class EmailService : IEmail
    {
        private readonly AppSettings appSettings;

        public EmailService(IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings.Value;
        }

        public void Send(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? appSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(appSettings.SmtpHost, appSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(appSettings.SmtpUser, appSettings.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
