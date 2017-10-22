using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Mail
{
    public class Email
    {
        public EmailAddress FromAddress { get; set; }
        public EmailAddress ReplyToAddress { get; set; }
        public IEnumerable<EmailAddress> BccAddresses { get; set; }
        public IEnumerable<EmailAddress> ToAddresses { get; set; }
        public EmailContent Content { get; set; }
    }
}
