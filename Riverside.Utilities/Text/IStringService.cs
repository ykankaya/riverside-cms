using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Text
{
    public interface IStringService
    {
        /// <summary>
        /// Replaces keywords found in provided text with dynamic text.
        /// </summary>
        /// <param name="text">The text whose keywords are to be substituted.</param>
        /// <param name="substitutions">Keyword substitutions. Keys found in text are replaced with the corresponding key values.</param>
        /// <param name="htmlEncode">Set true to HTML encode a substitution value.</param>
        /// <returns>The specified text with keywords replaced with keyword values.</returns>
        string SubstituteKeywords(string text, IEnumerable<KeyValuePair<string, string>> substitutions, bool htmlEncode);

        /// <summary>
        /// Converts text to byte array.
        /// </summary>
        /// <param name="text">The text to convert.</param>
        /// <returns>The resuting byte array.</returns>
        byte[] GetBytes(string text);

        /// <summary>
        /// Converts byte array to text.
        /// </summary>
        /// <param name="bytes">Byte array to convert.</param>
        /// <returns>String representation of bytes.</returns>
        string GetString(byte[] bytes);

        /// <summary>
        /// Compares two byte arrays. Returns true if their contents are the same.
        /// </summary>
        /// <param name="byteArray1">Byte array 1.</param>
        /// <param name="byteArray2">Byte array 2.</param>
        /// <returns>True if arrays equal (i.e. same length and containing the same bytes).</returns>
        bool ByteArraysEqual(byte[] byteArray1, byte[] byteArray2);

        /// <summary>
        /// Retrieve CSV values from single line of text.
        /// </summary>
        /// <param name="text">Line from CSV file.</param>
        /// <returns>Individual values.</returns>
        List<string> GetCsvValues(string text);
    }
}
