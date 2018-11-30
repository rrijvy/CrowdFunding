using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace CrowdFunding.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("crowdffunding@gmail.com"));
                message.To.Add(new MailboxAddress(email));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = htmlMessage
                };
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("crowdffunding@gmail.com", "agmadmtp");
                    client.Send(message);
                    client.Disconnect(true);
                }
                return Task.CompletedTask;
            }
            throw new NotImplementedException();
        }
    }
}
