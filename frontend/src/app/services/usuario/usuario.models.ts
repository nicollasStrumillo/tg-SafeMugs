export interface LoginRequest {
	email: string;
	senha: string;
	resolverDesafioSqlInjection?: boolean;
}

export interface CadastroRequest {
	nomeCompleto: string;
	email: string;
	senha: string;
	confirmarSenha: string;
}

export interface LoginResponse {
	token: string;
	expiresAt: string;
	usuarioId: number;
	nomeCompleto: string;
	email: string;
	urlImagemPerfil: string;
	perfil: string;
}

export interface UsuarioLogado {
	usuarioId: number;
	nomeCompleto: string;
	email: string;
	urlImagemPerfil: string;
	perfil: string;
}

