using listening.Security;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace listening.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private const string EmainSiteName = "Listening Site";
        private const string mailAddressServer = "smtp.gmail.com";
        private const int mailPortServer = 465;

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var myEmail = SecurityRulesSingleton.Instance.Rules.EmailSiteName;
            var myPassword = SecurityRulesSingleton.Instance.Rules.EmailPassword;
            var credentials = new NetworkCredential
            {
                UserName = myEmail,
                Password = myPassword
            };

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(EmainSiteName, myEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(mailAddressServer, mailPortServer, true).ConfigureAwait(false);
                await client.AuthenticateAsync(credentials);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
