using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Security
{
    public interface ISecurityService
    {
        /// <summary>
        /// Returns a newly created token.
        /// </summary>
        /// <param name="timeSpan">Length of time, into the future, before token expires.</param>
        /// <returns>Returns a token.</returns>
        Token CreateToken(TimeSpan timeSpan);

        /// <summary>
        /// Returns string representation of token.
        /// </summary>
        /// <param name="token">The token to serialize.</param>
        /// <returns>Token as string.</returns>
        string SerializeToken(Token token);

        /// <summary>
        /// Returns string representation of token value.
        /// </summary>
        /// <param name="token">The token whose value is serialized.</param>
        /// <returns>Token value as string.</returns>
        string SerializeTokenValue(Token token);

        /// <summary>
        /// Returns true if token expired, false if token still valid.
        /// </summary>
        /// <param name="token">Token whose expiry is checked.</param>
        /// <returns>True if token expired, false if token valid.</returns>
        bool TokenExpired(Token token);

        /// <summary>
        /// Returns token from string representation.
        /// </summary>
        /// <param name="text">The string to deserialize into a token.</param>
        /// <returns>Token.</returns>
        Token DeserializeToken(string text);

        /// <summary>
        /// Returns true if text is a valid token, false if not.
        /// </summary>
        /// <param name="text">String representation of token.</param>
        /// <param name="token">Will be populated with token (or set null if text not valid token).</param>
        /// <returns>True if text can be deserialized into a Token, false if not.</returns>
        bool ParseToken(string text, out Token token);

        /// <summary>
        /// Creates salt which can be used for password hashing.
        /// </summary>
        /// <param name="saltSize">Size of salt (number of bytes) that is created.</param>
        /// <returns>Salt.</returns>
        byte[] CreateSalt(int saltSize);

        /// <summary>
        /// Gets salt and salted hash of the plain text password supplied.
        /// </summary>
        /// <param name="password">Plain text password.</param>
        /// <param name="salt">Used to salt hash of password.</param>
        /// <returns>Salted hash of plain text password and the salt that was used.</returns>
        byte[] EncryptPassword(string password, byte[] salt);
    }
}
