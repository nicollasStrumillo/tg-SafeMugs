# Parameter Tampering
Ocorre quando um atacante modifica parâmetros enviados pela aplicação, como valores em URLs, formulários, cookies ou requisições HTTP, para alterar seu comportamento. Sem validações no servidor, isso pode permitir acesso indevido a recursos, alteração de preços, privilégios ou outras regras de negócio.

A falha nasce de um erro de confiança: a aplicação envia ao navegador um dado que deveria ser decidido por ela (um preço, um perfil de acesso, um desconto) e, depois, aceita de volta o valor devolvido sem conferir se ele continua sendo o que ela mesma enviou.

## Exploração
Observe esta tela de finalização de compra de uma loja:
![parametertampering-imagem1](/guias-categoria/imagens/parametertampering-imagem1.png)

Considere que o formulário desta tela seja montado pela aplicação desta maneira:
```html
<form action="/checkout" method="POST">
  <input type="hidden" name="produtoId" value="15">
  <input type="hidden" name="preco" value="89.90">
  <button type="submit">Comprar</button>
</form>
```
E que, ao receber o pedido, o servidor use diretamente os valores recebidos para cobrar o cliente:
```text
cobrar(preco)
```
Onde `preco` é o valor do campo enviado pelo navegador.

No uso normal, o navegador envia uma requisição como esta:
```http
POST /checkout
produtoId=15&preco=89.90
```
Os campos `type="hidden"` apenas não aparecem na tela, mas continuam sendo dados que o navegador mantém e que o usuário pode ver e alterar, seja pelas ferramentas de desenvolvedor do navegador (F12) ou interceptando a requisição antes de ela sair. Um usuário mal intencionado pode, então, mudar o valor antes do envio:
```http
POST /checkout
produtoId=15&preco=0.01
```
- `produtoId=15` continua sendo o produto legítimo, então a requisição parece normal.
- `preco=0.01` nunca seria enviado pela tela, mas o servidor não compara esse valor com o preço real do produto e o aceita como verdadeiro.
- A compra é concluída com o valor escolhido pelo atacante, e a aplicação registra a venda como se tudo estivesse correto.

### O usuário controla tudo o que sai do navegador
Qualquer parâmetro que passa pelo lado do cliente pode ser alterado, independentemente de onde esteja ou de como seja apresentado. Os lugares mais comuns são:

- **URL (query string):** um parâmetro como `?plano=basico` pode ser trocado por `?plano=premium` para acessar um recurso que o usuário não contratou.
- **Campos ocultos de formulários:** como o `preco` do exemplo acima, ou campos de desconto, frete e quantidade.
- **Cookies:** um cookie que guarda uma decisão de acesso pode ser editado, como no exemplo a seguir.
- **Corpo e cabeçalhos da requisição:** em APIs, os valores enviados em JSON ou em cabeçalhos também podem ser modificados livremente.

Veja o caso de um cookie que informa o perfil do usuário:
```http
GET /painel HTTP/1.1
Host: loja.exemplo.com
Cookie: sessao=abc123; perfil=usuario
```
Se a aplicação confiar nesse valor para decidir quem pode acessar o painel administrativo, basta alterá-lo:
```http
GET /painel HTTP/1.1
Host: loja.exemplo.com
Cookie: sessao=abc123; perfil=admin
```
- `sessao=abc123` continua sendo a sessão legítima do próprio atacante, então ele segue autenticado.
- `perfil=admin` altera a informação que a aplicação usa para decidir o que ele pode ver. Como o servidor não confirma o perfil em nenhum registro seu, o atacante passa a acessar uma área que não lhe pertence.

### Relação com outras categorias
Trocar um identificador para acessar dados de outras pessoas, como no IDOR, é um caso particular de Parameter Tampering. A diferença para o Improper Input Validation é o foco: lá, o problema é aceitar valores inválidos ou inesperados; aqui, os valores podem ser perfeitamente válidos (um preço de `0.01` é um número comum), mas a aplicação não deveria ter deixado essa decisão nas mãos do cliente.