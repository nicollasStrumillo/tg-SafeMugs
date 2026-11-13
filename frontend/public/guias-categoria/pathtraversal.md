# Path Traversal

Ocorre quando a aplicação utiliza uma entrada do usuário para montar um caminho de arquivo sem sanitizar separadores de diretório e segmentos como `..`, permitindo que o atacante acesse ou sobrescreva arquivos fora do diretório pretendido.

## Exploração

Observe esta URL de uma funcionalidade de download de arquivos: 
![pathtraversal-imagem1.png](/guias-categoria/imagens/pathtraversal-imagem1.png)

Considere que, para atender o download, a aplicação monte o caminho do arquivo juntando a pasta de arquivos com o nome informado, desta maneira:

```text
/var/www/app/arquivos/@arquivo
```

Onde `@arquivo` é a entrada do usuário. No uso normal, `manual.pdf` resulta em `/var/www/app/arquivos/manual.pdf`.

Se a montagem deste caminho for insegura, um usuário mal intencionado pode compor uma entrada que utiliza da sintaxe de caminhos para escapar da pasta original.

Neste caso, um exemplo de entrada maliciosa seria `../../../../etc/passwd`. Veja como o caminho ficará quando `@arquivo` for substituído:

```text
/var/www/app/arquivos/../../../../etc/passwd
```

- `..` representa o diretório pai. Cada ocorrência faz o caminho "subir" um nível: `arquivos` → `app` → `www` → `var` → `/` (raiz).
- Depois de subir até a raiz, `etc/passwd` aponta para um arquivo do sistema. O caminho final, já resolvido, é `/etc/passwd`, que está fora da pasta de downloads.
- A aplicação então lê o arquivo e o devolve na resposta, como se fosse um download comum.
