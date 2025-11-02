namespace Banking_CapStone.Service
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
        string MaskString(string input, int visibleChars = 4);
    }
}
