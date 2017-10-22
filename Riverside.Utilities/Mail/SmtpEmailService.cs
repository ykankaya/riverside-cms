using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Microsoft.Extensions.Options;

namespace Riverside.Utilities.Mail
{
    public class SmtpEmailService : IEmailService
    {
        // Get access to options
        private IOptions<EmailOptions> _options;

        public SmtpEmailService(IOptions<EmailOptions> options)
        {
            _options = options;
        }

        public void SendEmail(Email email)
        {
            using (SmtpClient client = new SmtpClient())
            {
                client.Host = _options.Value.EmailHost;
                if (_options.Value.EmailPort.HasValue)
                    client.Port = _options.Value.EmailPort.Value;
                if (_options.Value.EmailUsername != null && _options.Value.EmailPassword != null)
                    client.Credentials = new NetworkCredential(_options.Value.EmailUsername, _options.Value.EmailPassword);
                using (MailMessage mailMessage = new MailMessage())
                {
                    if (email.ReplyToAddress != null)
                        mailMessage.ReplyToList.Add(new MailAddress(email.ReplyToAddress.Email, email.ReplyToAddress.DisplayName));
                    mailMessage.From = new MailAddress(email.FromAddress.Email, email.FromAddress.DisplayName);
                    mailMessage.Subject = email.Content.Subject;
                    mailMessage.Body = email.Content.PlainTextBody != null ? email.Content.PlainTextBody : email.Content.HtmlBody;
                    mailMessage.IsBodyHtml = email.Content.PlainTextBody == null;
                    if (email.Content.PlainTextBody != null && email.Content.HtmlBody != null)
                    {
                        ContentType mimeType = new ContentType("text/html");
                        AlternateView alternate = AlternateView.CreateAlternateViewFromString(email.Content.HtmlBody, mimeType);
                        mailMessage.AlternateViews.Add(alternate);
                    }
                    foreach (EmailAddress toAddress in email.ToAddresses)
                        mailMessage.To.Add(new MailAddress(toAddress.Email, toAddress.DisplayName));
                    if (email.BccAddresses != null)
                        foreach (EmailAddress bccAddress in email.BccAddresses)
                            mailMessage.Bcc.Add(new MailAddress(bccAddress.Email, bccAddress.DisplayName));
                    client.Send(mailMessage);
                }
            }
        }
    }
}
