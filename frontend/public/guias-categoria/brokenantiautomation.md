# Broken Anti-Automation

Ocorre quando a aplicação não impõe limites ou controles suficientes sobre a frequência e o volume com que uma ação pode ser repetida. Funcionalidades pensadas para o uso humano ocasional (entrar na conta, pedir um código, enviar um formulário) passam a poder ser executadas em massa por um programa. Aqui nenhuma requisição é "malformada": cada uma, isoladamente, é legítima. O problema está na ausência de qualquer mecanismo que perceba e freie o excesso. Isso pode permitir a adivinhação de senhas e códigos, a criação em massa de contas, o envio de spam por formulários, a coleta em larga escala de dados e o esgotamento de recursos limitados.

## Exploração

Observe esta tela de login de uma loja: 
![[brokenAntiAutomation-imagem1.png]](/guias-categoria/imagens/brokenAntiAutomation-imagem1.png)

Considere que a aplicação trate o login com a seguinte lógica no servidor:

```text
POST /login
  usuario = buscar_usuario(email)
  se usuario existe e a senha confere:
      responder 200 "Bem-vindo"
  senão:
      responder 401 "Credenciais inválidas"
```

Nada nessa lógica está errado quando olhada isoladamente: ela valida as credenciais e responde de acordo. O problema é o que **não** existe nela: não há contagem de tentativas, não há bloqueio temporário, não há atraso entre as respostas e não há um sistema que diferencie uma pessoa de um programa.

Para uma pessoa, esse comportamento é normal: ela erra a senha uma ou duas vezes e acerta na seguinte. Mas, como o servidor trata cada requisição como se fosse a primeira, um programa pode enviar a mesma requisição de forma repetida, alterando apenas o valor de um campo:

```text
POST /login   email=maria@exemplo.com&senha=tentativa1   ->  401
POST /login   email=maria@exemplo.com&senha=tentativa2   ->  401
POST /login   email=maria@exemplo.com&senha=tentativa3   ->  401
...
POST /login   email=maria@exemplo.com&senha=tentativaN   ->  200
```

- Todas as respostas de falha são idênticas e rápidas. A aplicação nunca "reage" ao fato de que a mesma conta recebeu centenas de tentativas em poucos segundos.
- A velocidade passa a ser limitada só pela capacidade do servidor e da rede, não por nenhuma regra da aplicação. 
- O servidor só deixa de responder `401` quando uma tentativa acerta. 