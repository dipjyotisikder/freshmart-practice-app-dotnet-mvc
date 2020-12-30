namespace FreshMart.Services
{
    public interface IEncryptionServices
    {
        string Encrypt(string inputText);
        string Encrypt(string inputText, string password);

        string Decrypt(string inputText);
        string Decrypt(string inputText, string password);

        string GetUniqueKey(int size);

        string GetUniqueKeyOriginal_BIASED(int size);
    }
}
