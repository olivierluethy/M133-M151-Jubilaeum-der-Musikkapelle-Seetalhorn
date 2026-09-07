using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Services
{
    // Versendet E-Mails per SMTP mit MailKit. Die Zugangsdaten stammen aus den
    // MailSettings (Host/Port/Absender aus der Konfiguration, Passwort aus
    // User Secrets).
    public class SmtpEmailSender : IEmailSender
    {
        private readonly MailSettings _settings;

        public SmtpEmailSender(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.DisplayName, _settings.Mail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.Mail, _settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
