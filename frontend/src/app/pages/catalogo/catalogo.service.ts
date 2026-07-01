import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { map, Observable } from 'rxjs';

import {
	ProdutoCardViewModel,
	ProdutoCatalogoDto,
} from './catalogo.models';

@Injectable({
	providedIn: 'root',
})
export class CatalogoService {
	private readonly http = inject(HttpClient);
	private readonly apiBaseUrl = 'http://localhost:5242';

	public listarProdutos(): Observable<ProdutoCardViewModel[]> {
		return this.http
			.get<ProdutoCatalogoDto[]>(`${this.apiBaseUrl}/api/produtos/lista`)
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
		const comentarios = produto.comentariosProduto ?? [];

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
			ativo: produto.ativo,
			comentarios,
		};
	}

	private normalizarImagem(urlImagem?: string): string {
		if (!urlImagem) {
			return '/imagens/mug_behappy.jpg';
		}

		return urlImagem.startsWith('http') ? urlImagem : `http://localhost:5242${urlImagem}`;
	}
}