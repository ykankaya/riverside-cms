using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RiversideCms.Mvc.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string UrlFriendly(this IUrlHelper urlHelper, string text)
        {
            return UrlUtils.UrlFriendly(text);
        }
    }
}
