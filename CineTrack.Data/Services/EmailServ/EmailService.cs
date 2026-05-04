using System.Net;
using System.Net.Mail;


namespace CineTrack.Data.Services.EmailServ
{
    public class EmailService : IEmailService
    {
        private readonly string _mail;
        private readonly string _password;
        private readonly string _smtpHost;
        private readonly int _smtpPort;

        public EmailService(string mail, string password, string smtpHost, int smtpPort)
        {
            _mail = mail;
            _password = password;
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
        }

        public async Task SendPasswordResetCodeAsync(string userEmail, string code)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(_mail, "CineTrack"),
                Subject = "Demande de réinitialisation",
                Body = $"Voici le code de réinitialisation : {code}\n Le code expire 5 minutes après l'envoi de celui-ci.",
                IsBodyHtml = false
            };
            mail.To.Add(userEmail);

            using var smtp = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_mail, _password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
        public async Task SendVerificationCodeAsync(string userEmail, string code)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(_mail, "CineTrack"),
                Subject = "Vérification du compte",
                Body = $"Voici le code de vérification : {code}\n Le code expire dans 5 minutes",
                IsBodyHtml = false
            };
            mail.To.Add(userEmail);

            using var smtp = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_mail, _password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
