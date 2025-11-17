namespace CryptoApi.Models;

public class DecryptRequest
{
    public string CipherTextBase64 { get; set; } = default!;
    public string IvBase64 { get; set; } = default!;
}
