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
            entity.Property(e => e.HashSenha).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Telefone).IsRequired(false).HasMaxLength(20);
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

        modelBuilder.Entity<ComentarioProduto>(entity =>
        {
            entity.ToTable("comentarios_produto");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Comentario).IsRequired().HasMaxLength(400);

            entity.HasOne(e => e.Usuario)
                .WithMany(e => e.ComentariosProduto)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            entity.HasOne(e => e.Produto)
                .WithMany(e => e.ComentariosProduto)
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


        // Adiciona dados iniciais para o banco
        #region Seed Data
        var seedDate = new DateTime(2026, 6, 1, 8, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Perfil>().HasData(
            new Perfil
            {
                Id = 1,
                Nome = "Cliente",
                Descricao = "Perfil padrao para compras e avaliacoes"
            },
            new Perfil
            {
                Id = 2,
                Nome = "Administrador",
                Descricao = "Perfil para gestao interna da loja"
            });

        modelBuilder.Entity<CategoriaProduto>().HasData(
            new CategoriaProduto
            {
                Id = 1,
                Nome = "Canecas Decoradas",
                Descricao = "Canecas com estampas afetivas e divertidas"
            },
            new CategoriaProduto
            {
                Id = 2,
                Nome = "Canecas Tematicas",
                Descricao = "Modelos inspirados em pets e personagens"
            },
            new CategoriaProduto
            {
                Id = 3,
                Nome = "Canecas Rusticas",
                Descricao = "Pecas com acabamento artesanal e visual mais natural"
            });

        modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = 1,
                NomeCompleto = "Ana Lopes",
                Email = "ana.lopes@safermugs.com",
                HashSenha = "1b13939cd7d77f68bac85931bfbb0a36", // --> Seed@12345
                Telefone = "11990000001",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1
            },
            new Usuario
            {
                Id = 2,
                NomeCompleto = "Bruno Costa",
                Email = "bruno.costa@safermugs.com",
                HashSenha = "1b13939cd7d77f68bac85931bfbb0a36", // --> Seed@12345
                Telefone = "11990000002",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1
            },
            new Usuario
            {
                Id = 3,
                NomeCompleto = "Carla Mendes",
                Email = "carla.mendes@safermugs.com",
                HashSenha = "1b13939cd7d77f68bac85931bfbb0a36", // --> Seed@12345
                Telefone = "11990000003",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1
            },
            new Usuario
            {
                Id = 4,
                NomeCompleto = "Diego Souza",
                Email = "diego.souza@safermugs.com",
                HashSenha = "1b13939cd7d77f68bac85931bfbb0a36", // --> Seed@12345
                Telefone = "11990000004",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1
            },
            new Usuario
            {
                Id = 5,
                NomeCompleto = "Elisa Martins",
                Email = "elisa.martins@safermugs.com",
                HashSenha = "1b13939cd7d77f68bac85931bfbb0a36", // --> Seed@12345
                Telefone = "11990000005",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1
            },
            new Usuario
            {
                Id = 6,
                NomeCompleto = "Felipe Rocha",
                Email = "felipe.rocha@safermugs.com",
                HashSenha = "1b13939cd7d77f68bac85931bfbb0a36", // --> Seed@12345
                Telefone = "11990000006",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1
            },
            new Usuario
            {
                Id = 7,
                NomeCompleto = "Marina Alves",
                Email = "marina.alves@safermugs.com",
                HashSenha = "1b13939cd7d77f68bac85931bfbb0a36", // --> Seed@12345
                Telefone = "11990000007",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 2
            });

        modelBuilder.Entity<Produto>().HasData(
            new Produto
            {
                Id = 1,
                Nome = "Caneca Be Happy",
                Descricao = "Caneca clara com mensagem positiva e visual minimalista para o dia a dia.",
                Preco = 59.90m,
                Estoque = 18,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 1
            },
            new Produto
            {
                Id = 2,
                Nome = "Caneca Ceramica Rustica",
                Descricao = "Modelo com acabamento artesanal, textura marcada e estilo mais natural.",
                Preco = 54.90m,
                Estoque = 12,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 3
            },
            new Produto
            {
                Id = 3,
                Nome = "Caneca Coala",
                Descricao = "Caneca escura com estampa de coala para quem gosta de pecas fofas e diferentes.",
                Preco = 64.90m,
                Estoque = 15,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 2
            },
            new Produto
            {
                Id = 4,
                Nome = "Caneca Coracao",
                Descricao = "Caneca em tom quente com detalhe de coracao para presentear com carinho.",
                Preco = 62.90m,
                Estoque = 20,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 1
            },
            new Produto
            {
                Id = 5,
                Nome = "Caneca Cachorro",
                Descricao = "Caneca com ilustracao de cachorro e acabamento divertido para uso diario.",
                Preco = 58.90m,
                Estoque = 10,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 2
            },
            new Produto
            {
                Id = 6,
                Nome = "Caneca Vermelha Cafe",
                Descricao = "Caneca vermelha intensa, classica e versatil para cafe, cha ou chocolate.",
                Preco = 49.90m,
                Estoque = 25,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 1
            });

        modelBuilder.Entity<ImagemProduto>().HasData(
            new ImagemProduto
            {
                Id = 1,
                UrlImagem = "/imagens/mug_behappy.jpg",
                Legenda = "Caneca Be Happy",
                ProdutoId = 1
            },
            new ImagemProduto
            {
                Id = 2,
                UrlImagem = "/imagens/mug_ceramica_rustica.jpg",
                Legenda = "Caneca Ceramica Rustica",
                ProdutoId = 2
            },
            new ImagemProduto
            {
                Id = 3,
                UrlImagem = "/imagens/mug_coala.jpg",
                Legenda = "Caneca Coala",
                ProdutoId = 3
            },
            new ImagemProduto
            {
                Id = 4,
                UrlImagem = "/imagens/mug_coracao.jpg",
                Legenda = "Caneca Coracao",
                ProdutoId = 4
            },
            new ImagemProduto
            {
                Id = 5,
                UrlImagem = "/imagens/mug_dogpan.jpg",
                Legenda = "Caneca cachorro",
                ProdutoId = 5
            },
            new ImagemProduto
            {
                Id = 6,
                UrlImagem = "/imagens/mug_vermelha_cafe.jpg",
                Legenda = "Caneca Vermelha Cafe",
                ProdutoId = 6
            });

        modelBuilder.Entity<Avaliacao>().HasData(
            new Avaliacao
            {
                Id = 1,
                Nota = 5,
                Comentario = "Acabamento impecavel e o visual ficou exatamente como esperava.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 1,
                ProdutoId = 1
            },
            new Avaliacao
            {
                Id = 2,
                Nota = 4,
                Comentario = "Linda e com boa qualidade, chegou muito bem embalada.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 2,
                ProdutoId = 1
            },
            new Avaliacao
            {
                Id = 3,
                Nota = 5,
                Comentario = "Mensagem bem delicada e otima para presentear.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 3,
                ProdutoId = 1
            },
            new Avaliacao
            {
                Id = 4,
                Nota = 4,
                Comentario = "Textura bonita e o estilo rustico da um charme extra.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 2,
                ProdutoId = 2
            },
            new Avaliacao
            {
                Id = 5,
                Nota = 5,
                Comentario = "Peca muito bonita e com ar artesanal bem marcante.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 4,
                ProdutoId = 2
            },
            new Avaliacao
            {
                Id = 6,
                Nota = 5,
                Comentario = "A estampa de coala ficou excelente e a caneca e bem resistente.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 1,
                ProdutoId = 3
            },
            new Avaliacao
            {
                Id = 7,
                Nota = 4,
                Comentario = "Produto bonito e com acabamento muito caprichado.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 5,
                ProdutoId = 3
            },
            new Avaliacao
            {
                Id = 8,
                Nota = 5,
                Comentario = "Visual apaixonante e otima escolha para presente.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 6,
                ProdutoId = 3
            },
            new Avaliacao
            {
                Id = 9,
                Nota = 5,
                Comentario = "A ideia do coracao combinou muito com a cor da caneca.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 3,
                ProdutoId = 4
            },
            new Avaliacao
            {
                Id = 10,
                Nota = 4,
                Comentario = "Bonita e delicada, atende bem quem gosta de pecas afetivas.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 7,
                ProdutoId = 4
            },
            new Avaliacao
            {
                Id = 11,
                Nota = 5,
                Comentario = "A caneca do cachorro e divertida e tem uma pintura muito boa.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 4,
                ProdutoId = 5
            },
            new Avaliacao
            {
                Id = 12,
                Nota = 4,
                Comentario = "Gostei bastante do formato e da proposta mais descontraida.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 5,
                ProdutoId = 5
            },
            new Avaliacao
            {
                Id = 13,
                Nota = 5,
                Comentario = "Cor vibrante, pega muito bem e parece otima para o dia a dia.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 1,
                ProdutoId = 6
            },
            new Avaliacao
            {
                Id = 14,
                Nota = 4,
                Comentario = "Modelo classico e elegante, combina com qualquer ambiente.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 2,
                ProdutoId = 6
            },
            new Avaliacao
            {
                Id = 15,
                Nota = 5,
                Comentario = "Perfeita para cafe, com cor intensa e acabamento uniforme.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 6,
                ProdutoId = 6
            });

        modelBuilder.Entity<ComentarioProduto>().HasData(
            new ComentarioProduto
            {
                Id = 1,
                ProdutoId = 1,
                UsuarioId = 1,
                Comentario = "Muito bonita pessoalmente."
            },
            new ComentarioProduto
            {
                Id = 2,
                ProdutoId = 1,
                UsuarioId = null,
                Comentario = "Chegou antes do prazo."
            },
            new ComentarioProduto
            {
                Id = 3,
                ProdutoId = 1,
                UsuarioId = 3,
                Comentario = "A estampa ficou exatamente como nas fotos."
            },

            new ComentarioProduto
            {
                Id = 4,
                ProdutoId = 2,
                UsuarioId = 2,
                Comentario = "Gostei bastante do acabamento."
            },
            new ComentarioProduto
            {
                Id = 5,
                ProdutoId = 2,
                UsuarioId = null,
                Comentario = "A textura é bem diferente."
            },
            new ComentarioProduto
            {
                Id = 6,
                ProdutoId = 2,
                UsuarioId = 5,
                Comentario = "Combina muito com decoração em madeira."
            },

            new ComentarioProduto
            {
                Id = 7,
                ProdutoId = 3,
                UsuarioId = 6,
                Comentario = "Minha filha adorou."
            },
            new ComentarioProduto
            {
                Id = 8,
                ProdutoId = 3,
                UsuarioId = null,
                Comentario = "A arte ficou muito bonita."
            },
            new ComentarioProduto
            {
                Id = 9,
                ProdutoId = 3,
                UsuarioId = 1,
                Comentario = "Veio muito bem embalada."
            },

            new ComentarioProduto
            {
                Id = 10,
                ProdutoId = 4,
                UsuarioId = 4,
                Comentario = "Comprei para dar de presente."
            },
            new ComentarioProduto
            {
                Id = 11,
                ProdutoId = 4,
                UsuarioId = null,
                Comentario = "A cor é ainda mais bonita ao vivo."
            },
            new ComentarioProduto
            {
                Id = 12,
                ProdutoId = 4,
                UsuarioId = 2,
                Comentario = "Gostei do tamanho da caneca."
            },

            new ComentarioProduto
            {
                Id = 13,
                ProdutoId = 5,
                UsuarioId = 5,
                Comentario = "Perfeita para quem gosta de cachorros."
            },
            new ComentarioProduto
            {
                Id = 14,
                ProdutoId = 5,
                UsuarioId = null,
                Comentario = "Entrega rápida."
            },
            new ComentarioProduto
            {
                Id = 15,
                ProdutoId = 5,
                UsuarioId = 7,
                Comentario = "A impressão ficou muito nítida."
            },

            new ComentarioProduto
            {
                Id = 16,
                ProdutoId = 6,
                UsuarioId = 3,
                Comentario = "Bem resistente."
            },
            new ComentarioProduto
            {
                Id = 17,
                ProdutoId = 6,
                UsuarioId = null,
                Comentario = "Ótima para café pela manhã."
            },
            new ComentarioProduto
            {
                Id = 18,
                ProdutoId = 6,
                UsuarioId = 6,
                Comentario = "Produto conforme anunciado."
            }

        );

        #endregion


    }
}
