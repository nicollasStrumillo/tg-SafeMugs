using System.ComponentModel.DataAnnotations;

namespace backend.models.Enums;

public enum CategoriaDesafio
{
    [Display(
        Name = "SQL Injection", 
        Description = "Ocorre quando dados fornecidos pelo usuário são inseridos diretamente em uma consulta SQL sem o tratamento adequado. Isso pode permitir que um atacante altere a consulta original, acesse informações indevidas, modifique dados ou execute comandos não previstos pela aplicação."
    )]
    SqlInjection,

    [Display(
        Name = "Reflected XSS",
        Description = "Ocorre quando dados enviados pelo usuário são imediatamente refletidos na resposta da aplicação sem a devida validação ou codificação. O código malicioso normalmente é enviado por meio de um link ou formulário e é executado apenas quando a vítima acessa essa requisição específica."
    )]
    ReflectedXSS,

    [Display(
        Name = "Stored XSS",
        Description = "Ocorre quando o código malicioso é armazenado pela própria aplicação, por exemplo em um banco de dados, comentário ou perfil de usuário. Sempre que esse conteúdo é exibido para outras pessoas sem o tratamento adequado, o script é executado automaticamente no navegador das vítimas."
    )]
    StoredXSS,

    [Display(
        Name = "DOM XSS",
        Description = "Ocorre quando o JavaScript da aplicação manipula dados controlados pelo usuário diretamente no DOM da página, sem validação ou codificação. Nesse caso, a vulnerabilidade acontece inteiramente no navegador, sem que o servidor precise retornar o código malicioso na resposta."
    )]
    DomXSS,

    [Display(
        Name = "Broken Anti-Automation",
        Description = "Ocorre quando a aplicação não possui mecanismos adequados para impedir ações automatizadas, como tentativas repetidas de login, cadastro ou envio de requisições."
    )]
    BrokenAntiAutomation,

    [Display(
        Name = "Security Misconfiguration",
        Description = "Ocorre quando a aplicação, o servidor ou seus componentes são configurados de forma insegura. Configurações incorretas, recursos desnecessários habilitados, mensagens de erro detalhadas ou permissões excessivas podem expor informações sensíveis e facilitar outros ataques."
    )]
    SecurityMisconfiguration,

    [Display(
        Name = "Broken Authentication",
        Description = "Ocorre quando o processo de autenticação ou gerenciamento de sessões apresenta falhas que permitem a um atacante se passar por outro usuário. Senhas fracas, validações inadequadas, sessões previsíveis ou tokens inseguros podem resultar em acesso não autorizado à aplicação."
    )]
    BrokenAuthentication,

    [Display(
        Name = "XXE",
        Description = "Ocorre quando um processador XML aceita entidades externas sem as devidas restrições. Um atacante pode explorar esse comportamento para ler arquivos do servidor, acessar recursos internos da rede ou provocar negação de serviço."
    )]
    XXE,

    [Display(
        Name = "Insecure Deserialization",
        Description = "Ocorre quando a aplicação desserializa dados fornecidos pelo usuário sem validação adequada. Um atacante pode manipular esses dados para alterar o comportamento da aplicação, executar ações não autorizadas ou, em alguns casos, executar código malicioso."
    )]
    InsecureDeserialization,

    [Display(
        Name = "IDOR",
        Description = "Ocorre quando a aplicação permite o acesso direto a objetos ou recursos utilizando identificadores previsíveis, sem verificar se o usuário possui autorização."
    )]
    IDOR,

    [Display(
        Name = "Excessive Data Exposure",
        Description = "Ocorre quando a aplicação retorna mais informações do que o necessário em suas respostas. Mesmo que a interface exiba apenas parte dos dados, informações sensíveis podem ser expostas em APIs ou respostas HTTP e utilizadas por um atacante."
    )]
    ExcessiveDataExposure,

    [Display(
        Name = "Improper Input Validation",
        Description = "Ocorre quando a aplicação não valida corretamente os dados recebidos do usuário. Isso permite o envio de valores inválidos, inesperados ou maliciosos, podendo resultar em falhas de segurança, corrupção de dados ou exploração de outras vulnerabilidades."
    )]
    ImproperInputValidation,

    [Display(
        Name = "Parameter Tampering",
        Description = "Ocorre quando um atacante modifica parâmetros enviados pela aplicação, como valores em URLs, formulários, cookies ou requisições HTTP, para alterar seu comportamento. Sem validações no servidor, isso pode permitir acesso indevido a recursos, alteração de preços, privilégios ou outras regras de negócio."
    )]
    ParameterTampering,
    
    [Display(
        Name = "Outros",
        Description = "Desafios não relacionados a uma vulnerabilidade específica."
    )]
    Outros
}