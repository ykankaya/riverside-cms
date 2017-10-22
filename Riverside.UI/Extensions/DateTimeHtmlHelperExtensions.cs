using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Riverside.UI.Extensions
{
    public static class DateTimeHtmlHelperExtensions
    {
        /// <summary>
        /// An HTML helper extension method for returning a date time as a relative time period from the current date and time. For example, "one second ago" or "3 hours ago".
        /// Credit: http://stackoverflow.com/questions/11/how-do-i-calculate-relative-time (Jeff Atwood's answer).
        /// </summary>
        /// <param name="htmlHelper">This is an HtmlHelper extension method.</param>
        /// <param name="dateTime">The date time object whose relative time string representation is returned.</param>
        /// <returns>Date time as a relative time period from the current date and time.</returns>
        public static string RelativeTime(this IHtmlHelper htmlHelper, DateTime dateTime)
        {
            var timeSpan = DateTime.UtcNow - dateTime;
            double delta = timeSpan.TotalSeconds;
            if (delta < 1)
                return "now";
            if (delta < 60)
                return timeSpan.Seconds == 1 ? "one second ago" : timeSpan.Seconds + " seconds ago";
            if (delta < 120)
                return "a minute ago";
            if (delta < 2700)     // 45 * 60
                return timeSpan.Minutes + " minutes ago";
            if (delta < 5400)     // 90 * 60
                return "an hour ago";
            if (delta < 86400)    // 24 * 60 * 60
                return timeSpan.Hours + " hours ago";
            if (delta < 172800)   // 48 * 60 * 60
                return "yesterday";
            if (delta < 2592000)  // 30 * 24 * 60 * 60
                return timeSpan.Days + " days ago";
            if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                int months = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            int years = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }

        /// <summary>
        /// An HTML helper extension for rendering a date in a standard format.
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper object.</param>
        /// <param name="dateTime">A DateTime object whose date component is rendered.</param>
        /// <returns>Date component as string.</returns>
        public static string Date(this IHtmlHelper htmlHelper, DateTime dateTime)
        {
            return dateTime.ToString("d MMMM yyyy");
        }
    }
}
