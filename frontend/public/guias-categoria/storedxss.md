# Stored XSS
Variação do Cross-Site Scripting (XSS) em que a entrada maliciosa é armazenada pela aplicação (em um banco de dados, por exemplo) e depois exibida, sem o tratamento adequado, para quem acessar a página que a mostra. O navegador interpreta esse conteúdo como código e o executa no contexto do site vulnerável. Como o código fica salvo, o ataque atinge qualquer visitante sem que o atacante precise convencer ninguém a abrir um link, o que torna essa variação a mais perigosa, principalmente quando a página é vista por usuários com mais privilégios, como administradores.
 
## Exploração
Observe esta página de produto, que permite aos clientes deixarem avaliações:
![StoredXss-imagem1](/guias-categoria/imagens/storedxss-imagem1.png)
Considere que, ao enviar uma avaliação, a aplicação grave o texto no banco de dados e, sempre que a página do produto for aberta, monte a lista de avaliações inserindo cada texto gravado diretamente no HTML, desta maneira:
```html
<div class="avaliacao">
  <strong>@autor</strong>
  <p>@texto</p>
</div>
```
Onde `@autor` e `@texto` são valores gravados a partir da entrada do usuário.
 
Se a inserção destes valores na página for insegura, um usuário mal intencionado pode compor uma avaliação que utiliza da sintaxe do HTML para introduzir código que será executado pelo navegador.
 
Neste caso, um exemplo de entrada maliciosa seria `Ótima caneca! <script>alert('XSS')</script>`. O atacante apenas envia a avaliação pelo formulário e a aplicação a guarda como qualquer outra. A partir daí, sempre que a página for carregada, o HTML gerado ficará assim:
```html
<div class="avaliacao">
  <strong>Visitante</strong>
  <p>Ótima caneca! <script>alert('XSS')</script></p>
</div>
```
- `Ótima caneca!` é um texto comum, que faz a avaliação parecer legítima e passar despercebida.
- `<script>alert('XSS')</script>` deixa de ser texto e passa a ser uma tag de script. O navegador a executa a cada vez que a página é carregada, para todo visitante. O `alert` é apenas uma prova de conceito; no lugar dele poderia haver qualquer JavaScript, com os mesmos privilégios do site.
- A avaliação permanece no banco de dados até que alguém a remova, então o código continua sendo entregue a cada novo acesso.

Perceba que a entrada e a execução acontecem em momentos, e muitas vezes em páginas, diferentes: o formulário é o ponto de entrada, mas o ataque só se concretiza onde o conteúdo gravado é exibido. É isso que diferencia o XSS armazenado do refletido: não há link a ser enviado, e a vítima não precisa fazer nada além de visitar a página normalmente.