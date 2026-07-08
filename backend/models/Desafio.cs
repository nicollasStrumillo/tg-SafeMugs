using backend.models.Enums;

namespace backend.models;

public class Desafio
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public CategoriaDesafio Categoria { get; set; }
    public int Dificuldade { get; set; }
    public string UrlMitigacao { get; set; } = string.Empty;
    public bool Resolvido { get; set; }

    public ICollection<DicaDesafio> DicasDesafio { get; set; } = new List<DicaDesafio>();
}
