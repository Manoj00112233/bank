using Banking_CapStone.Service;
using System.Security.Cryptography;
using System.Text;

public class EncryptionService : IEncryptionService
{
    private readonly string _key;
    private readonly string _iv;

    public EncryptionService(IConfiguration configuration)
    {
        _key = configuration["Encryption:Key"] ?? "MyStrongDefaultKey1234567890123456"; // 32 chars
        _iv = configuration["Encryption:IV"] ?? "MyStrongDefaultIV12";                // 16 chars
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return plainText;

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_key);
        aes.IV = Encoding.UTF8.GetBytes(_iv);

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
            sw.Write(plainText);

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return cipherText;

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_key);
        aes.IV = Encoding.UTF8.GetBytes(_iv);

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
    public string MaskString(string input, int visibleChars = 4)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return input.Length <= visibleChars
            ? input
            : new string('*', input.Length - visibleChars) + input[^visibleChars..];
    }
}