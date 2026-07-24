using System.ComponentModel.DataAnnotations;

namespace backend.models.Enums;

public enum DesafiosEnum
{
    [Display(Name = "Login como Admin")]
    LoginAdmin,
    
    [Display(Name = "DOM XSS")]
    DomXss, 

    [Display(Name = "Brute force de login")]
    BruteForceLogin,

    [Display(Name = "Cadastro inválido")]
    CadastroInvalido,

    [Display(Name = "Manipular cadastro")]
    ManipularCadastro,

    [Display(Name = "Altere o comentário de outro usuário")]
    AlterarComentarioOutroUsuario,

    [Display(Name = "Stored XSS")]
    StoredXss,

    [Display(Name = "Crie um comentário por outro usuário")]
    CriarComentarioOutroUsuario,

    [Display(Name = "Encontrar a Score-Board")]
    EncontrarScoreBoard,

    [Display(Name = "Tratamento de Erro")]
    TratamentoErro
}
