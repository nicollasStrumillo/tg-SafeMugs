using backend.models;
using backend.models.Enums;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class ApplicationDBContext : DbContext
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Perfil> Perfis => Set<Perfil>();
    public DbSet<Endereco> Enderecos => Set<Endereco>();
    public DbSet<CategoriaProduto> CategoriasProduto => Set<CategoriaProduto>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<ImagemProduto> ImagensProduto => Set<ImagemProduto>();
    public DbSet<Avaliacao> Avaliacoes => Set<Avaliacao>();
    public DbSet<Carrinho> Carrinhos => Set<Carrinho>();
    public DbSet<ItemCarrinho> ItensCarrinho => Set<ItemCarrinho>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();
    public DbSet<Desafio> Desafios => Set<Desafio>();
    public DbSet<DicaDesafio> DicasDesafio => Set<DicaDesafio>();
    public DbSet<ProgressoDesafio> ProgressosDesafio => Set<ProgressoDesafio>();

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.ToTable("perfis");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NomeCompleto).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(180);
            entity.Property(e => e.Senha).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Telefone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasOne(e => e.Perfil)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(e => e.PerfilId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.ToTable("enderecos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Logradouro).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Bairro).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Cidade).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Estado).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Cep).IsRequired().HasMaxLength(20);

            entity.HasOne(e => e.Usuario)
                .WithMany(e => e.Enderecos)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CategoriaProduto>(entity =>
        {
            entity.ToTable("categorias_produto");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.ToTable("produtos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Preco).HasPrecision(18, 2);
            entity.Property(e => e.Ativo).HasDefaultValue(true);

            entity.HasOne(e => e.CategoriaProduto)
                .WithMany(e => e.Produtos)
                .HasForeignKey(e => e.CategoriaProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ImagemProduto>(entity =>
        {
            entity.ToTable("imagens_produto");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UrlImagem).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Legenda).HasMaxLength(255);

            entity.HasOne(e => e.Produto)
                .WithMany(e => e.ImagensProduto)
                .HasForeignKey(e => e.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Avaliacao>(entity =>
        {
            entity.ToTable("avaliacoes", t => t.HasCheckConstraint("CK_Avaliacao_Nota", "Nota >= 1 AND Nota <= 5"));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Comentario).HasMaxLength(1000);
            entity.HasIndex(e => new { e.UsuarioId, e.ProdutoId }).IsUnique();

            entity.HasOne(e => e.Usuario)
                .WithMany(e => e.Avaliacoes)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Produto)
                .WithMany(e => e.Avaliacoes)
                .HasForeignKey(e => e.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Carrinho>(entity =>
        {
            entity.ToTable("carrinhos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(30);
            entity.HasIndex(e => e.UsuarioId).IsUnique();

            entity.HasOne(e => e.Usuario)
                .WithOne(e => e.Carrinho)
                .HasForeignKey<Carrinho>(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ItemCarrinho>(entity =>
        {
            entity.ToTable("itens_carrinho");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PrecoUnitario).HasPrecision(18, 2);
            entity.Property(e => e.SubTotal).HasPrecision(18, 2);
            entity.HasIndex(e => new { e.CarrinhoId, e.ProdutoId }).IsUnique();

            entity.HasOne(e => e.Carrinho)
                .WithMany(e => e.Itens)
                .HasForeignKey(e => e.CarrinhoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Produto)
                .WithMany(e => e.ItensCarrinho)
                .HasForeignKey(e => e.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.ToTable("pedidos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(30);
            entity.Property(e => e.MetodoPagamento).HasConversion<string>().HasMaxLength(30);
            entity.Property(e => e.ValorTotal).HasPrecision(18, 2);
            entity.HasIndex(e => e.NumeroPedido).IsUnique();

            entity.HasOne(e => e.Usuario)
                .WithMany(e => e.Pedidos)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Endereco)
                .WithMany(e => e.Pedidos)
                .HasForeignKey(e => e.EnderecoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.ToTable("itens_pedido");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PrecoUnitario).HasPrecision(18, 2);
            entity.Property(e => e.SubTotal).HasPrecision(18, 2);
            entity.HasIndex(e => new { e.PedidoId, e.ProdutoId }).IsUnique();

            entity.HasOne(e => e.Pedido)
                .WithMany(e => e.ItensPedido)
                .HasForeignKey(e => e.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Produto)
                .WithMany(e => e.ItensPedido)
                .HasForeignKey(e => e.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Desafio>(entity =>
        {
            entity.ToTable("desafios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Categoria).IsRequired().HasConversion<string>().HasMaxLength(100);
            entity.Property(e => e.Dificuldade).HasConversion<string>().HasMaxLength(30);
            entity.Property(e => e.UrlMitigacao).IsRequired().HasMaxLength(500);
        });

        modelBuilder.Entity<DicaDesafio>(entity =>
        {
            entity.ToTable("dicas_desafio");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Texto).IsRequired().HasMaxLength(1000);
            entity.HasIndex(e => new { e.DesafioId, e.NrDica }).IsUnique();

            entity.HasOne(e => e.Desafio)
                .WithMany(e => e.DicasDesafio)
                .HasForeignKey(e => e.DesafioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProgressoDesafio>(entity =>
        {
            entity.ToTable("progressos_desafio");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UsuarioId, e.DesafioId }).IsUnique();

            entity.HasOne(e => e.Usuario)
                .WithMany(e => e.ProgressosDesafio)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Desafio)
                .WithMany(e => e.ProgressosDesafio)
                .HasForeignKey(e => e.DesafioId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
