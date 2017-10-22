using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Administration;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Flags element content as supporting the display of administration options.
    /// </summary>
    public interface IAdministrationContent
    {
        /// <summary>
        /// Administration options.
        /// </summary>
        IAdministrationOptions Options { get; set; }
    }
}
