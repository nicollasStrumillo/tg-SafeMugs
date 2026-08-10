export interface UsuarioResumoDto {
	id: number;
	nomeCompleto: string;
	email: string;
	ativo: boolean;
	perfil: string;
	urlImagemPerfil: string;
}

export interface CategoriaProdutoDto {
	id: number;
	nome: string;
	descricao: string;
}

export interface AvaliacaoDto {
	id: number;
	nota: number;
	comentario: string;
	usuario?: UsuarioResumoDto | null;
}

export interface ProdutoCompletoDto {
	id: number;
	nome: string;
	descricao: string;
	preco: number;
	estoque: number;
	categoriaProduto?: CategoriaProdutoDto | null;
	urlImagemProduto: string;
	avaliacoes?: AvaliacaoDto[];
	comentariosProduto?: ComentarioProdutoDto[];
}

export interface ProdutoCardViewModel {
	id: number;
	nome: string;
	descricao: string;
	preco: number;
	estoque: number;
	categoria: string;
	descricaoCategoria: string;
	imagemUrl: string;
	avaliacaoMedia: number | null;
	quantidadeAvaliacoes: number;
	quantidadeComentarios: number;
}

// comentarios
export interface ComentarioProdutoDto {
	id: number;
	comentario: string;
	usuario?: UsuarioResumoDto | null;
}

export interface ComentarioRequest {
	nomeCompleto: string | null;
	comentario: string;
}