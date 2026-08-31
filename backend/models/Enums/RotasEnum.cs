using System.ComponentModel.DataAnnotations;

namespace backend.models.Enums;

public enum RotasEnum
{
    [Display(Name = "/login")]
    Login,

    [Display(Name = "/register")]
    Cadastro,

    [Display(Name = "/senha")]
    EsqueciSenha,

    [Display(Name = "/catalogo")]
    Catalogo,

    [Display(Name = "/avaliacoes-produto")]
    AvaliacoesProduto,

    [Display(Name = "/carrinho")]
    Carrinho,

    [Display(Name = "/pedidos")]
    Pedidos,

    [Display(Name = "/perfil")]
    Perfil,
    
    [Display(Name = "/score-board")]
    ScoreBoard,
}
