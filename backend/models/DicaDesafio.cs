namespace backend.models;
public class DicaDesafio
{
    public int Id { get; set; }
    public int NrDica { get; set; }
    public string Texto { get; set; } = string.Empty;

    public int DesafioId { get; set; }
    public Desafio Desafio { get; set; } = null!;
}
