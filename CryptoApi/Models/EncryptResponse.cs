namespace CryptoApi.Models;

public class EncryptResponse
{
    public string CipherTextBase64 { get; set; } = default!;
    public string IvBase64 { get; set; } = default!;
}
