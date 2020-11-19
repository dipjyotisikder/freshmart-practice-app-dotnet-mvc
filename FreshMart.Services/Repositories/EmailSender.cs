using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FreshMart.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        //        public Task SendEmailAsync(string email, string subject, string message)
        //        {
        //
        //
        //
        //            return Task.CompletedTask;
        //        }


        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("yoursmtpserver")
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("yourusername", "yourpassword")
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("account-security-noreply@yourdomain.com")
            };
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = htmlMessage;
            return client.SendMailAsync(mailMessage);
        }


    }
}
