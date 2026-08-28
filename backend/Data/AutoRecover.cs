namespace backend.Data;

public static class AutoRecover
{
    public static void RestaurarImagens(string webRootPath)
    {
        var seedImagens = Path.Combine(webRootPath + "-seed", "imagens");

        if (!Directory.Exists(seedImagens))
            return;

        var imagens = Path.Combine(webRootPath, "imagens");

        if (Directory.Exists(imagens))
            Directory.Delete(imagens, recursive: true);

        CopiarDiretorio(seedImagens, imagens);
    }

    private static void CopiarDiretorio(string origem, string destino)
    {
        Directory.CreateDirectory(destino);

        foreach (var arquivo in Directory.GetFiles(origem))
            File.Copy(arquivo, Path.Combine(destino, Path.GetFileName(arquivo)));

        foreach (var pasta in Directory.GetDirectories(origem))
            CopiarDiretorio(pasta, Path.Combine(destino, Path.GetFileName(pasta)));
    }
}
