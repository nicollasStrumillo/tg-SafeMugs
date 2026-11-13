# Security Misconfiguration

Ocorre quando a aplicação, o servidor ou seus componentes são configurados de forma insegura. Configurações incorretas, recursos desnecessários habilitados, mensagens de erro detalhadas ou permissões excessivas podem expor informações sensíveis e facilitar outros ataques.

Sua exploração nem sempre depende de uma entrada maliciosa específica: o atacante aproveita o que já está exposto, bastando encontrar. Alguns exemplos comuns:

- **Credenciais padrão:** painéis administrativos, bancos de dados ou equipamentos que continuam com o usuário e a senha de padrão (como `admin` / `admin`).
- **Recursos desnecessários habilitados:** páginas de administração acessíveis pela internet, listagem de diretórios, portas e serviços abertos sem necessidade ou modo de depuração ativo em produção.
- **Mensagens de erro detalhadas:** em vez de uma resposta genérica, a aplicação devolve ao usuário informações internas.
- **Permissões excessivas:** arquivos de configuração, backups e áreas de armazenamento acessíveis publicamente, ou contas de serviço com mais privilégios do que precisam.