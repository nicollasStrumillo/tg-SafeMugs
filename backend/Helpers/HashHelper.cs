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

    public static string GerarMD5PorData()
    {
        string data = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
        
        byte[] inputBytes = Encoding.UTF8.GetBytes(data);

        using (MD5 md5 = MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}