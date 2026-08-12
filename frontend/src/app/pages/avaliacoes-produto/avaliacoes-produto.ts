import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ProdutoCompletoDto } from '../../services/produto/produto.models';
import { Router } from '@angular/router';

import { NotificationService } from '../../shared/notification/notification.service';
import { ProdutoService } from '../../services/produto/produto.service';
import { HttpErrorResponse } from '@angular/common/http';

interface AvaliacaoCardViewModel {
	id: number;

	nomeUsuario: string;
	emailUsuario: string;
	urlImagemPerfilUsuario: string | null;

	nota: number;
	comentario: string;
}

interface AvaliacoesProdutoViewModel {
	idProduto: number;
	nomeProduto: string;
	descricaoProduto: string;
	categoriaProduto: string;
	urlImagemProduto: string;
	avaliacaoMedia: number;

	avaliacoes: AvaliacaoCardViewModel[];
}

function paraViewModel(produto: ProdutoCompletoDto): AvaliacoesProdutoViewModel {
	const avaliacoes = produto.avaliacoes ?? [];
	const avaliacaoMedia =
		avaliacoes.length > 0
			? avaliacoes.reduce((soma, avaliacao) => soma + avaliacao.nota, 0) /
			  avaliacoes.length
			: 0;

	const avaliacoesViewModel: AvaliacaoCardViewModel[] = avaliacoes.map((avaliacao) => ({
		id: avaliacao.id,
		nomeUsuario: avaliacao.usuario?.nomeCompleto ?? 'Usuário desconhecido',
		emailUsuario: avaliacao.usuario?.email ?? 'Email desconhecido',
		urlImagemPerfilUsuario: avaliacao.usuario?.urlImagemPerfil ?? null,
		nota: avaliacao.nota,
		comentario: avaliacao.comentario,
	}));

	return {
		idProduto: produto.id,
		nomeProduto: produto.nome,
		descricaoProduto: produto.descricao,
		categoriaProduto: produto.categoriaProduto?.nome ?? 'Categoria desconhecida',
		urlImagemProduto: normalizarImagem(produto.urlImagemProduto),
		avaliacaoMedia: avaliacaoMedia,
		avaliacoes: avaliacoesViewModel,
	};
}

function normalizarImagem(urlImagem: string): string {
	if (!urlImagem) return '';
	if (urlImagem.startsWith('http') || urlImagem.startsWith('/')) return urlImagem;
	return `/${urlImagem}`;
}

@Component({
	selector: 'sm-avaliacoes-produto',
	imports: [
		CommonModule,
		MatButtonModule,
		MatCardModule,
		MatFormFieldModule,
		MatIconModule,
		MatInputModule,
		MatTooltipModule,
	],
	templateUrl: './avaliacoes-produto.html',
	styleUrl: './avaliacoes-produto.scss',
	standalone: true,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AvaliacoesProduto implements OnInit {
	private readonly notificationService = inject(NotificationService);
	private readonly route = inject(ActivatedRoute);
	private readonly produtoService = inject(ProdutoService);

	protected readonly viewModel = signal<AvaliacoesProdutoViewModel | null>(null);
	protected readonly nome = signal<string | null>(null);
	protected readonly carregando = signal(true);
	protected readonly termoBusca = signal('');
	protected readonly notaSelecionada = signal<number>(5);

	protected readonly avaliacoesFiltradas = computed(() => {
		const modelo = this.viewModel();
		const termo = this.normalizarTexto(this.termoBusca());
		const nota = this.notaSelecionada();

		return (modelo?.avaliacoes ?? []).filter((avaliacao) => {
			const correspondeAoTexto = !termo || this.normalizarTexto(avaliacao.comentario).includes(termo);
			const correspondeANota = nota === null || avaliacao.nota <= nota;
			return correspondeAoTexto && correspondeANota;
		});
	});

	constructor(private router: Router) {}

	ngOnInit(): void {
		this.route.queryParamMap.subscribe((params) => {
			const nome = params.get('nome') ?? '';

			this.nome.set(nome);
			this.viewModel.set(null);
			this.carregando.set(true);

			this.produtoService.obterProdutoCompletoPorNome(nome)
				.subscribe({
					next: (produto) => {
						this.viewModel.set(paraViewModel(produto));
						this.carregando.set(false);
					},
					error: (error: HttpErrorResponse) => {
						this.notificationService.notificarErroApi(error);
						this.viewModel.set(null);
						this.carregando.set(false);
					}
				});
		});
	}

	protected contarEstrelas(nota: number): {cheias: number, metade: number, vazias: number} {
		let metade = 0;
	
		let cheias = Math.floor(nota);
		
		const decimal = nota - cheias;
		if (decimal >= 0.85) cheias += 1;
		else if(decimal >= 0.4) metade += 1;

		const vazias = 5 - cheias - metade;

		return { cheias, metade, vazias };
	}

	protected selecionarProduto(id: number): void {
		this.router.navigate(['/catalogo'], { queryParams: { idProduto: id } });
	}

	protected atualizarBusca(event: Event): void {
		this.termoBusca.set((event.target as HTMLInputElement).value);
	}

	protected limparFiltros(): void {
		this.termoBusca.set('');
		this.notaSelecionada.set(5);
	}

	protected selecionarNota(nota: number): void {
		this.notaSelecionada.set(this.notaSelecionada() === nota ? 5 : nota);
	}

	protected estrelaIcone(indice: number, nota: number): string {
		const estrelas = this.contarEstrelas(nota);
		if (indice <= estrelas.cheias) return 'star';
		if (indice === estrelas.cheias + 1 && estrelas.metade > 0) return 'star_half';
		return 'star_border';
	}

	protected normalizarTexto(valor: string): string {
		return valor.normalize('NFD').replace(/[\u0300-\u036f]/g, '').toLowerCase();
	}
}
