import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { map, Observable } from 'rxjs';

import {
	ProdutoCardViewModel,
	ProdutoCatalogoDto,
	ComentarioRequest,
	ComentarioProdutoDto
} from './catalogo.models';

@Injectable({
	providedIn: 'root',
})
export class CatalogoService {
	private readonly http = inject(HttpClient);

	public listarProdutos(): Observable<ProdutoCardViewModel[]> {
		return this.http
			.get<ProdutoCatalogoDto[]>(`/api/produtos/lista`)
			.pipe(map((produtos) => produtos.map((produto) => this.paraViewModel(produto))));
	}

	private paraViewModel(produto: ProdutoCatalogoDto): ProdutoCardViewModel {
		const imagens = produto.imagensProduto ?? [];
		const avaliacoes = produto.avaliacoes ?? [];
		const imagemPrincipal = imagens[0];
		const avaliacaoMedia =
			avaliacoes.length > 0
				? avaliacoes.reduce((soma, avaliacao) => soma + avaliacao.nota, 0) /
				  avaliacoes.length
				: null;

		return {
			id: produto.id,
			nome: produto.nome,
			descricao: produto.descricao,
			preco: produto.preco,
			estoque: produto.estoque,
			categoria: produto.categoriaProduto?.nome ?? 'Sem categoria',
			imagemUrl: this.normalizarImagem(imagemPrincipal?.urlImagem),
			imagemLegenda: imagemPrincipal?.legenda ?? produto.nome,
			avaliacaoMedia,
			quantidadeAvaliacoes: avaliacoes.length,
			ativo: produto.ativo
		};
	}

	private normalizarImagem(urlImagem?: string): string {
		if (!urlImagem) {
			return '/imagens/mug_behappy.jpg';
		}

		if (urlImagem.startsWith('http') || urlImagem.startsWith('/')) {
			return urlImagem;
		}

		return `/${urlImagem}`;
	}

	//comentarios
	public obterComentarios(produtoId: number): Observable<ComentarioProdutoDto[]> {
		return this.http.get<ComentarioProdutoDto[]>(`/api/produtos/comentarios/${produtoId}`);
	}

	public fazerComentario(produtoId: number, comentarioRequest: ComentarioRequest): Observable<void>{
		return this.http.post<void>(`/api/produtos/comentarios/${produtoId}`, comentarioRequest);
	}
}