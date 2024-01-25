using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Bulky.Utility
{
public class EmailSender : IEmailSender {

        public string SendGridSecret {get; set;}

        public EmailSender(IConfiguration _config) {
            SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey");
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage) {
            var client = new SendGridClient(SendGridSecret);
            var from = new EmailAddress("jason@prismiq.co.uk", "Book Store");
            var to = new EmailAddress(email);
            var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            return client.SendEmailAsync(message);
        }
    }
}