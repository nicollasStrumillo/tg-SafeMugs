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
    public DbSet<ComentarioProduto> ComentariosProduto => Set<ComentarioProduto>();
    public DbSet<Carrinho> Carrinhos => Set<Carrinho>();
    public DbSet<ItemCarrinho> ItensCarrinho => Set<ItemCarrinho>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();
    public DbSet<Desafio> Desafios => Set<Desafio>();
    public DbSet<DicaDesafio> DicasDesafio => Set<DicaDesafio>();

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
            entity.ToTable("desafios", d => d.HasCheckConstraint("CK_Desafio_Dificuldade", "Dificuldade >= 1 AND Dificuldade <= 5"));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Categoria).IsRequired().HasConversion<string>().HasMaxLength(100);
            entity.Property(e => e.Dificuldade).IsRequired();
            entity.Property(e => e.UrlMitigacao).IsRequired().HasMaxLength(500);

            entity.HasIndex(e => e.Nome).IsUnique();
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
                Email = EmailsESenhasUsuarios.AnaLopes.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.AnaLopes.GetHashSenha(),
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
                Email = EmailsESenhasUsuarios.BrunoCosta.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.BrunoCosta.GetHashSenha(),
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
                Email = EmailsESenhasUsuarios.CarlaMendes.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.CarlaMendes.GetHashSenha(),
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
                Email = EmailsESenhasUsuarios.DiegoSouza.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.DiegoSouza.GetHashSenha(),
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
                Email = EmailsESenhasUsuarios.ElisaMartins.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.ElisaMartins.GetHashSenha(),
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
                Email = EmailsESenhasUsuarios.FelipeRocha.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.FelipeRocha.GetHashSenha(),
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
                Email = EmailsESenhasUsuarios.MarinaAlves.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.MarinaAlves.GetHashSenha(),
                Telefone = "11990000007",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1
            },
            new Usuario
            {
                Id = 8,
                NomeCompleto = "Admin",
                Email = EmailsESenhasUsuarios.Admin.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.Admin.GetHashSenha(),
                Telefone = "11990000008",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 2
            }
            );

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

        // Desafios e dicas 
        modelBuilder.Entity<Desafio>().HasData(
            new Desafio
            {
                Id = 1,
                Nome = "Login como Admin",
                Descricao = "Acesse uma conta administrativa utilizando SQL Injection.",
                Categoria = CategoriaDesafio.SqlInjection,
                Dificuldade = 2,
                UrlMitigacao = "url_placeholder",
                Resolvido = false
            },
            new Desafio
            {
                Id = 2,
                Nome = "DOM XSS",
                Descricao = "Utilize o payload \"<iframe src=\"javascript:alert(`XSS`)\">\" para causar um ataque de DOM XSS na página do catalogo",
                Categoria = CategoriaDesafio.DomXSS,
                Dificuldade = 2,
                UrlMitigacao = "url_placeholder",
                Resolvido = false
            },
            new Desafio
            {
                Id = 3,
                Nome = "Brute force de login",
                Descricao = "Acesse a conta de um dos usuários do domínio @safemugs.com utilizando força bruta.",
                Categoria = CategoriaDesafio.BrokenAntiAutomation,
                Dificuldade = 2,
                UrlMitigacao = "url_placeholder",
                Resolvido = false
            },
            new Desafio
            {
                Id = 4,
                Nome = "Validar cadastro",
                Descricao = "Teste a validação do formulário de cadastro.",
                Categoria = CategoriaDesafio.ImproperInputValidation,
                Dificuldade = 2,
                UrlMitigacao = "url_placeholder",
                Resolvido = false
            },
            new Desafio
            {
                Id = 5,
                Nome = "Manipular cadastro",
                Descricao = "Crie uma conta de administrador.",
                Categoria = CategoriaDesafio.ParameterTampering,
                Dificuldade = 3,
                UrlMitigacao = "url_placeholder",
                Resolvido = false
            },
            new Desafio
            {
                Id = 6,
                Nome = "Buscar por SQL Injection",
                Descricao = "Use a busca do catálogo para injetar uma consulta.",
                Categoria = CategoriaDesafio.SqlInjection,
                Dificuldade = 3,
                UrlMitigacao = "url_placeholder",
                Resolvido = false
            },
            new Desafio
            {
                Id = 7,
                Nome = "Buscar com script",
                Descricao = "Explore a busca do catálogo com payload de XSS refletido.",
                Categoria = CategoriaDesafio.ReflectedXSS,
                Dificuldade = 2,
                UrlMitigacao = "url_placeholder",
                Resolvido = false
            },
            new Desafio
            {
                Id = 8,
                Nome = "Alterar ordenação do catálogo",
                Descricao = "Modifique os parâmetros de filtro e ordenação da listagem para encontrar informações sensíveis.",
                Categoria = CategoriaDesafio.ParameterTampering,
                Dificuldade = 2,
                UrlMitigacao = "url_placeholder",
                Resolvido = false
            },
            new Desafio
            {
                Id = 9,
                Nome = "Stored XSS",
                Descricao = "Utilize o payload \"<iframe src=\"javascript:alert(`XSS`)\">\" para causar um ataque de Stored XSS na página do catalogo",
                Categoria = CategoriaDesafio.StoredXSS,
                Dificuldade = 3,
                UrlMitigacao = "url_placeholder",
                Resolvido = false
            },
            new Desafio
            {
                Id = 10,
                Nome = "Criar um comentário por outro usuário",
                Descricao = "Escreva um comentário que não pertence ao seu usuário.",
                Categoria = CategoriaDesafio.IDOR,
                Dificuldade = 3,
                UrlMitigacao = "url_placeholder",
                Resolvido = false
            },
            new Desafio
            {
                Id = 11,
                Nome = "Recuperar senha insegura",
                Descricao = "Explore o fluxo de esqueci minha senha sem proteção suficiente.",
                Categoria = CategoriaDesafio.BrokenAuthentication,
                Dificuldade = 3,
                UrlMitigacao = "url_placeholder",
                Resolvido = false
            },
            new Desafio
            {
                Id = 12,
                Nome = "Encontrar a Score-Board",
                Descricao = "Localize a página de score-board.",
                Categoria = CategoriaDesafio.Outros,
                Dificuldade = 1,
                UrlMitigacao = "url_placeholder",
                Resolvido = false
            },
            new Desafio
            {
                Id = 13,
                Nome = "Tratamento de Erro",
                Descricao = "Provoque um erro que o retorno da API não trata corretamente.",
                Categoria = CategoriaDesafio.SecurityMisconfiguration,
                Dificuldade = 1,
                UrlMitigacao = "url_placeholder",
                Resolvido = false
            }
        );

        modelBuilder.Entity<DicaDesafio>().HasData(
            new DicaDesafio
            {
                Id = 1,
                NrDica = 1,
                Texto = "Utilize SQL Injection para provocar um erro e observe a resposta da API.",
                DesafioId = 1
            },
            new DicaDesafio
            {
                Id = 2,
                NrDica = 2,
                Texto = "Tente identificar o e-mail de um administrador para fazer um ataque direcionado.",
                DesafioId = 1
            },
            new DicaDesafio
            {
                Id = 3,
                NrDica = 3,
                Texto = "Também é possível resolver utilizando outra coluna da tabela Usuários que não seja o e-mail.",
                DesafioId = 1
            },

            new DicaDesafio
            {
                Id = 4,
                NrDica = 1,
                Texto = "Procure por campos que reflitam sua entrada na interface.",
                DesafioId = 2
            },
            new DicaDesafio
            {
                Id = 5,
                NrDica = 2,
                Texto = "Tente pesquisar por produtos que não existem e observe a resposta.",
                DesafioId = 2
            },

            new DicaDesafio
            {
                Id = 6,
                NrDica = 1,
                Texto = "Não há bloqueio por muitas tentativas repetidas por minuto.",
                DesafioId = 3
            },
            new DicaDesafio
            {
                Id = 7,
                NrDica = 2,
                Texto = "Encontre um e-mail de usuário válido para atacar.",
                DesafioId = 3
            },

            new DicaDesafio
            {
                Id = 8,
                NrDica = 1,
                Texto = "Observe quais campos aceitam valores inesperados.",
                DesafioId = 4
            },
            new DicaDesafio
            {
                Id = 9,
                NrDica = 2,
                Texto = "A validação dos campos pode ser insuficiente.",
                DesafioId = 4
            },

            new DicaDesafio
            {
                Id = 10,
                NrDica = 1,
                Texto = "Identifique as colunas que compõem um usuário.",
                DesafioId = 5
            },
            new DicaDesafio
            {
                Id = 11,
                NrDica = 2,
                Texto = "Observar a resposta de uma requisição de login bem-sucedida é uma maneira de identificar as colunas que compõem um usuário.",
                DesafioId = 5
            },
            new DicaDesafio
            {
                Id = 12,
                NrDica = 3,
                Texto = "Você pode interceptar a requisição de cadastro e mudar o seu corpo.",
                DesafioId = 5
            },

            new DicaDesafio
            {
                Id = 13,
                NrDica = 1,
                Texto = "A busca do catálogo é a superfície de ataque.",
                DesafioId = 6
            },
            new DicaDesafio
            {
                Id = 14,
                NrDica = 2,
                Texto = "O nome do desafio aponta para SQL Injection.",
                DesafioId = 6
            },

            new DicaDesafio
            {
                Id = 15,
                NrDica = 1,
                Texto = "A busca reflete sua entrada na interface de resposta.",
                DesafioId = 7
            },
            new DicaDesafio
            {
                Id = 16,
                NrDica = 2,
                Texto = "Teste inserir um payload malicioso no termo de pesquisa.",
                DesafioId = 7
            },

            new DicaDesafio
            {
                Id = 17,
                NrDica = 1,
                Texto = "Filtros e ordenação costumam vir por query string.",
                DesafioId = 8
            },
            new DicaDesafio
            {
                Id = 18,
                NrDica = 2,
                Texto = "O backend pode estar ordenando por qualquer parâmetro que ele receber.",
                DesafioId = 8
            },

            new DicaDesafio
            {
                Id = 19,
                NrDica = 1,
                Texto = "O desafio está na seção de detalhes do produto.",
                DesafioId = 9
            },
            new DicaDesafio
            {
                Id = 20,
                NrDica = 2,
                Texto = "Comentários podem ser renderizados sem sanitização.",
                DesafioId = 9
            },

            new DicaDesafio
            {
                Id = 21,
                NrDica = 1,
                Texto = "O payload enviado ao backend pode estar transmitindo dados de forma insegura.",
                DesafioId = 10
            },
            new DicaDesafio
            {
                Id = 22,
                NrDica = 2,
                Texto = "Tente interceptar e modificar a requisição.",
                DesafioId = 10
            },

            new DicaDesafio
            {
                Id = 23,
                NrDica = 1,
                Texto = "Dica 1 placeholder",
                DesafioId = 11
            },
            new DicaDesafio
            {
                Id = 24,
                NrDica = 2,
                Texto = "Dica 2 placeholder",
                DesafioId = 11
            },

            new DicaDesafio
            {
                Id = 25,
                NrDica = 1,
                Texto = "Este desafio pode ser resolvido a partir de diferentes telas.",
                DesafioId = 13
            },
            new DicaDesafio
            {
                Id = 26,
                NrDica = 2,
                Texto = "Tente inserir valores inesperados em formulários que possam provocar um erro no backend.",
                DesafioId = 13
            },
            new DicaDesafio
            {
                Id = 27,
                NrDica = 3,
                Texto = "Tente quebrar a consulta SQL realizada a partir da tela de Login.",
                DesafioId = 13
            }
        );

        #endregion
    }
}
