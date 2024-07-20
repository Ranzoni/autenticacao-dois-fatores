using AutenticacaoDoisFatores.Core.Excecoes;
using AutenticacaoDoisFatores.Core.Extensoes;
using System.Security.Cryptography;
using System.Text;

namespace AutenticacaoDoisFatores.Core.Servicos
{
    internal static class Criptografia
    {
        internal static string Criptografar(string valor)
        {
            var chaveCriptografia = Environment.GetEnvironmentVariable("ENCRYPT_KEY") ?? "";
            if (chaveCriptografia.IsNullOrEmptyOrWhiteSpaces())
                CriptografiaException.ChaveNaoEncontrada();

            if (string.IsNullOrEmpty(valor))
                return valor;

            var key = Encoding.UTF8.GetBytes(chaveCriptografia);

            using var aes = Aes.Create();
            using var criptografia = aes.CreateEncryptor(key, aes.IV);
            using var msCriptografia = new MemoryStream();
            using (var csCriptografia = new CryptoStream(msCriptografia, criptografia, CryptoStreamMode.Write))
            using (var swCriptografia = new StreamWriter(csCriptografia))
            {
                swCriptografia.Write(valor);
            }

            var iv = aes.IV;

            var valorDescriptografado = msCriptografia.ToArray();

            var valorCriptografado = new byte[iv.Length + valorDescriptografado.Length];

            Buffer.BlockCopy(iv, 0, valorCriptografado, 0, iv.Length);
            Buffer.BlockCopy(valorDescriptografado, 0, valorCriptografado, iv.Length, valorDescriptografado.Length);

            return Convert.ToBase64String(valorCriptografado);
        }
    }
}
