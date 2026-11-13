# Server-Side Request Forgery (SSRF)

Ocorre quando a aplicação faz uma requisição HTTP para uma URL fornecida pelo usuário sem validar adequadamente o destino. Um atacante pode induzir o servidor a alcançar serviços internos ou recursos de rede inacessíveis externamente, executando requisições em nome da própria aplicação.

## Exploração

Observe esta funcionalidade de uma loja, que permite importar uma imagem de produto a partir de um endereço da internet: 
![serversiderequestforgery-imagem1.png](/guias-categoria/imagens/serversiderequestforgery-imagem1.png)

Considere que, para importar a imagem, o servidor acesse o endereço informado e guarde o conteúdo recebido, desta maneira:

```text
POST /importar-imagem
  resposta = requisicao_http(@url)
  salvar(resposta)
```

Onde `@url` é o endereço informado pelo usuário.

No uso normal, o servidor busca uma imagem de um site público:

```http
POST /importar-imagem
url=https://exemplo.com/foto.png
```

Se a aplicação não validar o destino, um usuário mal intencionado pode informar um endereço que só o servidor consegue alcançar:

```http
POST /importar-imagem
url=http://localhost:8080/admin
```

- A requisição não parte do computador do atacante, e sim do servidor da aplicação. Por isso, `localhost` aqui é o próprio servidor, e não a máquina de quem enviou o pedido.
- O endereço `localhost:8080/admin` aponta para um serviço que só aceita conexões internas, como um painel administrativo. De fora da rede ele estaria inacessível, mas, como o pedido vem de dentro, o serviço o trata como confiável.
- O conteúdo que o servidor recebe pode voltar ao atacante na resposta da aplicação, ou, mesmo quando nada é exibido, o pedido interno já foi executado.
