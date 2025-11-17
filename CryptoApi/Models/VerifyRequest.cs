namespace CryptoApi.Models;

public class VerifyRequest
{
    public string Data { get; set; } = default!;
    public string SignatureBase64 { get; set; } = default!;
}
