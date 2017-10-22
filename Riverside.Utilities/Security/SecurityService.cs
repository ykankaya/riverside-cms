using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Riverside.Utilities.Security
{
    public class SecurityService : ISecurityService
    {
        /// <summary>
        /// Returns a newly created token.
        /// </summary>
        /// <param name="timeSpan">Length of time, into the future, before token expires.</param>
        /// <returns>Returns a token.</returns>
        public Token CreateToken(TimeSpan timeSpan)
        {
            return new Token { Expiry = DateTime.UtcNow + timeSpan, Value = Guid.NewGuid() };
        }

        /// <summary>
        /// Returns string representation of token.
        /// </summary>
        /// <param name="token">The token to serialize.</param>
        /// <returns>Token as string.</returns>
        public string SerializeToken(Token token)
        {
            return string.Format("{0}-{1}", token.Value, token.Expiry.Ticks);
        }

        /// <summary>
        /// Returns string representation of token value.
        /// </summary>
        /// <param name="token">The token whose value is serialized.</param>
        /// <returns>Token value as string.</returns>
        public string SerializeTokenValue(Token token)
        {
            return string.Format("{0}", token.Value);
        }

        /// <summary>
        /// Returns true if token expired, false if token still valid.
        /// </summary>
        /// <param name="token">Token whose expiry is checked.</param>
        /// <returns>True if token expired, false if token valid.</returns>
        public bool TokenExpired(Token token)
        {
            return (token.Expiry <= DateTime.UtcNow);
        }

        /// <summary>
        /// Returns token from string representation.
        /// </summary>
        /// <param name="text">The string to deserialize into a token.</param>
        /// <returns>Token.</returns>
        public Token DeserializeToken(string text)
        {
            string valueText = text.Substring(0, text.LastIndexOf('-'));
            string expiryText = text.Substring(text.LastIndexOf('-') + 1);
            return new Token { Value = new Guid(valueText), Expiry = new DateTime(Convert.ToInt64(expiryText)) };
        }

        /// <summary>
        /// Returns true if text is a valid token, false if not.
        /// </summary>
        /// <param name="text">String representation of token.</param>
        /// <param name="token">Will be populated with token (or set null if text not valid token).</param>
        /// <returns>True if text can be deserialized into a Token, false if not.</returns>
        public bool ParseToken(string text, out Token token)
        {
            try
            {
                token = DeserializeToken(text);
                return true;
            }
            catch
            {
                token = null;
                return false;
            }
        }

        /// <summary>
        /// Creates salt which can be used for password hashing.
        /// </summary>
        /// <param name="saltSize">Size of salt (number of bytes) that is created.</param>
        /// <returns>Salt.</returns>
        public byte[] CreateSalt(int saltSize)
        {
            // Get salt
            byte[] salt = new byte[saltSize];
            using (RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetBytes(salt);
            }
            return salt;
        }

        /// <summary>
        /// Gets salt and salted hash of the plain text password supplied.
        /// See http://stackoverflow.com/questions/2138429/hash-and-salt-passwords-in-c-sharp for more details.
        /// </summary>
        /// <param name="password">Plain text password.</param>
        /// <param name="salt">Used to salt hash of password.</param>
        /// <returns>Salted hash of plain text password.</returns>
        public byte[] EncryptPassword(string password, byte[] salt)
        {
            byte[] saltedHash = null;
            byte[] plainText = Encoding.UTF8.GetBytes(password);
            using (HashAlgorithm hashAlgorithm = new SHA256Managed())
            {
                byte[] plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];
                for (int index = 0; index < plainText.Length; index++)
                    plainTextWithSaltBytes[index] = plainText[index];
                for (int index = 0; index < salt.Length; index++)
                    plainTextWithSaltBytes[plainText.Length + index] = salt[index];
                saltedHash = hashAlgorithm.ComputeHash(plainTextWithSaltBytes);
            }
            return saltedHash;
        }
    }
}
