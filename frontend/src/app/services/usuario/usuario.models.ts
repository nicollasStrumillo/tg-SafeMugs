export interface UsuarioResumoDto {
	id: number;
	nomeCompleto: string;
	email: string;
	ativo: boolean;
	perfil: string;
	urlImagemPerfil: string;
}

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

// Resposta dos endpoints que esperam que o frontend altere imediatamente o JWT do usuário, como login, cadastro, edição de usuário...
export interface AuthTokenResponse {
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


// Detalhes Usuario
export interface EnderecoDto{
	id: number;
	logradouro: string;
	numero: number;
	complemento: string | null;
	bairro: string;
	cidade: string;
	estado: string;
	cep: string;
}

export interface DetalhesUsuarioResponse {
	id: number;
	nomeCompleto: string;
	email: string;
	telefone: string | null;
	ativo: boolean;
	dtCadastro: string;
	dtAtualizacao: string;
	urlImagemPerfil: string;

	perfil: string;

	endereco: EnderecoDto | null;
}

export interface EditarUsuarioRequest {
	nomeCompleto: string;
	telefone: string | null;
	
	logradouro: string | null;
	numero: number | null;
	complemento: string | null;
	bairro: string | null;
	cidade: string | null;
	estado: string | null;
	cep: string | null;
}

export interface UploadFotoPerfilUrlRequest {	
	url: string;
}

export interface MudarSenhaRequest {
	novaSenha: string;
}
