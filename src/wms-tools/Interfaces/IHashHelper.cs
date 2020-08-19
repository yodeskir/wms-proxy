namespace WMSTools.Interfaces
{
    public interface IHashHelper
    {
        string Base64Decode(string base64EncodedData);
        string Base64Encode(string text);
        string GetHash(string text);
        string GetSalt();
    }
}