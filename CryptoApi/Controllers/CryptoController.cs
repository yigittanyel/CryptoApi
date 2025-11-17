using CryptoApi.Models;
using CryptoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CryptoController : ControllerBase
{
    private readonly CryptoService _crypto;

    public CryptoController(CryptoService crypto)
    {
        _crypto = crypto;
    }

    // AES Encrypt
    [HttpPost("encrypt")]
    public ActionResult<EncryptResponse> Encrypt([FromBody] EncryptRequest request)
    {
        var (cipher, iv) = _crypto.EncryptAes(request.PlainText);

        return Ok(new EncryptResponse
        {
            CipherTextBase64 = Convert.ToBase64String(cipher),
            IvBase64 = Convert.ToBase64String(iv)
        });
    }

    // AES Decrypt
    [HttpPost("decrypt")]
    public ActionResult Decrypt([FromBody] DecryptRequest request)
    {
        var cipher = Convert.FromBase64String(request.CipherTextBase64);
        var iv = Convert.FromBase64String(request.IvBase64);

        var plainText = _crypto.DecryptAes(cipher, iv);
        return Ok(new { PlainText = plainText });
    }

    // RSA Sign
    [HttpPost("sign")]
    public ActionResult<SignResponse> Sign([FromBody] SignRequest request)
    {
        var signature = _crypto.Sign(request.Data);

        return Ok(new SignResponse
        {
            SignatureBase64 = Convert.ToBase64String(signature)
        });
    }

    // RSA Verify
    [HttpPost("verify")]
    public ActionResult Verify([FromBody] VerifyRequest request)
    {
        var signatureBytes = Convert.FromBase64String(request.SignatureBase64);
        var valid = _crypto.Verify(request.Data, signatureBytes);

        return Ok(new { IsValid = valid });
    }
}
