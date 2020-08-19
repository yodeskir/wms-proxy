using WMSTools.Interfaces;

namespace WMSTools
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        public byte[] Create()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return bytes;
            }
        }
    }
}