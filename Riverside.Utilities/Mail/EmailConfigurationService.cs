using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Mail
{
    public class EmailConfigurationService : IEmailConfigurationService
    {
        // Get access to options
        private IOptions<EmailOptions> _options;

        /// <summary>
        /// Default constructor sets default application setting keys that should hold recipient details.
        /// </summary>
        public EmailConfigurationService(IOptions<EmailOptions> options)
        {
            _options = options;
        }

        /// <summary>
        /// Converts a string like "Mike (testtesttest@example.com)" into a display name and email address ("Mike", "testtesttest@example.com").
        /// If string is just an email address like "testtesttest@example.com", then this is returned as the email address with display name null.
        /// </summary>
        /// <param name="recipient">A string like "Mike (testtesttest@example.com)".</param>
        /// <returns>An email address object populated with display name and email.</returns>
        private EmailAddress GetRecipientAddress(string recipient)
        {
            // Get rid of any leading or trailing white space
            recipient = recipient.Trim();

            // Find the positions of the opening and closing brackets
            int startIndex = recipient.IndexOf('(');
            int stopIndex = startIndex != -1 ? recipient.IndexOf(')', startIndex) : -1;
            bool bracketsFound = startIndex != -1 && stopIndex != -1;

            // Email address will be between brackets or the entire string
            string email = bracketsFound ? recipient.Substring(startIndex + 1, stopIndex - startIndex - 1).Trim() : recipient;

            // Display name is everything before the first opening bracket or null if no brackets found
            string displayName = bracketsFound ? recipient.Substring(0, startIndex).Trim() : null;

            // Return result
            return new EmailAddress { DisplayName = displayName, Email = email };
        }

        /// <summary>
        /// Converts recipients string in the format "Mike (mike@example.com);Chris (chris@example.com)" into list of EmailAddress objects.
        /// </summary>
        /// <param name="recipients">Recipients.</param>
        /// <returns>List of EmailAddress objects.</returns>
        private List<EmailAddress> GetRecipientAddresses(string recipients)
        {
            List<EmailAddress> emailAddresses = new List<EmailAddress>();
            if (!string.IsNullOrWhiteSpace(recipients))
            {
                string[] recipientsArray = recipients.Split(';');
                foreach (string recipient in recipientsArray)
                {
                    emailAddresses.Add(GetRecipientAddress(recipient));
                }
            }
            return emailAddresses;
        }

        /// <summary>
        /// Gets from recipient details from application setting "EmailFromAddress".
        /// </summary>
        /// <returns>From recipient email address and display name (or null if application setting "EmailFromAddress" not found).</returns>
        public EmailAddress GetFromEmailRecipient()
        {
            if (_options.Value.EmailFromAddress != null)
                return GetRecipientAddress(_options.Value.EmailFromAddress);
            return null;
        }

        /// <summary>
        /// Gets reply to recipient details from application setting "EmailReplyToAddress".
        /// </summary>
        /// <returns>Reply to recipient email address and display name (or null if application setting "EmailReplyToAddress" not found).</returns>
        public EmailAddress GetReplyToEmailRecipient()
        {
            if (_options.Value.EmailReplyToAddress != null)
                return GetRecipientAddress(_options.Value.EmailReplyToAddress);
            return null;
        }

        /// <summary>
        /// Gets BCC recipients from application setting "EmailBccAddresses".
        /// </summary>
        /// <returns>BCC email addresses and display names (or null if application setting "EmailBccAddresses" not found).</returns>
        public IEnumerable<EmailAddress> GetBccEmailRecipients()
        {
            if (_options.Value.EmailBccAddresses != null)
                return GetRecipientAddresses(_options.Value.EmailBccAddresses);
            return null;
        }

        /// <summary>
        /// Gets to recipients from application setting "EmailToAddresses".
        /// </summary>
        /// <returns>To email addresses and display names (or null if application setting "EmailToAddresses" not found).</returns>
        public IEnumerable<EmailAddress> GetToEmailRecipients()
        {
            if (_options.Value.EmailToAddresses != null)
                return GetRecipientAddresses(_options.Value.EmailToAddresses);
            return null;
        }
    }
}
