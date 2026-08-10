import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { map, Observable } from 'rxjs';

import {
	ProdutoCardViewModel,
	ProdutoCompletoDto,
	ComentarioRequest,
	ComentarioProdutoDto
} from './produto.models';

@Injectable({
	providedIn: 'root',
})
export class ProdutoService {
	private readonly http = inject(HttpClient);

	public listarProdutos(): Observable<ProdutoCardViewModel[]> {
		return this.http
			.get<ProdutoCompletoDto[]>(`/api/produtos/lista`)
			.pipe(map((produtos) => produtos.map((produto) => this.paraViewModel(produto))));
	}

	private paraViewModel(produto: ProdutoCompletoDto): ProdutoCardViewModel {
		const avaliacoes = produto.avaliacoes ?? [];
		const comentarios = produto.comentariosProduto ?? [];
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
			descricaoCategoria: produto.categoriaProduto?.descricao ?? 'Sem descrição',
			imagemUrl: this.normalizarImagem(produto.urlImagemProduto),
			avaliacaoMedia,
			quantidadeAvaliacoes: avaliacoes.length,
			quantidadeComentarios: comentarios.length,
			
		};
	}

	private normalizarImagem(urlImagem?: string): string {
		if (!urlImagem) {
			return '/imagens/produto/mug_behappy.jpg';
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

	public atualizarComentario(comentarioId: number, comentario: string): Observable<void> {
		const requestBody = { comentarioId, comentario };
		return this.http.patch<void>(`/api/produtos/comentarios`, requestBody);
	}
}