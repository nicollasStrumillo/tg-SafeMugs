using backend.DTOs.Carrinho;
using backend.DTOs.Produto;
using backend.DTOs.Usuario;
using backend.Exceptions;
using backend.models;
using backend.models.Enums;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations;

public class CarrinhoService : ICarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;

    private readonly IProdutoService _produtoService;

    public CarrinhoService(ICarrinhoRepository carrinhoRepository, IProdutoService produtoService)
    {
        _carrinhoRepository = carrinhoRepository;
        _produtoService = produtoService;
    }

    private CarrinhoDTO MapCarrinhoToDTO(Carrinho carrinho)
    {
        var dto = new CarrinhoDTO
        {
            Id = carrinho.Id,
            Status = carrinho.Status.ToString(),
            Usuario = new UsuarioDetalhesDTO
            {
                Id = carrinho.Usuario.Id,
                NomeCompleto = carrinho.Usuario.NomeCompleto,
                Email = carrinho.Usuario.Email,
                Telefone = carrinho.Usuario.Telefone,
                Ativo = carrinho.Usuario.Ativo,
                DtCadastro = carrinho.Usuario.DtCadastro,
                DtAtualizacao = carrinho.Usuario.DtAtualizacao,
                UrlImagemPerfil = carrinho.Usuario.UrlImagemPerfil,
                Perfil = carrinho.Usuario.Perfil.Nome,
                Endereco = carrinho.Usuario.Endereco != null ? new DTOs.Endereco.EnderecoDTO
                {
                    Id = carrinho.Usuario.Endereco.Id,
                    Logradouro = carrinho.Usuario.Endereco.Logradouro,
                    Numero = carrinho.Usuario.Endereco.Numero,
                    Complemento = carrinho.Usuario.Endereco.Complemento,
                    Bairro = carrinho.Usuario.Endereco.Bairro,
                    Cidade = carrinho.Usuario.Endereco.Cidade,
                    Estado = carrinho.Usuario.Endereco.Estado,
                    Cep = carrinho.Usuario.Endereco.Cep
                } : null
            },

            Itens = carrinho.Itens.Select(i => new ItemCarrinhoDTO
            {
                Id = i.Id,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.Produto.Preco,
                Produto = new ProdutoDTO
                {
                    Id = i.Produto.Id,
                    Nome = i.Produto.Nome,
                    Descricao = i.Produto.Descricao,
                    Preco = i.Produto.Preco,
                    Estoque = i.Produto.Estoque,
                    CategoriaProduto = new CategoriaProdutoDTO
                    {
                        Id = i.Produto.CategoriaProduto.Id,
                        Nome = i.Produto.CategoriaProduto.Nome,
                        Descricao = i.Produto.CategoriaProduto.Descricao
                    },
                    UrlImagemProduto = i.Produto.UrlImagemProduto
                }
            }).ToList()
        };
        dto.Total = dto.Itens.Sum(i => i.PrecoTotal);
        return dto;
    }

    public async Task<CarrinhoDTO> ObterOuCriarCarrinhoAtivoAsync(int usuarioId)
    {
        var carrinho = await _carrinhoRepository.ObterOuCriarCarrinhoAtivoAsync(usuarioId);
        return MapCarrinhoToDTO(carrinho);
    }

    public async Task AdicionarUnidadeProdutoAoCarrinhoAsync(int usuarioId, int produtoId, int quantidade)
    {
        var produto = await _produtoService.ObterProdutoPorIdAsync(produtoId)
            ?? throw new NotFoundException($"Produto com ID {produtoId} não encontrado.");

        var carrinho = await _carrinhoRepository.ObterOuCriarCarrinhoAtivoAsync(usuarioId);
        await _carrinhoRepository.AdicionarUnidadeProdutoAoCarrinhoAsync(carrinho, produto, quantidade);
    }

    public async Task RemoverUnidadeProdutoDoCarrinhoAsync(int usuarioId, int produtoId, int quantidade)
    {
        var carrinho = await _carrinhoRepository.ObterOuCriarCarrinhoAtivoAsync(usuarioId);
        await _carrinhoRepository.RemoverUnidadeProdutoDoCarrinhoAsync(carrinho, produtoId, quantidade);
    }

    public async Task FinalizarCarrinhoAsync(int usuarioId)
    {
        var carrinho = await _carrinhoRepository.ObterCarrinhoAsNoTrackingPorUsuarioIdAsync(usuarioId)
            ?? throw new NotFoundException($"Carrinho para o usuário com ID {usuarioId} não encontrado.");
        if (carrinho.Status != StatusCarrinho.Ativo)
            throw new InvalidOperationException($"O carrinho com ID {carrinho.Id} não está ativo e não pode ser finalizado.");

        var carrinhoAsTracked = await _carrinhoRepository.ObterOuCriarCarrinhoAtivoAsync(usuarioId);
        
        await _carrinhoRepository.FinalizarCarrinhoAsync(carrinhoAsTracked);
    }
}
