using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.Contacts
{
    public class ContactSettings : ElementSettings
    {
        /// <summary>
        /// Display name is a header that is rendered at the top of contact element. Can be left null if a header is not required.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// If set, preamble text rendered above contact information. Can be left null if not required.
        /// </summary>
        public string Preamble { get; set; }

        /// <summary>
        /// Twitter username (e.g. "example").
        /// </summary>
        public string TwitterUsername { get; set; }

        /// <summary>
        /// Facebook username (e.g. "example").
        /// </summary>
        public string FacebookUsername { get; set; }

        /// <summary>
        /// Company LinkedIn username (e.g. "example").
        /// </summary>
        public string LinkedInCompanyUsername { get; set; }

        /// <summary>
        /// Personal LinkedIn username (e.g. "example").
        /// </summary>
        public string LinkedInPersonalUsername { get; set; }

        /// <summary>
        /// Instagram username (e.g. "example").
        /// </summary>
        public string InstagramUsername { get; set; }

        /// <summary>
        /// Contact email address (e.g. "hello@example.com").
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// First telephone number.
        /// </summary>
        public string TelephoneNumber1 { get; set; }

        /// <summary>
        /// Second telephone number.
        /// </summary>
        public string TelephoneNumber2 { get; set; }

        /// <summary>
        /// YouTube channel identifier (e.g. "example").
        /// </summary>
        public string YouTubeChannelId { get; set; }
    }
}
