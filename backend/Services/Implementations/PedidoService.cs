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
using backend.models.Enums;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations;

public class PedidoService : IPedidoService
{
    private static readonly HashSet<string> TabelasDoBanco = new(StringComparer.OrdinalIgnoreCase)
    {
        "usuarios", "pedidos", "produtos", "carrinhos", "itens_carrinho",
        "categorias_produto", "comentarios_produto", "avaliacoes", "enderecos",
        "perfis", "desafios", "dicas_desafio"
    };

    private readonly IPedidoRepository _pedidoRepository;

    private readonly ApplicationDBContext _context;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEnderecoRepository _enderecoRepository;
    private readonly ICarrinhoService _carrinhoService;
    private readonly IDesafioService _desafioService;

    private readonly IAuthenticatedUserService _user;

    public PedidoService(IPedidoRepository pedidoRepository, ICarrinhoService carrinhoService, IUsuarioRepository usuarioRepository, IEnderecoRepository enderecoRepository, ApplicationDBContext context, IAuthenticatedUserService user, IDesafioService desafioService)
    {
        _pedidoRepository = pedidoRepository;
        _usuarioRepository = usuarioRepository;
        _enderecoRepository = enderecoRepository;
        _carrinhoService = carrinhoService;
        _user = user;
        _context = context;
        _desafioService = desafioService;
    }

    private PedidoDTO MapToDTO(Pedido pedido)
    {
        return new PedidoDTO
        {
            Id = pedido.Id,
            NumeroPedido = pedido.NumeroPedido,
            ValorTotal = pedido.ValorTotal,
            QuantidadeItens = pedido.QuantidadeItens,
            DataPedido = pedido.DataPedido,
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
                DataPedido = DateTime.UtcNow,
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

    public async Task<List<PedidoResumoDTO>> ListarPedidosPorUsuarioAsync(int usuarioId)
    {
        if (usuarioId != _user.UsuarioId)
            throw new UnauthorizedAccessException("Você não tem permissão para listar pedidos de outro usuário.");

        var pedidos = await _pedidoRepository.ObterPedidosPorUsuarioIdAsync(usuarioId);

        return pedidos.Select(p => new PedidoResumoDTO
        {
            Id = p.Id,
            NumeroPedido = p.NumeroPedido,
            ValorTotal = p.ValorTotal,
            QuantidadeItens = p.QuantidadeItens,
            DataPedido = p.DataPedido,
            UsuarioId = p.UsuarioId
        }).ToList();
    }

    public async Task<PedidoDTO> ObterDetalhesPedidoAsync(int pedidoId)
    {
        var pedido = await _pedidoRepository.ObterPedidoPorIdAsync(pedidoId)
            ?? throw new NotFoundException($"Pedido com id {pedidoId} não encontrado");

        if (pedido.UsuarioId != _user.UsuarioId)
            throw new UnauthorizedAccessException("Você não tem permissão para acessar os detalhes deste pedido.");

        return MapToDTO(pedido);
    }

    public async Task<List<PedidoResumoDTO>> PesquisarPedidoPorNumeroAsync(string numeroPedido)
    {
        if (string.IsNullOrWhiteSpace(numeroPedido))
            throw new ValidationException("Informe o número do pedido.");

        var pedidos = await _pedidoRepository.ObterPedidoPorNumeroAsync(numeroPedido, _user.UsuarioId);

        await _desafioService.SolveIfAsync(DesafiosEnum.EnumerarTabelasBanco, () =>
            TabelasDoBanco.All(tabela => pedidos.Any(pedido =>
                string.Equals(pedido.NumeroPedido, tabela, StringComparison.OrdinalIgnoreCase))));

        return pedidos;
    }

    public async Task CancelarPedidoAsync(int pedidoId)
    {
        var pedido = await _pedidoRepository.ObterPedidoPorIdAsync(pedidoId)
            ?? throw new NotFoundException($"Pedido com id {pedidoId} não encontrado");

        if (pedido.UsuarioId != _user.UsuarioId)
            throw new UnauthorizedAccessException("Você não tem permissão para cancelar este pedido.");

        await _pedidoRepository.CancelarPedidoAsync(pedido);
    }
}
