using backend.Data;
using backend.models;
using backend.models.Enums;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Implementations;
public class CarrinhoRepository : ICarrinhoRepository
{
    private readonly ApplicationDBContext _dbContext;

    public CarrinhoRepository(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Carrinho?> ObterCarrinhoAsNoTrackingPorUsuarioIdAsync(int usuarioId)
    {
        return await _dbContext.Carrinhos.AsNoTracking()
            .Include(c => c.Itens)
                .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
    }
  
    public async Task<Carrinho> ObterOuCriarCarrinhoAtivoAsync(int usuarioId)
    {
        var carrinhoAtivo = await _dbContext.Carrinhos
            .Include(c => c.Itens)
                .ThenInclude(i => i.Produto)
                    .ThenInclude(p => p.CategoriaProduto)
            .Include(c => c.Usuario)
                .ThenInclude(u => u.Endereco)
            .Include(c => c.Usuario)
                .ThenInclude(u => u.Perfil)
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Status == StatusCarrinho.Ativo);
        
        if (carrinhoAtivo == null)
        {
            carrinhoAtivo = new Carrinho
            {
                UsuarioId = usuarioId,
                Status = StatusCarrinho.Ativo,
                DtCadastro = DateTime.UtcNow,
                DtAtualizacao = DateTime.UtcNow
            };
            _dbContext.Carrinhos.Add(carrinhoAtivo);
            await _dbContext.SaveChangesAsync();
        }

        return carrinhoAtivo;
    }

    // Adiciona o produto ao carrinho ou simplesmente incrementa a quantidadea
    public async Task AdicionarUnidadeProdutoAoCarrinhoAsync(Carrinho carrinho, Produto produto, int quantidade)
    {
        var itemExistente = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produto.Id);
        if (itemExistente != null)
            itemExistente.Quantidade += quantidade;
        else
        {
            var novoItem = new ItemCarrinho
            {
                ProdutoId = produto.Id,
                Quantidade = quantidade,
                PrecoUnitario = produto.Preco,
                CarrinhoId = carrinho.Id
            };
            carrinho.Itens.Add(novoItem);
        }

        carrinho.DtAtualizacao = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
    }

    //Decrementa a quantidade do produto no carrinho ou remove o item se a quantidade se tornar 0
    public async Task RemoverUnidadeProdutoDoCarrinhoAsync(Carrinho carrinho, int produtoId, int quantidade)
    {
        var itemExistente = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        if (itemExistente == null) return;

        if (itemExistente.Quantidade - quantidade <= 0)
        {
            carrinho.Itens.Remove(itemExistente);
            _dbContext.ItensCarrinho.Remove(itemExistente);
        }
        else
        {
            itemExistente.Quantidade -= quantidade;
        }
        carrinho.DtAtualizacao = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
    }

    public async Task FinalizarCarrinhoAsync(Carrinho carrinho)
    {
        carrinho.Status = StatusCarrinho.Finalizado;
        carrinho.DtAtualizacao = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
    }
}
