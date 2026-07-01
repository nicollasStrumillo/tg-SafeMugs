export interface UsuarioResumoDto {
	id: number;
	nomeCompleto: string;
}

export interface PerfilResumoDto {
	id: number;
	nome: string;
}

export interface CategoriaProdutoDto {
	id: number;
	nome: string;
}

export interface ImagemProdutoDto {
	id: number;
	urlImagem: string;
	legenda?: string | null;
}

export interface AvaliacaoDto {
	id: number;
	nota: number;
	comentario?: string | null;
	usuario?: UsuarioResumoDto | null;
}

export interface ComentarioProdutoDto {
	id: number;
	comentario: string;
	usuario?: UsuarioResumoDto | null;
}

export interface ProdutoCatalogoDto {
	id: number;
	nome: string;
	descricao: string;
	preco: number;
	estoque: number;
	ativo: boolean;
	categoriaProduto?: CategoriaProdutoDto | null;
	imagensProduto?: ImagemProdutoDto[];
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
	imagemUrl: string;
	imagemLegenda: string;
	avaliacaoMedia: number | null;
	quantidadeAvaliacoes: number;
	ativo: boolean;
	comentarios: ComentarioProdutoDto[];
}