using System.Security.Cryptography;
using System.Text;

namespace backend.Helpers;

public static class HashHelper
{
    public static string GerarMD5(string texto)
    {
        using var md5 = MD5.Create();

        var bytes = Encoding.UTF8.GetBytes(texto);

        var hash = md5.ComputeHash(bytes);

        return Convert.ToHexString(hash);
    }

    public static bool VerificarMD5(string texto, string hash)
    {
        var hashGerado = GerarMD5(texto);

        return hashGerado.Equals(hash, StringComparison.OrdinalIgnoreCase);
    }
}