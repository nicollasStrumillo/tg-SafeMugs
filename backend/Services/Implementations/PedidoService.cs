using backend.Authentication.Interfaces;
using backend.Data;
using backend.DTOs.Carrinho;
using backend.DTOs.Endereco;
using backend.DTOs.Pedido;
using backend.DTOs.Produto;
using backend.DTOs.Usuario;
using backend.Exceptions;
using backend.Helpers;
using backend.models;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;

    private readonly ApplicationDBContext _context;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEnderecoRepository _enderecoRepository;
    private readonly ICarrinhoService _carrinhoService;

    private readonly IAuthenticatedUserService _user;

    public PedidoService(IPedidoRepository pedidoRepository, ICarrinhoService carrinhoService, IUsuarioRepository usuarioRepository, IEnderecoRepository enderecoRepository, ApplicationDBContext context, IAuthenticatedUserService user)
    {
        _pedidoRepository = pedidoRepository;
        _usuarioRepository = usuarioRepository;
        _enderecoRepository = enderecoRepository;
        _carrinhoService = carrinhoService;
        _user = user;
        _context = context;
    }

    private PedidoDTO MapToDTO(Pedido pedido)
    {
        return new PedidoDTO
        {
            Id = pedido.Id,
            NumeroPedido = pedido.NumeroPedido,
            ValorTotal = pedido.ValorTotal,
            QuantidadeItens = pedido.QuantidadeItens,
            Usuario = new UsuarioDetalhesDTO
            {
                Id = pedido.Usuario.Id,
                NomeCompleto = pedido.Usuario.NomeCompleto,
                Email = pedido.Usuario.Email,
                Telefone = pedido.Usuario.Telefone,
                Ativo = pedido.Usuario.Ativo,
                DtCadastro = pedido.Usuario.DtCadastro,
                DtAtualizacao = pedido.Usuario.DtAtualizacao,
                UrlImagemPerfil = pedido.Usuario.UrlImagemPerfil,
                Perfil = pedido.Usuario.Perfil.Nome,
                Endereco = pedido.Usuario.Endereco != null ? new EnderecoDTO
                {
                    Id = pedido.Usuario.Endereco.Id,
                    Logradouro = pedido.Usuario.Endereco.Logradouro,
                    Numero = pedido.Usuario.Endereco.Numero,
                    Complemento = pedido.Usuario.Endereco.Complemento,
                    Bairro = pedido.Usuario.Endereco.Bairro,
                    Cidade = pedido.Usuario.Endereco.Cidade,
                    Estado = pedido.Usuario.Endereco.Estado,
                    Cep = pedido.Usuario.Endereco.Cep
                } : null
            },
            Endereco = new EnderecoDTO
            {
                Id = pedido.Endereco.Id,
                Logradouro = pedido.Endereco.Logradouro,
                Numero = pedido.Endereco.Numero,
                Complemento = pedido.Endereco.Complemento,
                Bairro = pedido.Endereco.Bairro,
                Cidade = pedido.Endereco.Cidade,
                Estado = pedido.Endereco.Estado,
                Cep = pedido.Endereco.Cep
            },
            Carrinho = new CarrinhoDTO
            {
                Id = pedido.Carrinho.Id,
                Status = pedido.Carrinho.Status.ToString(),
                Total = pedido.ValorTotal,
                Itens = pedido.Carrinho.Itens.Select(i => new ItemCarrinhoDTO
                {
                    Id = i.Id,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    Produto = new ProdutoDTO
                    {
                        Id = i.Produto.Id,
                        Nome = i.Produto.Nome,
                        Descricao = i.Produto.Descricao,
                        Preco = i.Produto.Preco,
                        UrlImagemProduto = i.Produto.UrlImagemProduto
                    }
                }).ToList()
            }
        };
    }

    public async Task<PedidoDTO> CriarPedido(int usuarioId)
    {
        if (usuarioId != _user.UsuarioId)
            throw new UnauthorizedAccessException("Você não tem permissão para criar um pedido por outro usuário.");
        
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try{
            var usuario = await _usuarioRepository.BuscarPorIdAsync(usuarioId)
                ?? throw new NotFoundException($"Usuário com id {usuarioId} não encontrado");

            var endereco = usuario.Endereco 
                ?? throw new ValidationException("Este usuário não possui um endereço");
            var snapshotEndereco = await _enderecoRepository.CriarDuplicataAsync(endereco);

            var carrinho = await _carrinhoService.FinalizarEObterCarrinhoAsync(usuario.Id);

            decimal valorTotal = carrinho.Itens.Sum(i => i.PrecoUnitario * i.Quantidade);
            int quantidadeItens = carrinho.Itens.Sum(i => i.Quantidade);

            Pedido novoPedido = new()
            {
                NumeroPedido = "placeholder",
                ValorTotal = valorTotal,
                QuantidadeItens = quantidadeItens,
                UsuarioId = usuario.Id,
                Usuario = usuario,
                EnderecoId = snapshotEndereco.Id,
                Endereco = snapshotEndereco,
                CarrinhoId = carrinho.Id,
                Carrinho = carrinho
            };

            var pedidoPersistido = await _pedidoRepository.PersistirPedidoAsync(novoPedido);
            string numeroPedido = NumeroPedidoGenerator.GerarNumeroPedido(pedidoPersistido.Id);
            await _pedidoRepository.MudarNumeroPedidoAsync(pedidoPersistido, numeroPedido);

            await transaction.CommitAsync();

            return MapToDTO(pedidoPersistido);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
