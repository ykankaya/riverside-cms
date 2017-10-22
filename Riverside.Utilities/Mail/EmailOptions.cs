using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Mail
{
    public class EmailOptions
    {
        public string EmailBccAddresses { get; set; }
        public string EmailFromAddress { get; set; }
        public string EmailReplyToAddress { get; set; }
        public string EmailToAddresses { get; set; }
        public int? EmailPort { get; set; }
        public string EmailHost { get; set; }
        public string EmailUsername { get; set; }
        public string EmailPassword { get; set; }
    }
}
