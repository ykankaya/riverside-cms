using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Mail
{
    public interface IEmailConfigurationService
    {
        EmailAddress GetFromEmailRecipient();
        EmailAddress GetReplyToEmailRecipient();
        IEnumerable<EmailAddress> GetBccEmailRecipients();
        IEnumerable<EmailAddress> GetToEmailRecipients();
    }
}
