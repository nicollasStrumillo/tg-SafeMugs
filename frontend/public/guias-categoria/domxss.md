# DOM XSS
Variação do Cross-Site Scripting (XSS) em que a falha está no JavaScript executado no próprio navegador, e não na resposta enviada pelo servidor. O script da página lê um dado controlado pelo usuário (como a URL) e o escreve na página, sem o tratamento adequado, de uma forma que o navegador o interpreta como código. Como todo o processo acontece no navegador, o servidor pode nunca receber nem devolver o conteúdo malicioso. Assim como nas demais variações, o código injetado executa no contexto do site e pode roubar sessões, ler informações exibidas na página ou realizar ações em nome da vítima.
 
## Exploração
Observe esta página de busca, que exibe na tela o termo informado no final da URL (a parte depois do `#`):
![DomXss-imagem1](/guias-categoria/imagens/domxss-imagem1.png)
 
Considere que o servidor entregue sempre a mesma página, e que um script dessa página leia o termo da URL e o escreva no HTML, desta maneira:
```html
<h2 id="resultado"></h2>
<script>
  const termo = decodeURIComponent(location.hash.substring(1));
  document.getElementById("resultado").innerHTML = "Resultados para: " + termo;
</script>
```
Onde `location.hash` é a parte da URL a partir do `#` (no exemplo, `#caneca`) e `termo` é o valor extraído dela, controlado pelo usuário.
 
Se a escrita deste valor na página for insegura, um usuário mal intencionado pode compor uma entrada que utiliza da sintaxe do HTML para introduzir código que será executado pelo navegador.
 
Neste caso, um exemplo de entrada maliciosa seria `<img src=x onerror=alert('XSS')>`. O atacante monta um link com ele embutido:
```text
https://loja.exemplo.com/busca#<img src=x onerror=alert('XSS')>
```
Depois que o script escreve o valor com `innerHTML`, a página (já montada no navegador) fica assim:
```html
<h2 id="resultado">Resultados para: <img src=x onerror=alert('XSS')></h2>
```
- `<img src=x ...>` insere uma imagem cujo endereço (`x`) não existe, então o carregamento falha.
- `onerror=alert('XSS')` é um evento que o navegador dispara justamente quando o carregamento da imagem falha, e ele executa o código informado. O `alert` é apenas uma prova de conceito; no lugar dele poderia haver qualquer JavaScript, com os mesmos privilégios do site.
- Repare que não foi usada a tag `<script>`: scripts inseridos por meio de `innerHTML` não são executados pelo navegador, por isso nesse cenário o atacante recorre a eventos como `onerror`.
### O servidor não participa
A parte da URL depois do `#` (o fragmento) nunca é enviada ao servidor. Para o exemplo acima, a requisição feita pelo navegador seria apenas:
```http
GET /busca HTTP/1.1
Host: loja.exemplo.com
```
O servidor responde com a página de sempre, que contém apenas o script legítimo. O código malicioso existe somente no link e no navegador da vítima. É isso que diferencia o DOM XSS do XSS refletido, em que o servidor recebe a entrada e a devolve dentro da resposta.
