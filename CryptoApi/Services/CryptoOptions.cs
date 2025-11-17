namespace CryptoApi.Services;

public class CryptoOptions
{
    public string AesKeyBase64 { get; set; } = default!;
    public string AesIvBase64 { get; set; } = default!;
    public string RsaPrivateKeyBase64 { get; set; } = default!;
    public string RsaPublicKeyBase64 { get; set; } = default!;
}
