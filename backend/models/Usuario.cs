namespace backend.models;

public class Usuario
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string HashSenha { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public bool Ativo { get; set; }
    public DateTime DtCadastro { get; set; }
    public DateTime DtAtualizacao { get; set; }

    public int PerfilId { get; set; }
    public Perfil Perfil { get; set; } = null!;

    public int? EnderecoId { get; set; }
    public Endereco? Endereco { get; set; }

    public string UrlImagemPerfil { get; set; } = string.Empty;

    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
    public ICollection<ComentarioProduto> ComentariosProduto { get; set; } = new List<ComentarioProduto>();
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    public ICollection<Carrinho> Carrinhos { get; set; } = new List<Carrinho>();
}
