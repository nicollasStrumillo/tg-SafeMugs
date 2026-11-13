# Open Redirect

Ocorre quando a aplicação redireciona o usuário para uma URL controlada pelo cliente sem validar adequadamente o destino. Um atacante pode explorar essa falha para construir links aparentemente legítimos que levam vítimas a sites maliciosos, facilitando ataques de phishing, roubo de credenciais ou evasão de filtros baseados em allowlist.

## Exploração

Observe esta página de login, aberta a partir de um link que veio de outra tela do site: 
![[openredirect-imagem1.png]](/guias-categoria/imagens/openredirect-imagem1.png)

Considere que a aplicação use o parâmetro `retorno` para levar o usuário de volta à página em que ele estava depois de entrar, desta maneira:

```text
POST /login
  se as credenciais forem válidas:
      redirecionar para @retorno
```

Onde `@retorno` é o valor do parâmetro `retorno` presente na URL.

No uso normal, a resposta do servidor depois do login é esta:

```http
HTTP/1.1 302 Found
Location: /pedidos
```

Se o destino do redirecionamento não for validado, um usuário mal intencionado pode compor um valor que aponta para outro site. Ele monta um link com esse valor e o envia à vítima:

```text
https://loja.exemplo.com/login?retorno=https://site-malicioso.exemplo.net
```

Depois que a vítima faz login, a resposta do servidor passa a ser:

```http
HTTP/1.1 302 Found
Location: https://site-malicioso.exemplo.net
```

- `https://loja.exemplo.com/login` é o endereço legítimo do site. A vítima vê o domínio verdadeiro no link e confia nele.
- `retorno=https://site-malicioso.exemplo.net` é o parâmetro manipulado. O servidor não confere se o destino é uma página do próprio site e apenas repete o valor recebido.
- O navegador obedece ao `Location` e leva a vítima para o site do atacante. Esse site pode imitar a loja e dizer, por exemplo, que a sessão expirou, pedindo que a pessoa informe suas credenciais novamente.

### Onde o redirecionamento costuma aparecer

O parâmetro que carrega o destino recebe nomes variados, como `retorno`, `next`, `url`, `redirect` e `returnUrl`. Os pontos mais comuns são:

- **Páginas de login e logout:** que levam o usuário de volta para onde ele estava.
- **Links de saída:** que avisam que o usuário está deixando o site, mas usam um parâmetro para definir o endereço.
- **Fluxos de autenticação com terceiros:** em que o destino de retorno também é informado por parâmetro.

O ataque se destaca pela credibilidade: como o link começa com o domínio verdadeiro da aplicação, ele passa por filtros e desperta menos desconfiança do que um endereço desconhecido.