using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Validation
{
    /// <summary>
    /// Regular expressions used for validation.
    /// </summary>
    public static class RegularExpression
    {
        /// <summary>
        /// Email regular expression.
        /// </summary>
        public const string Email = @"^[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$";

        /// <summary>
        /// URL regular expression.
        /// </summary>
        public const string Url = @"(http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?";

        /// <summary>
        /// HTML DOM identfier. Credit: http://stackoverflow.com/questions/14664860/allowed-html-4-01-id-values-regex
        /// </summary>
        public const string DomIdentifier = @"^[a-zA-Z][\w:.-]*$";

        /// <summary>
        /// HTML CSS class. Credit: http://stackoverflow.com/questions/14664860/allowed-html-4-01-id-values-regex
        /// </summary>
        public const string CssClass = @"^[a-zA-Z][\w:.-]*$";

        /// <summary>
        /// String must not start with or end with white space. Credit: http://stackoverflow.com/questions/17768298/regular-expression-to-match-string-not-starting-with-or-ending-with-spaces
        /// </summary>
        public const string Trimmed = @"^\S(.*\S)?$";
    }
}
