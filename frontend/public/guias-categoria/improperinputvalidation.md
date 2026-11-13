# Improper Input Validation
Ocorre quando a aplicação não valida corretamente os dados recebidos do usuário. Isso permite o envio de valores inválidos, inesperados ou maliciosos, podendo resultar em falhas de segurança, corrupção de dados ou exploração de outras vulnerabilidades.

Por ser uma categoria ampla, ela está na raiz de muitas outras: as injeções (SQL, comandos) e o XSS são, no fundo, casos em que uma entrada não validada acaba sendo tratada como código. Aqui, o foco está na falha em si: a aplicação confia que o dado virá no formato, no tamanho e no intervalo esperados, sem nunca conferir.

## Exploração
Observe este formulário de uma loja, que adiciona um item ao carrinho:
![improperinputvalidation-imagem1](/guias-categoria/imagens/improperinputvalidation-imagem1.png) 

Considere que a tela limite a quantidade de 1 a 10 por meio do próprio HTML:
```html
<input type="number" name="quantidade" min="1" max="10" value="2">
```
E que o servidor, ao receber o pedido, calcule o valor do item e o some ao total do carrinho, desta maneira:
```text
subtotal = preco * quantidade
total_carrinho = total_carrinho + subtotal
```
Onde `quantidade` é o valor enviado pelo usuário.

No uso normal, o navegador envia uma requisição como esta:
```http
POST /carrinho/adicionar
produtoId=15&quantidade=2
```
O problema é que os atributos `min` e `max` existem apenas no navegador, como uma facilidade para quem está digitando. O servidor nunca verifica se o valor recebido está dentro do intervalo permitido, e uma requisição pode ser montada diretamente, sem passar pela tela. Um usuário mal intencionado pode, então, enviar um valor que o formulário jamais permitiria:
```http
POST /carrinho/adicionar
produtoId=15&quantidade=-3
```
Considerando um produto de `40,00`, a conta feita pelo servidor passa a ser:
```text
subtotal = 40,00 * -3 = -120,00
```
- `produtoId=15` é um identificador legítimo, então a requisição parece normal.
- `quantidade=-3` nunca seria aceito pela tela, mas o servidor o processa como qualquer outro valor.
- O subtotal negativo é somado ao total do carrinho e abate o valor de outros itens. Dependendo da regra de negócio, o pedido pode ser fechado por um valor muito menor ou até gerar um valor a ser devolvido ao cliente.

### A validação feita só na tela não protege
Tudo o que é verificado no navegador (atributos como `min`, `max` e `maxlength`, ou validações em JavaScript) serve para orientar quem usa o formulário, mas não impede que o servidor receba outra coisa. Qualquer pessoa pode alterar a requisição antes de enviá-la ou criá-la por conta própria, e a aplicação só enxerga o que chegou, não de onde veio.

### O mesmo erro, outras consequências
A falha não se limita a números negativos. O efeito depende de qual campo não é validado e de onde o valor será usado:

- **Valores fora do intervalo:** zero, negativos ou muito grandes. Um número grande o bastante pode ultrapassar o limite do tipo numérico e "dar a volta", resultando em um valor negativo ou muito pequeno.
- **Texto maior que o esperado:** pode causar erros, truncar o dado ao ser gravado ou ocupar mais espaço do que o previsto.
- **Tipo inesperado:** enviar `abc` onde se espera um número costuma gerar um erro interno que, muitas vezes, também expõe detalhes do sistema na resposta.
- **Formatos inválidos:** datas no futuro como data de nascimento, CPFs ou e-mails inexistentes e identificadores com formato errado são gravados normalmente, corrompendo a base de dados.
- **Entrada tratada como código:** quando o valor sem validação chega a uma consulta SQL, a um comando do sistema ou ao HTML de uma página, a falha dá origem às categorias de injeção e de XSS.