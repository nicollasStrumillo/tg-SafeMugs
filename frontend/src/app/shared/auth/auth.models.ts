export interface LoginRequest {
	email: string;
	senha: string;
}

export interface CadastroRequest {
	nomeCompleto: string;
	email: string;
	senha: string;
	confirmarSenha: string;
}

export interface LoginResponse {
	usuarioId: number;
	nomeCompleto: string;
	email: string;
	perfil: string;
}

export interface UsuarioLogado {
	usuarioId: number;
	nomeCompleto: string;
	email: string;
	perfil: string;
	autenticadoEm: string;
}