namespace backend.models;

public class Usuario
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public DateTime DtCadastro { get; set; }
    public DateTime DtAtualizacao { get; set; }

    public int PerfilId { get; set; }
    public Perfil Perfil { get; set; } = null!;

    public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    public ICollection<ProgressoDesafio> ProgressosDesafio { get; set; } = new List<ProgressoDesafio>();
    public Carrinho? Carrinho { get; set; }
}
