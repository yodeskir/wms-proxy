using System;
using System.Security.Cryptography;
using System.Text;
using WMSTools.Interfaces;
using IRandomNumberGenerator = WMSTools.Interfaces.IRandomNumberGenerator;

namespace WMSTools
{
    public class HashHelper : IHashHelper
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        public HashHelper(IRandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator ?? throw new ArgumentNullException(nameof(randomNumberGenerator));
        }
        public string GetHash(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public string GetSalt()
        {
            var bytes = _randomNumberGenerator.Create();
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public string Base64Encode(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(plainTextBytes);
        }

        public string Base64Decode(string base64EncodedData)
        {
            if (base64EncodedData == null)
                throw new ArgumentNullException(nameof(base64EncodedData));
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return null;
            }
        }
    }
}
