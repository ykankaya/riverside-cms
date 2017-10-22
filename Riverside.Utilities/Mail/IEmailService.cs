using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Mail
{
    public interface IEmailService
    {
        void SendEmail(Email email);
    }
}
