# SQL Injection
Ocorre quando dados fornecidos pelo usuário são inseridos diretamente em uma consulta SQL sem o tratamento adequado. Isso pode permitir que um atacante altere a consulta original, acesse informações indevidas, modifique dados ou execute comandos não previstos pela aplicação.

## Exploração 
Observe este formulário que espera a entrada de um usuário para executar uma pesquisa:
![SqlInjection-imagem1](/guias-categoria/imagens/sqlinjection-imagem1.png)

Considere que, para executar a pesquisa, a aplicação injete a entrada do usuário no meio da query, desta maneira:
```sql
Select * from produtos p where p.Nome = "@nome"; 
```
Onde `@nome` é a entrada do usuário. 

Se a injeção deste parâmetro para dentro da consulta for insegura, um usuário mal intencionado pode compor 
uma entrada que utiliza da sintaxe do SQL para modificar a query e fazer com que ela produza um resultado arbitrário. 

Neste caso, um exemplo de entrada maliciosa seria `A" or p.Ativo = 0; --`. Veja como a consulta ficará quando `@nome` for substituído:
```sql
Select * from produtos p where p.Nome = "A" or p.Ativo = 0; --"; 
```
- `A"` imediatamente fecha as aspas que cercava o parâmetro `@nome`
- `or p.Ativo = 0` adiciona um filtro arbitrário à consulta. 
- `; --` termina a consulta e comenta todo resto da linha. Desta maneira, a execução não da erro por causa da aspas que ficou avulsa. 

