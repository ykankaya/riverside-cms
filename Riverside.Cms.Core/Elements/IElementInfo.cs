using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Interface for types that hold element settings and content.
    /// </summary>
    public interface IElementInfo
    {
        IElementSettings Settings { get; set; }
        IElementContent Content { get; set; }
    }

    /// <summary>
    /// Generic equivalent of IElementInfo interface.
    /// </summary>
    /// <typeparam name="TSettings">Type of element settings.</typeparam>
    /// <typeparam name="TContent">Type of element content.</typeparam>
    public interface IElementInfo<TSettings, TContent> : IElementInfo where TSettings : IElementSettings where TContent : IElementContent
    {
        new TSettings Settings { get; set; }
        new TContent Content { get; set; }
    }
}
