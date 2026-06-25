namespace backend.models;

public class ProgressoDesafio
{
    public int Id { get; set; }
    public DateTime DtResolvido { get; set; }
    public int QtDicasDesbloqueadas { get; set; }
    public DateTime DtAtualizacao { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int DesafioId { get; set; }
    public Desafio Desafio { get; set; } = null!;

}
