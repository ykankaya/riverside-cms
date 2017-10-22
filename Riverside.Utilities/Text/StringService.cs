using Riverside.Utilities.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Text
{
    public class StringService : IStringService
    {
        // Member variables
        private IEncodeDecodeService _encodeDecodeService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="encodeDecodeService">Used to HTML encode text strings.</param>
        public StringService(IEncodeDecodeService encodeDecodeService)
        {
            _encodeDecodeService = encodeDecodeService;
        }

        /// <summary>
        /// Replaces keywords found in provided text with dynamic text.
        /// </summary>
        /// <param name="text">The text whose keywords are to be substituted.</param>
        /// <param name="substitutions">Keyword substitutions. Keys found in text are replaced with the corresponding key values.</param>
        /// <param name="htmlEncode">Set true to HTML encode a substitution value.</param>
        /// <returns>The specified text with keywords replaced with keyword values.</returns>
        public string SubstituteKeywords(string text, IEnumerable<KeyValuePair<string, string>> substitutions, bool htmlEncode)
        {
            if (text != null)
            {
                foreach (KeyValuePair<string, string> substitution in substitutions)
                {
                    string value = substitution.Value;
                    if (htmlEncode)
                        value = _encodeDecodeService.HtmlEncode(value);
                    text = text.Replace(substitution.Key, value);
                }
            }
            return text;
        }

        /// <summary>
        /// Converts text to byte array.
        /// </summary>
        /// <param name="text">The text to convert.</param>
        /// <returns>The resuting byte array.</returns>
        public byte[] GetBytes(string text)
        {
            return Convert.FromBase64String(text);
        }

        /// <summary>
        /// Converts byte array to text.
        /// </summary>
        /// <param name="bytes">Byte array to convert.</param>
        /// <returns>String representation of bytes.</returns>
        public string GetString(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Compares two byte arrays. Returns true if their contents are the same.
        /// </summary>
        /// <param name="byteArray1">Byte array 1.</param>
        /// <param name="byteArray2">Byte array 2.</param>
        /// <returns>True if arrays equal (i.e. same length and containing the same bytes).</returns>
        public bool ByteArraysEqual(byte[] byteArray1, byte[] byteArray2)
        {
            // Byte arrays are equal if references are the same
            if (byteArray1 == byteArray2)
                return true;

            // Byte arrays are not equal if one or other array is null
            if (byteArray1 == null || byteArray2 == null)
                return false;

            // Byte arrays are not equal if lengths are not the same
            if (byteArray1.Length != byteArray2.Length)
                return false;

            // Inspect individual bytes to determine if byte arrays are the same
            for (int index = 0; index < byteArray1.Length; index++)
                if (byteArray1[index] != byteArray2[index])
                    return false;

            //  Byte arrays must be equal
            return true;
        }

        /// <summary>
        /// Retrieve CSV values from single line of text.
        /// </summary>
        /// <param name="text">Line from CSV file.</param>
        /// <returns>Individual values.</returns>
        public List<string> GetCsvValues(string text)
        {
            List<string> values = new List<string>();
            while (!string.IsNullOrEmpty(text))
            {
                if (text[0] == '\"')
                {
                    // Value starts with double quote, so find closing double quote
                    int closeIndex = 1;
                    bool finished = false;
                    while (!finished)
                    {
                        closeIndex = text.IndexOf("\"", closeIndex);
                        if (closeIndex < 0)
                            throw new InvalidOperationException();
                        int nonCloseIndex = text.IndexOf("\"\"", closeIndex);
                        bool invalidClose = closeIndex == nonCloseIndex;
                        finished = !invalidClose;
                        if (!finished)
                            closeIndex = closeIndex + 2;
                    }

                    // Get value
                    values.Add(text.Substring(1, closeIndex - 1).Replace("\"\"", "\""));

                    // Start with next value
                    text = text.Substring(closeIndex + 1);

                    // Strip comma if it exists
                    if (text.StartsWith(","))
                    {
                        text = text.Substring(1);
                        if (text == string.Empty)
                            values.Add(string.Empty);
                    }
                }
                else
                {
                    // Value does not start with double quote, so a single comma must close this value
                    int closeIndex = text.IndexOf(",");
                    if (closeIndex < 0)
                    {
                        values.Add(text);
                        text = null;
                    }
                    else
                    {
                        values.Add(text.Substring(0, closeIndex));
                        text = text.Substring(closeIndex + 1);
                        if (text == string.Empty)
                            values.Add(string.Empty);
                    }
                }
            }
            return values;
        }
    }
}
