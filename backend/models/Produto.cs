namespace backend.models;
public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public bool Ativo { get; set; }
    public DateTime DtCadastro { get; set; }
    public DateTime DtAtualizacao { get; set; }

    public int CategoriaProdutoId { get; set; }
    public CategoriaProduto CategoriaProduto { get; set; } = null!;

    public ICollection<ImagemProduto> ImagensProduto { get; set; } = new List<ImagemProduto>();
    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
    public ICollection<ItemCarrinho> ItensCarrinho { get; set; } = new List<ItemCarrinho>();
    public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
}
