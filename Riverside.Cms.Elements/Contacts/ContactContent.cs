using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.Contacts
{
    public class ContactContent : ElementContent
    {
        /// <summary>
        /// E.g. "https://twitter.com/example".
        /// </summary>
        public string TwitterUrl { get; set; }

        /// <summary>
        /// E.g. "https://www.facebook.com/example".
        /// </summary>
        public string FacebookUrl { get; set; }

        /// <summary>
        /// E.g. "https://www.linkedin.com/in/example".
        /// </summary>
        public string LinkedInPersonalUrl { get; set; }

        /// <summary>
        /// E.g. "https://www.linkedin.com/company/example".
        /// </summary>
        public string LinkedInCompanyUrl { get; set; }

        /// <summary>
        /// E.g. "https://www.instagram.com/example".
        /// </summary>
        public string InstagramUrl { get; set; }

        /// <summary>
        /// E.g. "https://www.youtube.com/channel/example".
        /// </summary>
        public string YouTubeChannelUrl { get; set; }
    }
}
