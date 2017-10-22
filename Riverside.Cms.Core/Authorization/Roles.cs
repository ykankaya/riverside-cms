using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Authorization
{
    /// <summary>
    /// The different authorization roles.
    /// </summary>
    public class Roles
    {
        public const string User = "User";                      // Regular user
        public const string Editor = "Editor";                  // Editor can update page content
        public const string EditorInChief = "EditorInChief";    // Editor can update page and site content
        public const string Administrator = "Administrator";    // Administrator can upadte page and site content and site master pages
    }
}
