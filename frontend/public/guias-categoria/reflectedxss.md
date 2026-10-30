# Reflected XSS
Variação do Cross-Site Scripting (XSS) em que a entrada maliciosa não é armazenada pela aplicação: ela viaja na própria requisição (geralmente na URL) e é devolvida imediatamente na resposta, sem o tratamento adequado. O navegador interpreta esse trecho como código e o executa no contexto do site vulnerável. Como o código só roda para quem fizer aquela requisição, o atacante precisa levar a vítima a abrir um link especialmente montado. Uma vez executado, o script pode roubar a sessão, ler informações exibidas na página ou realizar ações em nome da vítima.
 
## Exploração
Observe esta página de busca, em que o termo pesquisado é enviado pela URL e exibido na tela:
![ReflectedXss-imagem1](/guias-categoria/imagens/reflectedxss-imagem1.png)
 
Considere que a aplicação, ao receber o parâmetro `termo`, o insira diretamente no HTML da resposta, desta maneira:
```html
<h2>Resultados para: @termo</h2>
```
Onde `@termo` é o valor do parâmetro `termo` presente na URL.
 
Se a inserção deste parâmetro na página for insegura, um usuário mal intencionado pode compor um valor que utiliza da sintaxe do HTML para introduzir código que será executado pelo navegador.
 
Neste caso, um exemplo de entrada maliciosa seria `<script>alert('XSS')</script>`. Como o valor vai na URL, o atacante monta um link com ele embutido:
```text
https://loja.exemplo.com/busca?termo=<script>alert('XSS')</script>
```
Na prática, o navegador codifica os caracteres especiais antes de enviar (o link ficaria algo como `...?termo=%3Cscript%3Ealert(%27XSS%27)%3C/script%3E`), e o servidor os decodifica ao ler o parâmetro. Quando `@termo` for substituído, a página devolvida pelo servidor ficará assim:
```html
<h2>Resultados para: <script>alert('XSS')</script></h2>
```
- `https://loja.exemplo.com/busca` é o endereço legítimo do site. A vítima vê o domínio verdadeiro, o que aumenta a confiança no link.
- `?termo=` é o parâmetro vulnerável, cujo valor será "refletido" de volta na página.
- `<script>alert('XSS')</script>` é o código injetado. O navegador o executa assim que carrega a página, com os mesmos privilégios do site. O `alert` é apenas uma prova de conceito; no lugar dele poderia haver qualquer JavaScript.
### O caminho completo do ataque
1. O atacante monta o link com o código embutido no parâmetro e o envia à vítima (por e-mail, mensagem, rede social ou um link encurtado).
2. A vítima abre o link. O navegador faz a requisição ao site legítimo, anexando automaticamente o cookie de sessão dela.
3. O servidor monta a resposta incluindo o valor do parâmetro e a devolve ao navegador.
4. O navegador entende o trecho injetado como parte da página e o executa como se fosse código do próprio site.
Perceba que o código malicioso nunca fica salvo no servidor: ele existe apenas dentro do link e na resposta daquela requisição. É isso que diferencia o XSS refletido do XSS armazenado, em que a entrada fica guardada e atinge qualquer pessoa que visitar a página.
