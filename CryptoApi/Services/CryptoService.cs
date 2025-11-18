using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace CryptoApi.Services;

public class CryptoService
{
    private readonly byte[] _aesKey;
    private readonly byte[] _aesIv;
    private readonly RSA _rsaPrivate;
    private readonly RSA _rsaPublic;

    public CryptoService(IOptions<CryptoOptions> options)
    {
        var opt = options.Value;

        _aesKey = Convert.FromBase64String(opt.AesKeyBase64);
        _aesIv = Convert.FromBase64String(opt.AesIvBase64);

        _rsaPrivate = RSA.Create();
        _rsaPublic = RSA.Create();

        // Private key import
        var privateKeyBytes = Convert.FromBase64String(opt.RsaPrivateKeyBase64);
        _rsaPrivate.ImportPkcs8PrivateKey(privateKeyBytes, out _);

        // Public key import
        var publicKeyBytes = Convert.FromBase64String(opt.RsaPublicKeyBase64);
        _rsaPublic.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
    }

    #region AES

    public (byte[] cipher, byte[] iv) EncryptAes(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _aesKey;

        // Her şifreleme işlemi için random IV üretmek daha güvenli:
        aes.GenerateIV();
        var iv = aes.IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, iv);
        var bytes = Encoding.UTF8.GetBytes(plainText);
        var cipher = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);

        return (cipher, iv);
    }

    public string DecryptAes(byte[] cipher, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = _aesKey;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        var decrypted = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
        return Encoding.UTF8.GetString(decrypted);
    }

    #endregion

    #region Signature (RSA)

    public byte[] Sign(string data)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        return _rsaPrivate.SignData(
            bytes,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);
    }

    public bool Verify(string data, byte[] signature)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        return _rsaPublic.VerifyData(
            bytes,
            signature,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);
    }

    #endregion
}
