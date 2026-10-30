# IDOR (Insecure Direct Object Reference)
Ocorre quando a aplicação expõe um identificador direto de um objeto (como o id de um pedido, de um usuário ou de um arquivo) e o utiliza para buscar o recurso sem verificar se o usuário autenticado tem permissão para acessá-lo. Com isso, um atacante pode simplesmente trocar o identificador por outro e visualizar, alterar ou excluir dados de outras pessoas. É uma das formas mais comuns de falha de controle de acesso.

## Exploração
Observe esta tela de detalhes de um pedido, exibida para um usuário logado:
![Idor-imagem1](/guias-categoria/imagens/idor-imagem1.png)

Considere que, ao abrir um pedido, o navegador envie a seguinte requisição:
```http
GET /pedidos/1001 HTTP/1.1
Host: loja.exemplo.com
Cookie: sessao=abc123
```
E que a aplicação busque o pedido no banco usando apenas o número recebido na URL:
```sql
Select * from pedidos p where p.Id = @id;
```
Onde `@id` é o valor `1001` vindo da requisição.

O problema é que a consulta confirma que o pedido existe, mas não confirma que ele pertence a quem está logado. Um usuário mal intencionado pode simplesmente alterar o identificador na requisição:
```http
GET /pedidos/1002 HTTP/1.1
Host: loja.exemplo.com
Cookie: sessao=abc123
```
- O cookie continua sendo o do próprio atacante, então a aplicação o reconhece como um usuário legítimo e autenticado.
- `1002` é o id de um pedido de outra pessoa. Como nada verifica o dono do pedido, a aplicação responde com nome, endereço e valores do outro cliente.
- Quando os ids são sequenciais, o atacante pode repetir o processo com `1003`, `1004`, `1005`... e percorrer todos os registros do sistema.

O identificador não precisa estar na URL: o mesmo problema ocorre quando ele vem na query string (`?pedido=1002`), no corpo da requisição ou em um cabeçalho. Também não se limita à leitura, pois endpoints de edição e exclusão (`PUT`, `DELETE`) que seguem o mesmo padrão permitem alterar ou apagar dados alheios.