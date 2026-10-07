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
    public DbSet<Avaliacao> Avaliacoes => Set<Avaliacao>();
    public DbSet<ComentarioProduto> ComentariosProduto => Set<ComentarioProduto>();
    public DbSet<Carrinho> Carrinhos => Set<Carrinho>();
    public DbSet<ItemCarrinho> ItensCarrinho => Set<ItemCarrinho>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
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
            entity.Property(e => e.UrlImagemPerfil).IsRequired().HasMaxLength(500);

            entity.HasOne(e => e.Perfil)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(e => e.PerfilId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Endereco)
                .WithOne(e => e.Usuario)
                .HasForeignKey<Usuario>(e => e.EnderecoId)
                .OnDelete(DeleteBehavior.SetNull);  
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
            entity.Property(e => e.Complemento).IsRequired(false).HasMaxLength(200);
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
            entity.Property(e => e.Ativo).HasDefaultValue(false);
            entity.Property(e => e.UrlImagemProduto).IsRequired();

            entity.HasOne(e => e.CategoriaProduto)
                .WithMany(e => e.Produtos)
                .HasForeignKey(e => e.CategoriaProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Avaliacao>(entity =>
        {
            entity.ToTable("avaliacoes", t => t.HasCheckConstraint("CK_Avaliacao_Nota", "Nota >= 0 AND Nota <= 5.0"));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Comentario).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Nota).IsRequired();
            entity.Property(e => e.DtCadastro).IsRequired();
            entity.Property(e => e.DtAtualizacao).IsRequired();
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
            
            entity.HasOne(e => e.Usuario)
                .WithMany(e => e.Carrinhos)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ItemCarrinho>(entity =>
        {
            entity.ToTable("itens_carrinho");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PrecoUnitario).HasPrecision(18, 2);
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
            entity.Property(e => e.NumeroPedido).IsRequired().HasMaxLength(30);
            entity.Property(e => e.ValorTotal).HasPrecision(18, 2);
            entity.Property(e => e.QuantidadeItens).IsRequired();
            entity.Property(e => e.DataPedido).IsRequired();
            entity.HasIndex(e => e.NumeroPedido).IsUnique();

            entity.HasOne(e => e.Usuario)
                .WithMany(e => e.Pedidos)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Endereco)
                .WithMany(e => e.Pedidos)
                .HasForeignKey(e => e.EnderecoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Carrinho)
                .WithOne(e => e.Pedido)
                .HasForeignKey<Pedido>(e => e.CarrinhoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Desafio>(entity =>
        {
            entity.ToTable("desafios", d => d.HasCheckConstraint("CK_Desafio_Dificuldade", "Dificuldade >= 1 AND Dificuldade <= 5"));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(500);
            entity.Property(e => e.DescricaoDetalhes).IsRequired().HasMaxLength(1500);
            entity.Property(e => e.Categoria).IsRequired().HasConversion<string>().HasMaxLength(100);
            entity.Property(e => e.Dificuldade).IsRequired();
            entity.Property(e => e.Rota).IsRequired(false);
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
                HashSenha = EmailsESenhasUsuarios.AnaLopes.GetDescription(),
                Telefone = "11990000001",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1,
                UrlImagemPerfil = "/imagens/perfil/generic_profile.jpg"
            },
            new Usuario
            {
                Id = 2,
                NomeCompleto = "Bruno Costa",
                Email = EmailsESenhasUsuarios.BrunoCosta.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.BrunoCosta.GetDescription(),
                Telefone = "11990000002",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1,
                UrlImagemPerfil = "/imagens/perfil/generic_profile.jpg"
            },
            new Usuario
            {
                Id = 3,
                NomeCompleto = "Carla Mendes",
                Email = EmailsESenhasUsuarios.CarlaMendes.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.CarlaMendes.GetDescription(),
                Telefone = "11990000003",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1,
                UrlImagemPerfil = "/imagens/perfil/generic_profile.jpg"
            },
            new Usuario
            {
                Id = 4,
                NomeCompleto = "Diego Souza",
                Email = EmailsESenhasUsuarios.DiegoSouza.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.DiegoSouza.GetDescription(),
                Telefone = "11990000004",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1,
                UrlImagemPerfil = "/imagens/perfil/generic_profile.jpg"
            },
            new Usuario
            {
                Id = 5,
                NomeCompleto = "Elisa Martins",
                Email = EmailsESenhasUsuarios.ElisaMartins.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.ElisaMartins.GetDescription(),
                Telefone = "11990000005",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1,
                UrlImagemPerfil = "/imagens/perfil/generic_profile.jpg"
            },
            new Usuario
            {
                Id = 6,
                NomeCompleto = "Felipe Rocha",
                Email = EmailsESenhasUsuarios.FelipeRocha.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.FelipeRocha.GetDescription(),
                Telefone = "11990000006",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1,
                UrlImagemPerfil = "/imagens/perfil/generic_profile.jpg"
            },
            new Usuario
            {
                Id = 7,
                NomeCompleto = "Marina Alves",
                Email = EmailsESenhasUsuarios.MarinaAlves.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.MarinaAlves.GetDescription(),
                Telefone = "11990000007",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 1,
                UrlImagemPerfil = "/imagens/perfil/generic_profile.jpg"
            },
            new Usuario
            {
                Id = 8,
                NomeCompleto = "Admin",
                Email = EmailsESenhasUsuarios.Admin.GetNomeDisplay(),
                HashSenha = EmailsESenhasUsuarios.Admin.GetDescription(),
                Telefone = "11990000008",
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                PerfilId = 2,
                UrlImagemPerfil = "/imagens/perfil/generic_admin_profile.jpg"
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
                CategoriaProdutoId = 1,
                UrlImagemProduto = "/imagens/produto/mug_behappy.jpg"
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
                CategoriaProdutoId = 3,
                UrlImagemProduto = "/imagens/produto/mug_ceramica_rustica.jpg"
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
                CategoriaProdutoId = 2,
                UrlImagemProduto = "/imagens/produto/mug_coala.jpg"
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
                CategoriaProdutoId = 1,
                UrlImagemProduto = "/imagens/produto/mug_coracao.jpg"
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
                CategoriaProdutoId = 2,
                UrlImagemProduto = "/imagens/produto/mug_dogpan.jpg"
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
                CategoriaProdutoId = 1,
                UrlImagemProduto = "/imagens/produto/mug_vermelha_cafe.jpg"
            },
            new Produto
            {
                Id = 7,
                Nome = "Caneca Gato Cinza",
                Descricao = "Caneca branca com ilustracao de um gato cinza segurando um morango.",
                Preco = 56.90m,
                Estoque = 0,
                Ativo = false,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 2,
                UrlImagemProduto = "/imagens/produto/mug_gato_morango.jpg"
            },
            new Produto
            {
                Id = 8,
                Nome = "Caneca Purrista Cat Cafe",
                Descricao = "Caneca de ceramica artesanal em tons azuis e terracota, com selo em relevo Purrista Cat Cafe.",
                Preco = 69.90m,
                Estoque = 8,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 3,
                UrlImagemProduto = "/imagens/produto/mug_cat_cafe.jpg"
            },
            new Produto
            {
                Id = 9,
                Nome = "Caneca CSS is Amazing",
                Descricao = "Caneca branca com frase divertida CSS is Amazing, ideal para programadores e devs.",
                Preco = 52.90m,
                Estoque = 22,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 1,
                UrlImagemProduto = "/imagens/produto/mug_css.jpg"
            },
            new Produto
            {
                Id = 10,
                Nome = "Caneca Desenhos de Gatos",
                Descricao = "Caneca branca ilustrada com diversas raças de gatos e seus nomes em japones e ingles.",
                Preco = 61.90m,
                Estoque = 14,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 2,
                UrlImagemProduto = "/imagens/produto/mug_desenhos_gatos.jpg"
            },
            new Produto
            {
                Id = 11,
                Nome = "Caneca Icones Cafe",
                Descricao = "Caneca clara com ilustracao de cafeteira italiana e xicara fumegante, tema cafe.",
                Preco = 55.90m,
                Estoque = 17,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 1,
                UrlImagemProduto = "/imagens/produto/mug_icones_cafe.jpg"
            },
            new Produto
            {
                Id = 12,
                Nome = "Caneca Preta Fosca",
                Descricao = "Caneca preta fosca de design minimalista, combina com qualquer ambiente.",
                Preco = 47.90m,
                Estoque = 30,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 1,
                UrlImagemProduto = "/imagens/produto/mug_preta.jpg"
            },
            new Produto
            {
                Id = 13,
                Nome = "Caneca Rosa",
                Descricao = "Caneca rosa clara de acabamento fosco, visual delicado e moderno.",
                Preco = 53.90m,
                Estoque = 19,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 1,
                UrlImagemProduto = "/imagens/produto/mug_rosa.jpg"
            },
            new Produto
            {
                Id = 14,
                Nome = "Caneca Sono",
                Descricao = "Caneca de ceramica artesanal com rosto sonolento gravado, acabamento rustico.",
                Preco = 58.90m,
                Estoque = 11,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 3,
                UrlImagemProduto = "/imagens/produto/mug_sono.jpg"
            },
            new Produto
            {
                Id = 15,
                Nome = "Caneca Sorriso Engraçado",
                Descricao = "Caneca branca com rosto sorridente, para o dia a dia com bom humor.",
                Preco = 49.90m,
                Estoque = 16,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 1,
                UrlImagemProduto = "/imagens/produto/mug_sorriso.jpg"
            },
            new Produto
            {
                Id = 16,
                Nome = "Caneca World's Best Boss",
                Descricao = "Caneca branca com a frase World's Best Boss, otima opção de presente para chefes.",
                Preco = 54.90m,
                Estoque = 13,
                Ativo = true,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                CategoriaProdutoId = 1,
                UrlImagemProduto = "/imagens/produto/mug_worlds_best_boss.jpg"
            }
        );

        modelBuilder.Entity<Avaliacao>().HasData(
            new Avaliacao
            {
                Id = 1,
                Nota = 5f,
                Comentario = "Acabamento impecavel e o visual ficou exatamente como esperava.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 1,
                ProdutoId = 1
            },
            new Avaliacao
            {
                Id = 2,
                Nota = 4.5f,
                Comentario = "Linda e com boa qualidade, chegou muito bem embalada.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 2,
                ProdutoId = 1
            },
            new Avaliacao
            {
                Id = 3,
                Nota = 5f,
                Comentario = "Mensagem bem delicada e otima para presentear.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 3,
                ProdutoId = 1
            },
            new Avaliacao
            {
                Id = 4,
                Nota = 4f,
                Comentario = "Textura bonita e o estilo rustico da um charme extra.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 2,
                ProdutoId = 2
            },
            new Avaliacao
            {
                Id = 5,
                Nota = 3.5f,
                Comentario = "Peca muito bonita e com ar artesanal bem marcante.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 4,
                ProdutoId = 2
            },
            new Avaliacao
            {
                Id = 6,
                Nota = 4.5f,
                Comentario = "A estampa de coala ficou excelente e a caneca e bem resistente.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 1,
                ProdutoId = 3
            },
            new Avaliacao
            {
                Id = 7,
                Nota = 4f,
                Comentario = "Produto bonito e com acabamento muito caprichado.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 5,
                ProdutoId = 3
            },
            new Avaliacao
            {
                Id = 8,
                Nota = 5f,
                Comentario = "Visual apaixonante e otima escolha para presente.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 6,
                ProdutoId = 3
            },
            new Avaliacao
            {
                Id = 9,
                Nota = 3.5f,
                Comentario = "A ideia do coracao combinou muito com a cor da caneca.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 3,
                ProdutoId = 4
            },
            new Avaliacao
            {
                Id = 10,
                Nota = 4.5f,
                Comentario = "Bonita e delicada, atende bem quem gosta de pecas afetivas.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 7,
                ProdutoId = 4
            },
            new Avaliacao
            {
                Id = 11,
                Nota = 4f,
                Comentario = "A caneca do cachorro e divertida e tem uma pintura muito boa.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 4,
                ProdutoId = 5
            },
            new Avaliacao
            {
                Id = 12,
                Nota = 4f,
                Comentario = "Gostei bastante do formato e da proposta mais descontraida.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 5,
                ProdutoId = 5
            },
            new Avaliacao
            {
                Id = 13,
                Nota = 5f,
                Comentario = "Cor vibrante, pega muito bem e parece otima para o dia a dia.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 1,
                ProdutoId = 6
            },
            new Avaliacao
            {
                Id = 14,
                Nota = 4.4f,
                Comentario = "Modelo classico e elegante, combina com qualquer ambiente.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 2,
                ProdutoId = 6
            },
            new Avaliacao
            {
                Id = 15,
                Nota = 4.5f,
                Comentario = "Perfeita para cafe, com cor intensa e acabamento uniforme.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 6,
                ProdutoId = 6
            },
            // Produto 8 - Caneca Purrista Cat Cafe (3 avaliacoes)
            new Avaliacao
            {
                Id = 16,
                Nota = 5f,
                Comentario = "Acabamento artesanal lindo e ceramica muito bem feita.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 2,
                ProdutoId = 8
            },
            new Avaliacao
            {
                Id = 17,
                Nota = 4.5f,
                Comentario = "Detalhe em relevo da um charme especial, adorei o tom terracota.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 4,
                ProdutoId = 8
            },
            new Avaliacao
            {
                Id = 18,
                Nota = 4f,
                Comentario = "Caneca charmosa e diferente, combina bem com decoracao.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 7,
                ProdutoId = 8
            },
            // Produto 9 - Caneca CSS is Amazing (4 avaliacoes)
            new Avaliacao
            {
                Id = 19,
                Nota = 5f,
                Comentario = "Frase divertida e estampa muito nitida, perfeita para o trabalho.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 1,
                ProdutoId = 9
            },
            new Avaliacao
            {
                Id = 20,
                Nota = 4.5f,
                Comentario = "Otima para presentear colegas da area de tecnologia.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 3,
                ProdutoId = 9
            },
            new Avaliacao
            {
                Id = 21,
                Nota = 4f,
                Comentario = "Material de boa qualidade e impressao bem feita.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 5,
                ProdutoId = 9
            },
            new Avaliacao
            {
                Id = 22,
                Nota = 5f,
                Comentario = "Chegou bem embalada e o acabamento e excelente.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 6,
                ProdutoId = 9
            },
            // Produto 10 - Caneca Desenhos de Gatos (2 avaliacoes)
            new Avaliacao
            {
                Id = 23,
                Nota = 5f,
                Comentario = "Ilustracoes muito fofas e detalhadas, adorei a proposta.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 2,
                ProdutoId = 10
            },
            new Avaliacao
            {
                Id = 24,
                Nota = 4.5f,
                Comentario = "Estampa criativa e cores muito bonitas.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 7,
                ProdutoId = 10
            },
            // Produto 11 - Caneca Icones Cafe (3 avaliacoes)
            new Avaliacao
            {
                Id = 25,
                Nota = 4.5f,
                Comentario = "Design elegante e tema de cafe muito bem representado.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 1,
                ProdutoId = 11
            },
            new Avaliacao
            {
                Id = 26,
                Nota = 4f,
                Comentario = "Visual delicado e acabamento caprichado.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 4,
                ProdutoId = 11
            },
            new Avaliacao
            {
                Id = 27,
                Nota = 5f,
                Comentario = "Perfeita para a rotina do cafe da manha.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 6,
                ProdutoId = 11
            },
            // Produto 12 - Caneca Preta Fosca (2 avaliacoes)
            new Avaliacao
            {
                Id = 28,
                Nota = 4.5f,
                Comentario = "Acabamento fosco muito bonito e sofisticado.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 3,
                ProdutoId = 12
            },
            new Avaliacao
            {
                Id = 29,
                Nota = 5f,
                Comentario = "Minimalista e versatil, combina com tudo.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 5,
                ProdutoId = 12
            },
            // Produto 13 - Caneca Rosa (4 avaliacoes)
            new Avaliacao
            {
                Id = 30,
                Nota = 4.5f,
                Comentario = "Tom de rosa suave e acabamento impecavel.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 2,
                ProdutoId = 13
            },
            new Avaliacao
            {
                Id = 31,
                Nota = 4f,
                Comentario = "Caneca delicada e com tamanho otimo para o dia a dia.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 4,
                ProdutoId = 13
            },
            new Avaliacao
            {
                Id = 32,
                Nota = 5f,
                Comentario = "Cor linda e material de otima qualidade.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 1,
                ProdutoId = 13
            },
            new Avaliacao
            {
                Id = 33,
                Nota = 4.5f,
                Comentario = "Visual moderno e muito agradavel.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 7,
                ProdutoId = 13
            },
            // Produto 14 - Caneca Sono (3 avaliacoes)
            new Avaliacao
            {
                Id = 34,
                Nota = 5f,
                Comentario = "Rostinho sonolento muito fofo e acabamento rustico lindo.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 3,
                ProdutoId = 14
            },
            new Avaliacao
            {
                Id = 35,
                Nota = 4f,
                Comentario = "Peca diferente e com personalidade, gostei bastante.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 5,
                ProdutoId = 14
            },
            new Avaliacao
            {
                Id = 36,
                Nota = 4.5f,
                Comentario = "Ceramica bem trabalhada e detalhe gravado caprichado.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 6,
                ProdutoId = 14
            },
            // Produto 15 - Caneca Sorriso (2 avaliacoes)
            new Avaliacao
            {
                Id = 37,
                Nota = 5f,
                Comentario = "Rosto sorridente traz bom humor para o dia.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 1,
                ProdutoId = 15
            },
            new Avaliacao
            {
                Id = 38,
                Nota = 4.5f,
                Comentario = "Caneca leve e com estampa muito simpatica.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 4,
                ProdutoId = 15
            },
            // Produto 16 - Caneca World's Best Boss (3 avaliacoes)
            new Avaliacao
            {
                Id = 39,
                Nota = 4.5f,
                Comentario = "Frase divertida e otima ideia para presente.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 2,
                ProdutoId = 16
            },
            new Avaliacao
            {
                Id = 40,
                Nota = 5f,
                Comentario = "Estampa bem feita e caneca de boa qualidade.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 3,
                ProdutoId = 16
            },
            new Avaliacao
            {
                Id = 41,
                Nota = 4f,
                Comentario = "Gostei do acabamento e da proposta descontraida.",
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 7,
                ProdutoId = 16
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
            },

            // Produto 8 - Caneca Purrista Cat Cafe (3 comentarios)
            new ComentarioProduto
            {
                Id = 19,
                ProdutoId = 8,
                UsuarioId = 2,
                Comentario = "Adorei o tom azul e o detalhe em relevo."
            },
            new ComentarioProduto
            {
                Id = 20,
                ProdutoId = 8,
                UsuarioId = null,
                Comentario = "Chegou bem embalada."
            },
            new ComentarioProduto
            {
                Id = 21,
                ProdutoId = 8,
                UsuarioId = 5,
                Comentario = "Acabamento artesanal muito bonito."
            },

            // Produto 9 - Caneca CSS is Amazing (2 comentarios)
            new ComentarioProduto
            {
                Id = 22,
                ProdutoId = 9,
                UsuarioId = 1,
                Comentario = "Frase muito divertida."
            },
            new ComentarioProduto
            {
                Id = 23,
                ProdutoId = 9,
                UsuarioId = null,
                Comentario = "Presente perfeito para devs."
            },

            // Produto 10 - Caneca Desenhos de Gatos (4 comentarios)
            new ComentarioProduto
            {
                Id = 24,
                ProdutoId = 10,
                UsuarioId = 7,
                Comentario = "Ilustrações muito fofas."
            },
            new ComentarioProduto
            {
                Id = 25,
                ProdutoId = 10,
                UsuarioId = null,
                Comentario = "Os desenhos ficaram lindos."
            },
            new ComentarioProduto
            {
                Id = 26,
                ProdutoId = 10,
                UsuarioId = 2,
                Comentario = "Minha coleção favorita."
            },
            new ComentarioProduto
            {
                Id = 27,
                ProdutoId = 10,
                UsuarioId = 4,
                Comentario = "Cores bem vivas."
            },

            // Produto 11 - Caneca Icones Cafe (3 comentarios)
            new ComentarioProduto
            {
                Id = 28,
                ProdutoId = 11,
                UsuarioId = 3,
                Comentario = "Tema café ficou ótimo."
            },
            new ComentarioProduto
            {
                Id = 29,
                ProdutoId = 11,
                UsuarioId = null,
                Comentario = "Muito elegante."
            },
            new ComentarioProduto
            {
                Id = 30,
                ProdutoId = 11,
                UsuarioId = 6,
                Comentario = "Uso todo dia no trabalho."
            },

            // Produto 12 - Caneca Preta Fosca (2 comentarios)
            new ComentarioProduto
            {
                Id = 31,
                ProdutoId = 12,
                UsuarioId = 1,
                Comentario = "Preta fosca muito chique."
            },
            new ComentarioProduto
            {
                Id = 32,
                ProdutoId = 12,
                UsuarioId = null,
                Comentario = "Combina com qualquer ambiente."
            },

            // Produto 13 - Caneca Rosa (3 comentarios)
            new ComentarioProduto
            {
                Id = 33,
                ProdutoId = 13,
                UsuarioId = 4,
                Comentario = "Tonalidade rosa bem delicada."
            },
            new ComentarioProduto
            {
                Id = 34,
                ProdutoId = 13,
                UsuarioId = null,
                Comentario = "Linda ao vivo."
            },
            new ComentarioProduto
            {
                Id = 35,
                ProdutoId = 13,
                UsuarioId = 2,
                Comentario = "Tamanho ideal."
            },

            // Produto 14 - Caneca Sono (2 comentarios)
            new ComentarioProduto
            {
                Id = 36,
                ProdutoId = 14,
                UsuarioId = 6,
                Comentario = "Rostinho muito simpático."
            },
            new ComentarioProduto
            {
                Id = 37,
                ProdutoId = 14,
                UsuarioId = null,
                Comentario = "Acabamento rústico lindo."
            },

            // Produto 15 - Caneca Sorriso Kawaii (4 comentarios)
            new ComentarioProduto
            {
                Id = 38,
                ProdutoId = 15,
                UsuarioId = 3,
                Comentario = "Sorriso deixa o dia mais leve."
            },
            new ComentarioProduto
            {
                Id = 39,
                ProdutoId = 15,
                UsuarioId = null,
                Comentario = "Bem divertida."
            },
            new ComentarioProduto
            {
                Id = 40,
                ProdutoId = 15,
                UsuarioId = 5,
                Comentario = "Ótima para o café da tarde."
            },
            new ComentarioProduto
            {
                Id = 41,
                ProdutoId = 15,
                UsuarioId = 1,
                Comentario = "Estampa bem nítida."
            },

            // Produto 16 - Caneca World's Best Boss (2 comentarios)
            new ComentarioProduto
            {
                Id = 42,
                ProdutoId = 16,
                UsuarioId = 7,
                Comentario = "Presenteei meu chefe e ele adorou."
            },
            new ComentarioProduto
            {
                Id = 43,
                ProdutoId = 16,
                UsuarioId = null,
                Comentario = "Frase muito criativa."
            }

        );

        modelBuilder.Entity<Carrinho>().HasData(
            new Carrinho
            {
                Id = 1,
                Status = StatusCarrinho.Ativo,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 1
            },
            new Carrinho
            {
                Id = 2,
                Status = StatusCarrinho.Ativo,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 2
            },
            new Carrinho
            {
                Id = 3,
                Status = StatusCarrinho.Ativo,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 3
            },
            new Carrinho
            {
                Id = 4,
                Status = StatusCarrinho.Ativo,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 4
            },
            new Carrinho
            {
                Id = 5,
                Status = StatusCarrinho.Ativo,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 5
            },
            new Carrinho
            {
                Id = 6,
                Status = StatusCarrinho.Ativo,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 6
            },
            new Carrinho
            {
                Id = 7,
                Status = StatusCarrinho.Ativo,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 7
            },
            new Carrinho
            {
                Id = 8,
                Status = StatusCarrinho.Ativo,
                DtCadastro = seedDate,
                DtAtualizacao = seedDate,
                UsuarioId = 8
            }
        );

        modelBuilder.Entity<ItemCarrinho>().HasData(
            new ItemCarrinho
            {
                Id = 1,
                Quantidade = 1,
                PrecoUnitario = 64.90m,
                CarrinhoId = 1,
                ProdutoId = 3
            },
            new ItemCarrinho
            {
                Id = 2,
                Quantidade = 2,
                PrecoUnitario = 49.90m,
                CarrinhoId = 1,
                ProdutoId = 6
            },
            new ItemCarrinho
            {
                Id = 3,
                Quantidade = 1,
                PrecoUnitario = 54.90m,
                CarrinhoId = 2,
                ProdutoId = 2
            },
            new ItemCarrinho
            {
                Id = 4,
                Quantidade = 1,
                PrecoUnitario = 58.90m,
                CarrinhoId = 2,
                ProdutoId = 5
            },
            new ItemCarrinho
            {
                Id = 5,
                Quantidade = 1,
                PrecoUnitario = 59.90m,
                CarrinhoId = 4,
                ProdutoId = 1
            },
            new ItemCarrinho
            {
                Id = 6,
                Quantidade = 1,
                PrecoUnitario = 62.90m,
                CarrinhoId = 4,
                ProdutoId = 4
            },
            new ItemCarrinho
            {
                Id = 7,
                Quantidade = 2,
                PrecoUnitario = 56.90m,
                CarrinhoId = 4,
                ProdutoId = 7
            },
            new ItemCarrinho
            {
                Id = 8,
                Quantidade = 1,
                PrecoUnitario = 64.90m,
                CarrinhoId = 5,
                ProdutoId = 3
            },
            new ItemCarrinho
            {
                Id = 9,
                Quantidade = 1,
                PrecoUnitario = 69.90m,
                CarrinhoId = 3,
                ProdutoId = 8
            },
            new ItemCarrinho
            {
                Id = 10,
                Quantidade = 2,
                PrecoUnitario = 52.90m,
                CarrinhoId = 6,
                ProdutoId = 9
            },
            new ItemCarrinho
            {
                Id = 11,
                Quantidade = 1,
                PrecoUnitario = 61.90m,
                CarrinhoId = 7,
                ProdutoId = 10
            },
            new ItemCarrinho
            {
                Id = 12,
                Quantidade = 1,
                PrecoUnitario = 55.90m,
                CarrinhoId = 3,
                ProdutoId = 11
            },
            new ItemCarrinho
            {
                Id = 13,
                Quantidade = 1,
                PrecoUnitario = 47.90m,
                CarrinhoId = 8,
                ProdutoId = 12
            },
            new ItemCarrinho
            {
                Id = 14,
                Quantidade = 1,
                PrecoUnitario = 53.90m,
                CarrinhoId = 2,
                ProdutoId = 13
            },
            new ItemCarrinho
            {
                Id = 15,
                Quantidade = 1,
                PrecoUnitario = 58.90m,
                CarrinhoId = 1,
                ProdutoId = 14
            },
            new ItemCarrinho
            {
                Id = 16,
                Quantidade = 2,
                PrecoUnitario = 49.90m,
                CarrinhoId = 5,
                ProdutoId = 15
            },
            new ItemCarrinho
            {
                Id = 17,
                Quantidade = 1,
                PrecoUnitario = 54.90m,
                CarrinhoId = 6,
                ProdutoId = 16
            },
            new ItemCarrinho
            {
                Id = 18,
                Quantidade = 1,
                PrecoUnitario = 69.90m,
                CarrinhoId = 7,
                ProdutoId = 8
            },
            new ItemCarrinho
            {
                Id = 19,
                Quantidade = 1,
                PrecoUnitario = 47.90m,
                CarrinhoId = 3,
                ProdutoId = 12
            },
            new ItemCarrinho
            {
                Id = 20,
                Quantidade = 1,
                PrecoUnitario = 52.90m,
                CarrinhoId = 8,
                ProdutoId = 9
            }
        );

        // Desafios e dicas 
        modelBuilder.Entity<Desafio>().HasData(
            new Desafio
            {
                Id = 1,
                Nome = DesafiosEnum.LoginAdmin.GetNomeDisplay(),
                Descricao = "Acesse uma conta administrativa utilizando SQL Injection.",
                DescricaoDetalhes = "A consulta SQL que busca o usuário no banco de dados não trata corretamente a entrada de parâmetros, permitindo que seja possível injetar código SQL malicioso. O payload |' OR p.Nome = \"Administrador\";-- |, por exemplo, utiliza a aspas simples para quebrar a string da consulta, o operador OR adiciona mais uma condição ao Select e \";-- \" finaliza a consulta e transforma o restante da linha em um comentário. Isso permite que você acesse a conta de um usuário com perfil administrativo sem precisar conhecer a senha.",
                Categoria = CategoriaDesafio.SqlInjection,
                Dificuldade = 2,
                Rota = RotasEnum.Login.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 2,
                Nome = DesafiosEnum.DomXss.GetNomeDisplay(),
                Descricao = "Utilize o payload |<iframe src=\"javascript:alert(`XSS`)\">| para causar um ataque de DOM XSS na página do catalogo.",
                DescricaoDetalhes = "O campo de busca do catálogo reflete, no próprio DOM do navegador, o termo digitado sem qualquer tratamento. A entrada do usuário é inserida diretamente no HTML da página, permitindo que tags e atributos perigosos sejam interpretados pelo navegador. Como a manipulação acontece inteiramente no lado do cliente, nenhum dado malicioso precisa trafegar pelo servidor: tudo ocorre no DOM. O payload |<iframe src=\"javascript:alert(`XSS`)\">|, por exemplo, é interpretado ao ser renderizado no catálogo, comprovando uma injeção de HTML tipicamente conhecida como DOM XSS.",
                Categoria = CategoriaDesafio.DomXSS,
                Dificuldade = 2,
                Rota = RotasEnum.Catalogo.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 3,
                Nome = DesafiosEnum.BruteForceLogin.GetNomeDisplay(),
                Descricao = "Acesse a conta de um dos usuários do domínio @safemugs.com utilizando força bruta.",
                DescricaoDetalhes = "O fluxo de login do SafeMugs não implementa nenhum mecanismo de antiautomação: não há limite de tentativas por conta, bloqueio temporário, captcha nem atraso entre requisições. Conhecendo um e-mail válido do domínio |@safemugs.com|, é possível automatizar tentativas consecutivas de senha até acertar. As senhas dos usuários comuns são fracas e baseadas em padrões conhecidos (como 'welcome123' ou 'password123'), o que torna viável um ataque usando uma lista de palavras. Sem qualquer controle de velocidade, um script consegue testar milhares de combinações em poucos segundos, obtendo acesso a uma conta legítima.",
                Categoria = CategoriaDesafio.BrokenAntiAutomation,
                Dificuldade = 3,
                Rota = RotasEnum.Login.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 4,
                Nome = DesafiosEnum.CadastroInvalido.GetNomeDisplay(),
                Descricao = "Tente burlar a validação do formulário de cadastro.",
                DescricaoDetalhes = "O formulário de cadastro valida os dados apenas no navegador. No servidor, a checagem de formato do e-mail é insuficiente e não impede registros com valores malformados. Ao interceptar a requisição de cadastro e enviar um e-mail em formato inválido (como |ana@@safemugs..com|), o servidor aceita o registro. Isso demonstra que a validação do front-end pode ser facilmente contornada e que o servidor não deve confiar nos dados recebidos, precisando aplicar regras de formato e sanitização por conta própria.",
                Categoria = CategoriaDesafio.ImproperInputValidation,
                Dificuldade = 2,
                Rota = RotasEnum.Cadastro.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 5,
                Nome = DesafiosEnum.ManipularCadastro.GetNomeDisplay(),
                Descricao = "Crie uma conta de administrador.",
                DescricaoDetalhes = "O cadastro de um novo usuário é definido, em parte, por um parâmetro que indica o perfil da conta. Embora esse campo não apareça no formulário visível, ele trafega na requisição. Ao interceptar o cadastro e incluir |\"perfil\":\"Administrador\"| no corpo enviado, a conta é criada com privilégios administrativos, sem que o servidor questione o nível solicitado. Confiam-se cegamente nos parâmetros informados pelo cliente, o que permite a escalação de privilégio direta via adulteração de parâmetro.",
                Categoria = CategoriaDesafio.ParameterTampering,
                Dificuldade = 3,
                Rota = RotasEnum.Cadastro.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 6,
                Nome = DesafiosEnum.AlterarComentarioOutroUsuario.GetNomeDisplay(),
                Descricao = "Edite um comentário que não pertence ao seu usuário.",
                DescricaoDetalhes = "A requisição que atualiza um comentário recebe apenas o identificador do comentário e o novo texto, sem verificar se ele pertence ao usuário autenticado. Ao interceptar a edição e trocar o identificador informado pelo de um comentário alheio, é possível alterá-lo livremente. O servidor não valida a posse do recurso, confiando apenas no parâmetro recebido, o que caracteriza uma Insecure Direct Object Reference (IDOR).",
                Categoria = CategoriaDesafio.IDOR,
                Dificuldade = 3,
                Rota = RotasEnum.Catalogo.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 7,
                Nome = DesafiosEnum.StoredXssComentarios.GetNomeDisplay(),
                Descricao = "Utilize o payload |<iframe src=\"javascript:alert(`XSS`)\">| para causar um ataque de Stored XSS na página do catalogo.",
                DescricaoDetalhes = "Na tela de detalhes do produto, os comentários deixados pelos usuários são renderizados diretamente no HTML, sem qualquer sanitização. Como o comentário é salvo no banco de dados e exibido a todos que abrem o produto, enviar o payload |<iframe src=\"javascript:alert(`XSS`)\">| como texto do comentário faz com que o código seja armazenado e, posteriormente, interpretado no navegador de qualquer pessoa que visualize a página. Diferente do DOM XSS, aqui o conteúdo malicioso persiste no servidor, atingindo múltiplas vítimas.",
                Categoria = CategoriaDesafio.StoredXSS,
                Dificuldade = 3,
                Rota = RotasEnum.Catalogo.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 8,
                Nome = DesafiosEnum.CriarComentarioOutroUsuario.GetNomeDisplay(),
                Descricao = "Escreva um comentário que não pertence ao seu usuário.",
                DescricaoDetalhes = "Todo comentário enviado ao servidor carrega um campo que identifica o autor a ser atribuído a ele. O servidor não valida se esse nome corresponde ao usuário autenticado: ele apenas procura o usuário pelo nome informado e o vincula ao comentário. Interceptando a requisição de criação e alterando o nome pelo de outro usuário (por exemplo |Bruno Costa|), é possível registrar um comentário em nome de outra pessoa, simulando uma identidade indevida.",
                Categoria = CategoriaDesafio.IDOR,
                Dificuldade = 2,
                Rota = RotasEnum.Catalogo.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 9,
                Nome = DesafiosEnum.EncontrarScoreBoard.GetNomeDisplay(),
                Descricao = "Localize a página de score-board.",
                DescricaoDetalhes = "O SafeMugs possui uma página oculta que concentra o acompanhamento dos desafios e o progresso do jogador, sem estar referenciada em nenhum menu ou link visível. Para localizá-la, é preciso explorar nomes de rotas comuns e deduzir a URL por força bruta, ou inspecionar o código javascript da página. O objetivo aqui é treinar a descoberta do domínio sendo testado, prática comum em avaliações de segurança.",
                Categoria = CategoriaDesafio.Outros,
                Dificuldade = 1,
                Rota = RotasEnum.ScoreBoard.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 10,
                Nome = DesafiosEnum.TratamentoErro.GetNomeDisplay(),
                Descricao = "Provoque um erro que o retorno da API não trata corretamente.",
                DescricaoDetalhes = "Quando ocorre um erro inesperado no SafeMugs, a API retorna ao cliente uma resposta contendo o nome da classe da exceção e a mensagem original, frequentemente acompanhada de detalhes internos como nomes de tabelas, consultas SQL e trechos de código. Ao provocar uma falha no backend, é possível extrair dessa resposta informações sensíveis sobre a implementação. Um exemplo é inserir uma aspa simples no e-mail do login (|'|) para quebrar a consulta SQL e forçar uma exceção não tratada, expondo os detalhes técnicos no corpo retornado.",
                Categoria = CategoriaDesafio.SecurityMisconfiguration,
                Dificuldade = 1,
                Rota = null,
                Resolvido = false
            },
            new Desafio
            {
                Id = 11,
                Nome = DesafiosEnum.EnumerarTabelasBanco.GetNomeDisplay(),
                Descricao = "Obtenha o nome de todas as tabelas do banco de dados do SafeMugs.",
                DescricaoDetalhes = "A consulta SQL que pesquisa pedidos por número concatena o termo digitado diretamente na string da consulta, permitindo injeção de SQL. Como a consulta original retorna seis colunas, um |UNION SELECT| anexado ao payload só é aceito se trouxer exatamente o mesmo número de colunas, e as mensagens de erro da API expõem quando essa contagem está errada, funcionando como um oráculo para descobri-la. O MySQL mantém os metadados do banco na view |information_schema.tables|: selecionar a coluna |TABLE_NAME| com o filtro |WHERE TABLE_SCHEMA = DATABASE()| retorna o nome de todas as tabelas do esquema atual. O payload |' UNION SELECT 1,TABLE_NAME,3,4,CURDATE(),6 FROM information_schema.tables WHERE TABLE_SCHEMA = DATABASE()%23|, por exemplo, lista todas as tabelas na coluna do número do pedido, enquanto o |%23| ('#' encodado para URL) transforma o restante da linha em comentário, neutralizando o filtro que restringiria os resultados ao usuário autenticado.",
                Categoria = CategoriaDesafio.SqlInjection,
                Dificuldade = 4,
                Rota = RotasEnum.Pedidos.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 12,
                Nome = DesafiosEnum.AdicionarProdutoIndisponivel.GetNomeDisplay(),
                Descricao = "Adicione a um carrinho um produto que não devia estar disponível.",
                DescricaoDetalhes = "Nem todos os produtos registrados no banco do SafeMugs aparecem no catálogo: existe um item com estoque zerado e marcado como inativo, invisível para qualquer cliente da loja. O fluxo que adiciona um produto ao carrinho recebe o identificador do produto no corpo da requisição e apenas verifica se ele existe no banco — não confere se o produto está ativo nem se há estoque disponível. Para explorá-lo, primeiro é preciso descobrir o produto oculto: consultando o carrinho de outro usuário pela rota |/api/carrinho/{usuarioId}|, que expõe todos os itens e seus produtos, ou extraindo os registros da tabela de produtos com a injeção de SQL da pesquisa de pedidos. Com o identificador em mãos, basta interceptar a requisição |PATCH /api/carrinho/adicionar| e substituir o |produtoId| pelo do produto indisponível: o servidor aceita o item e o adiciona ao carrinho sem qualquer questionamento. O backend confia cegamente que o cliente só referencia produtos que podem ser vendidos, quando deveria validar o estado do recurso — ativo e com estoque — antes de aceitá-lo em qualquer operação, caracterizando uma validação de entrada imprópria no servidor.",
                Categoria = CategoriaDesafio.ImproperInputValidation,
                Dificuldade = 3,
                Rota = RotasEnum.Carrinho.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 13,
                Nome = DesafiosEnum.VisualizarCarrinhoAlheio.GetNomeDisplay(),
                Descricao = "Visualize o carrinho de compras de outro usuário.",
                DescricaoDetalhes = "A rota que busca o carrinho de compras recebe o identificador do usuário diretamente pela URL e o utiliza para localizar o carrinho, sem verificar se ele corresponde ao usuário autenticado. Ao interceptar a requisição que a tela de carrinho dispara e trocar o identificador pelo de outro cliente, o servidor devolve o carrinho completo do alvo: itens, quantidades e preços — acompanhados dos dados pessoais do dono, como nome, e-mail, telefone e endereço.\nO servidor confia que o parâmetro recebido na URL identifica o próprio dono do recurso, quando deveria validar que ele corresponde ao usuário autenticado antes de retornar qualquer dado, caracterizando uma Insecure Direct Object Reference (IDOR).",
                Categoria = CategoriaDesafio.IDOR,
                Dificuldade = 2,
                Rota = RotasEnum.Carrinho.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 14,
                Nome = DesafiosEnum.ModificarCarrinhoAlheio.GetNomeDisplay(),
                Descricao = "Modifique o carrinho de compras de outro usuário.",
                DescricaoDetalhes = "As requisições que alteram o carrinho — adicionar e remover produtos — carregam o identificador do dono no corpo da requisição, e o servidor o utiliza diretamente para localizar o carrinho a ser modificado, sem verificar se ele corresponde ao usuário autenticado. Ao interceptar a requisição que a tela de carrinho dispara e trocar o identificador pelo de outro cliente, é possível adicionar produtos ao carrinho alheio ou remover itens que ele continha — inclusive esvaziá-lo por completo.\nO servidor confia que o identificador recebido no corpo da requisição identifica o próprio dono do recurso, quando deveria validar que ele corresponde ao usuário autenticado antes de alterar qualquer dado, caracterizando uma Insecure Direct Object Reference (IDOR).",
                Categoria = CategoriaDesafio.IDOR,
                Dificuldade = 2,
                Rota = RotasEnum.Carrinho.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 15,
                Nome = DesafiosEnum.BurlarAllowlistRedirect.GetNomeDisplay(),
                Descricao = "Faça a API redirecionar para um domínio que não seja o GitHub, burlando a allowlist de redirect.",
                DescricaoDetalhes = "O botão do GitHub na navegação lateral não abre o link diretamente: ele chama o endpoint |/api/redirect?url=...|, que deve garantir que o destino seja um host confiável antes de redirecionar. Para isso, o backend mantém uma allowlist com o host permitido e valida a URL recebida checando se ela contém o host como substring — ou seja, basta que a string |https://github.com| apareça em qualquer posição da URL para que ela seja considerada segura. Como essa checagem ignora a estrutura real da URL, é possível embutir o host permitido em posições que não correspondem ao destino efetivo. Em uma URL, o trecho antes do |@| é tratado como informação de usuário e o host vem depois dele: o payload |https://github.com@evil.com| passa pela allowlist (contém |https://github.com|), mas o navegador direciona ao host |evil.com|. Outro bypass análogo usa a query string — |https://evil.com?https://github.com| — em que o host permitido aparece após o |?|, como parte dos parâmetros, sem identificar o host real. Um redirect confiável deve analisar a URL como uma estrutura (parseando host, esquema e caminho) e comparar o host efetivo contra a allowlist, nunca realizar um simples casamento de substring.",
                Categoria = CategoriaDesafio.OpenRedirect,
                Dificuldade = 3,
                Rota = null,
                Resolvido = false
            },
            new Desafio
            {
                Id = 16,
                Nome = DesafiosEnum.SsrfUploadFotoPerfil.GetNomeDisplay(),
                Descricao = "Alcance o endpoint interno |/api/usuario/requisicao-interna|, que só responde a requisições originadas pelo próprio servidor.",
                DescricaoDetalhes = "Existe um endpoint interno em |/api/usuario/requisicao-interna| que valida um header secreto adicionado apenas quando a requisição é disparada pelo próprio backend — por isso não pode ser alcançado diretamente pelo navegador ou pelo Burp. O caminho é induzir o servidor a emitir uma requisição HTTP para uma URL controlada por você, apontando-a para o loopback. O payload esperado é |http://127.0.0.1:8080/api/usuario/requisicao-interna|: o servidor faz a requisição em seu nome, alcança o endpoint interno e o desafio é marcado.",
                Categoria = CategoriaDesafio.ServerSideRequestForgery,
                Dificuldade = 3,
                Rota = RotasEnum.Perfil.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 17,
                Nome = DesafiosEnum.PathTraversalFotoPerfil.GetNomeDisplay(),
                Descricao = "Sobrescreva a imagem de um dos produtos exibidos no catálogo da loja.",
                DescricaoDetalhes = "O catálogo exibe imagens guardadas na pasta |/imagens/produto|, cada uma referenciada por um produto do banco. Um endpoint que recebe o upload de um arquivo monta o caminho de destino concatenando o nome do arquivo informado pelo cliente diretamente em Path.Combine, sem sanitizar separadores nem segmentos |..|. Como |..| permite navegar para cima na árvore de diretórios, basta interceptar o upload e trocar o metadado filename por |../../produto/mug_behappy.jpg| (ou outra imagem real da pasta) para que o arquivo enviado sobrescreva a imagem daquele produto.",
                Categoria = CategoriaDesafio.PathTraversal,
                Dificuldade = 4,
                Rota = RotasEnum.Perfil.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 18,
                Nome = DesafiosEnum.ReflectedXss.GetNomeDisplay(),
                Descricao = "Faça a API refletir, no corpo de uma resposta, o payload |<iframe src=\"javascript:alert(`XSS`)\">| de uma forma que ele seja executado no navegador do cliente.",
                DescricaoDetalhes = "Ao buscar um produto pelo nome, a API recebe o termo pesquisado como parâmetro da requisição. Quando o produto não é encontrado, a resposta de erro devolve esse mesmo termo de volta, sem tratamento, dentro da mensagem. No cliente, esse conteúdo refletido é inserido diretamente na página sem sanitização, de modo que qualquer HTML presente no termo é interpretado pelo navegador. O payload |?nome=<iframe src=\"javascript:alert(`XSS`)\">|, por exemplo, faz o servidor devolver o |<iframe>| na resposta, que então é renderizado e executado. Por se tratar de um conteúdo que parte do servidor e volta no corpo da resposta, trata-se de um Reflected XSS — diferente do DOM XSS, em que o payload nunca sai do navegador.",
                Categoria = CategoriaDesafio.ReflectedXSS,
                Dificuldade = 3,
                Rota = RotasEnum.AvaliacoesProduto.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 19,
                Nome = DesafiosEnum.StoredXssAvaliacoes.GetNomeDisplay(),
                Descricao = "Utilize o payload |<iframe src=\"javascript:alert(`XSS`)\">| para causar um ataque de Stored XSS na página de avaliações.",
                DescricaoDetalhes = "Na página de avaliações do produto, o e-mail do autor de cada avaliação é renderizado diretamente no HTML sem sanitização. O e-mail é um dado persistido da conta do usuário e exibido a qualquer visitante que abra as avaliações daquele produto. Ao interceptar a requisição de cadastro e registrar um usuário com o e-mail |<iframe src=\"javascript:alert(`XSS`)\">|, depois publicar uma avaliação com essa conta, o payload fica armazenado no banco e é interpretado no navegador de quem visualiza a página em |/avaliacoes-produto?nome=...|. Diferente do Reflected XSS, aqui o conteúdo malicioso persiste no servidor e atinge múltiplas vítimas, caracterizando Stored XSS via campo de e-mail.",
                Categoria = CategoriaDesafio.StoredXSS,
                Dificuldade = 4,
                Rota = RotasEnum.AvaliacoesProduto.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 20,
                Nome = DesafiosEnum.CriarAvaliacaoOutroUsuario.GetNomeDisplay(),
                Descricao = "Crie uma avaliação em nome de outro usuário.",
                DescricaoDetalhes = "Ao criar uma avaliação, o cliente envia no corpo da requisição o identificador do autor (usuarioId) junto com nota e comentario para POST |/api/produtos/avaliacoes/{produtoId}|. O servidor apenas verifica se o usuário informado existe e o vincula à avaliação, sem conferir se corresponde ao usuário autenticado. Interceptando a requisição disparada pelo diálogo de avaliação e trocando usuarioId pelo de outro usuário, é possível registrar uma avaliação em nome de outra pessoa.",
                Categoria = CategoriaDesafio.IDOR,
                Dificuldade = 2,
                Rota = RotasEnum.AvaliacoesProduto.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 21,
                Nome = DesafiosEnum.CriarAvaliacaoZeroEstrelas.GetNomeDisplay(),
                Descricao = "Envie uma avaliação com nota zero.",
                DescricaoDetalhes = "A interface de avaliação permite apenas notas de 0,5 a 5, mas o servidor não valida o intervalo. Interceptando POST |/api/produtos/avaliacoes/{produtoId}| e enviando nota com valor 0, a avaliação é persistida com zero estrelas.",
                Categoria = CategoriaDesafio.ImproperInputValidation,
                Dificuldade = 2,
                Rota = RotasEnum.AvaliacoesProduto.GetNomeDisplay(),
                Resolvido = false
            },
            new Desafio
            {
                Id = 22,
                Nome = DesafiosEnum.SsrfServicoInternoOculto.GetNomeDisplay(),
                Descricao = "Descubra o serviço escondido em uma porta interna do servidor e recupere o segredo que ele guarda.",
                DescricaoDetalhes = "O campo de URL da foto de perfil faz o próprio backend buscar o endereço informado — inclusive endereços que apontam para o próprio servidor. Isso permite usá-lo como sonda para enumerar portas internas: uma porta fechada devolve um erro controlado de conexão, enquanto uma porta em que algum serviço responde devolve uma resposta HTTP; quando essa resposta não é uma imagem, o corpo é refletido na mensagem de erro da requisição. Varrendo portas comuns de serviços web encontra-se um listener interno em |http://127.0.0.1:9090|, não publicado no host, cujo GET na raiz devolve em texto puro um segredo usado internamente pela aplicação. Como a porta não é exposta externamente, nenhum scan externo a encontra: a descoberta só é possível induzindo o próprio servidor a fazer a requisição — um Server-Side Request Forgery usado como varredura de portas internas.",
                Categoria = CategoriaDesafio.ServerSideRequestForgery,
                Dificuldade = 5,
                Rota = RotasEnum.Perfil.GetNomeDisplay(),
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
                Texto = "A validação dos campos pode ser insuficiente no lado do servidor.",
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
                Texto = "Observe o corpo da requisição de editar comentário.",
                DesafioId = 6
            },
            new DicaDesafio
            {
                Id = 14,
                NrDica = 2,
                Texto = "Intercepte a requisição de editar comentário para mudar o seu payload.",
                DesafioId = 6
            },

            new DicaDesafio
            {
                Id = 15,
                NrDica = 1,
                Texto = "O desafio está na seção de detalhes do produto.",
                DesafioId = 7
            },
            new DicaDesafio
            {
                Id = 16,
                NrDica = 2,
                Texto = "Comentários podem ser renderizados sem sanitização.",
                DesafioId = 7
            },

            new DicaDesafio
            {
                Id = 17,
                NrDica = 1,
                Texto = "O payload enviado ao backend pode estar transmitindo dados de forma insegura.",
                DesafioId = 8
            },
            new DicaDesafio
            {
                Id = 18,
                NrDica = 2,
                Texto = "Tente interceptar e modificar a requisição.",
                DesafioId = 8
            },

            new DicaDesafio
            {
                Id = 19,
                NrDica = 1,
                Texto = "Este desafio pode ser resolvido a partir de diferentes telas.",
                DesafioId = 10
            },
            new DicaDesafio
            {
                Id = 20,
                NrDica = 2,
                Texto = "Tente inserir valores inesperados em formulários que possam provocar um erro no backend.",
                DesafioId = 10
            },
            new DicaDesafio
            {
                Id = 21,
                NrDica = 3,
                Texto = "Tente quebrar a consulta SQL realizada a partir da tela de Login.",
                DesafioId = 10
            },

            new DicaDesafio
            {
                Id = 22,
                NrDica = 1,
                Texto = "A consulta que pesquisa pedidos por número concatena o termo pesquisado diretamente na string SQL.",
                DesafioId = 11
            },
            new DicaDesafio
            {
                Id = 23,
                NrDica = 2,
                Texto = "Um UNION SELECT só é aceito com o mesmo número de colunas da consulta original. As mensagens de erro da API indicam quando essa contagem está errada.",
                DesafioId = 11
            },
            new DicaDesafio
            {
                Id = 24,
                NrDica = 3,
                Texto = "Os nomes das tabelas ficam na coluna TABLE_NAME da view information_schema.tables. Finalize o payload com # (ou -- seguido de espaço) para comentar o restante da consulta.",
                DesafioId = 11
            },

            new DicaDesafio
            {
                Id = 25,
                NrDica = 1,
                Texto = "Nem todos os produtos registrados no banco aparecem no catálogo da loja.",
                DesafioId = 12
            },
            new DicaDesafio
            {
                Id = 26,
                NrDica = 2,
                Texto = "Existe um endpoint que revela indevidamente os produtos que participaram de outras operações do sistema",
                DesafioId = 12
            },
            new DicaDesafio
            {
                Id = 27,
                NrDica = 3,
                Texto = "Os carrinhos de outros usuários podem revelar produtos que não estão à venda.",
                DesafioId = 12
            },
            new DicaDesafio
            {
                Id = 28,
                NrDica = 4,
                Texto = "Intercepte a requisição que adiciona um produto ao carrinho e troque o produtoId pelo do produto indisponível.",
                DesafioId = 12
            },

            new DicaDesafio
            {
                Id = 29,
                NrDica = 1,
                Texto = "A rota que busca o carrinho recebe o identificador do usuário diretamente pela URL.",
                DesafioId = 13
            },
            new DicaDesafio
            {
                Id = 30,
                NrDica = 2,
                Texto = "Troque o identificador da requisição pelo de outro usuário e observe a resposta.",
                DesafioId = 13
            },

            new DicaDesafio
            {
                Id = 31,
                NrDica = 1,
                Texto = "As requisições que adicionam e removem produtos do carrinho carregam o identificador do usuário no corpo.",
                DesafioId = 14
            },
            new DicaDesafio
            {
                Id = 32,
                NrDica = 2,
                Texto = "Troque o identificador pelo de outro usuário e observe o carrinho dele mudar.",
                DesafioId = 14
            },

            new DicaDesafio
            {
                Id = 33,
                NrDica = 1,
                Texto = "A allowlist confere apenas se a URL contém o host permitido como substring, sem examinar o destino real.",
                DesafioId = 15
            },
            new DicaDesafio
            {
                Id = 34,
                NrDica = 2,
                Texto = "Pense em como embutir o host permitido dentro de uma URL que aponta para outro domínio.",
                DesafioId = 15
            },
            new DicaDesafio
            {
                Id = 35,
                NrDica = 3,
                Texto = "Em uma URL, o segmento antes do @ é tratado como informação de usuário e o host vem depois.",
                DesafioId = 15
            },
            new DicaDesafio
            {
                Id = 36,
                NrDica = 1,
                Texto = "Procure um local da aplicação onde é possível induzir o servidor a fazer uma requisição a uma url escolhida por você.",
                DesafioId = 16
            },
            new DicaDesafio
            {
                Id = 37,
                NrDica = 2,
                Texto = "Na tela de perfil, o campo de upload de foto por URL faz o próprio servidor baixar a imagem informada, sem validar o destino.",
                DesafioId = 16
            },
            new DicaDesafio
            {
                Id = 38,
                NrDica = 1,
                Texto = "Procure uma funcionalidade do sistema que te permite fazer uploads de imagens.",
                DesafioId = 17
            },
            new DicaDesafio
            {
                Id = 39,
                NrDica = 2,
                Texto = "Na tela de perfil, o upload de foto de perfil monta o caminho de destino a partir do nome do arquivo enviado pelo cliente, sem sanitizar.",
                DesafioId = 17
            },
            new DicaDesafio
            {
                Id = 40,
                NrDica = 1,
                Texto = "Procure uma funcionalidade de busca que devolva, na resposta, algo que você enviou na requisição.",
                DesafioId = 18
            },
            new DicaDesafio
            {
                Id = 41,
                NrDica = 1,
                Texto = "O campo vulnerável não é o comentário da avaliação.",
                DesafioId = 19
            },
            new DicaDesafio
            {
                Id = 42,
                NrDica = 2,
                Texto = "O dado que foi mal sanitizado faz parte do cadastro de usuários.",
                DesafioId = 19
            },
            new DicaDesafio
            {
                Id = 43,
                NrDica = 1,
                Texto = "O autor da avaliação vem do corpo da requisição, não da sessão autenticada.",
                DesafioId = 20
            },
            new DicaDesafio
            {
                Id = 44,
                NrDica = 1,
                Texto = "A validação da nota acontece só no navegador.",
                DesafioId = 21
            },

            new DicaDesafio
            {
                Id = 45,
                NrDica = 1,
                Texto = "O campo de URL da foto de perfil faz o próprio servidor buscar o endereço informado — inclusive endereços que apontam para ele mesmo.",
                DesafioId = 22
            },
            new DicaDesafio
            {
                Id = 46,
                NrDica = 2,
                Texto = "Compare a resposta da API para uma porta fechada com a resposta de uma porta em que algum serviço responde.",
                DesafioId = 22
            },
            new DicaDesafio
            {
                Id = 47,
                NrDica = 3,
                Texto = "Serviços web internos costumam escutar em portas como 8080, 8443 e 9090. Faça um GET nelas pelo campo de URL.",
                DesafioId = 22
            }
        );

        #endregion
    }
}
