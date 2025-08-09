using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace WebApiDotNet.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string htmlBody);
    }

    public sealed class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            var host = _config["Email:SmtpHost"];
            var port = int.Parse(_config["Email:SmtpPort"] ?? "587");
            var enableSsl = bool.Parse(_config["Email:EnableSsl"] ?? "true");
            var username = _config["Email:Username"];
            var password = _config["Email:Password"];
            var from = _config["Email:From"] ?? username;

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                Credentials = new NetworkCredential(username, password)
            };
            var mail = new MailMessage(from, to)
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            await client.SendMailAsync(mail);
        }
    }
}

