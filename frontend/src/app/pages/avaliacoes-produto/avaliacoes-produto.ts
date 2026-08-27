import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { ProdutoCompletoDto } from '../../services/produto/produto.models';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { HttpErrorResponse } from '@angular/common/http';
import { MatTooltipModule, MatTooltip } from '@angular/material/tooltip';

import { ProdutoService } from '../../services/produto/produto.service';
import { AuthSessionService } from '../../services/usuario/auth/auth-session.service';
import { ApiError } from '../../shared/notification/erro-api.model';
import { FazerAvaliacao } from '../fazer-avaliacao/fazer-avaliacao';


// Parte da vulnerabilidade de Reflected XSS e Stored XSS
import {SignalRService} from '../../services/signalR/signalr.service';
import { DomSanitizer, type SafeHtml } from '@angular/platform-browser'

const REFLECTEDXSS_PAYLOAD = '<iframe src="javascript:alert(`XSS`)">';
const STOREDXSS_PAYLOAD = '<iframe src="javascript:alert(`XSS`)">';

export interface AvaliacaoCardViewModel {
	id: number;

	nomeUsuario: string;
	emailUsuario: SafeHtml;
	urlImagemPerfilUsuario: string | null;

	nota: number;
	comentario: string;
}

export interface AvaliacoesProdutoViewModel {
	idProduto: number;
	nomeProduto: string;
	descricaoProduto: string;
	categoriaProduto: string;
	urlImagemProduto: string;

	avaliacoes: AvaliacaoCardViewModel[];
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
    MatCardModule,
    MatIconModule,
    MatInputModule,
    MatTooltip,
	MatTooltipModule
],
	templateUrl: './avaliacoes-produto.html',
	styleUrl: './avaliacoes-produto.scss',
	standalone: true,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AvaliacoesProduto implements OnInit {
	private readonly authSessionService = inject(AuthSessionService);
	private readonly route = inject(ActivatedRoute);
	private readonly produtoService = inject(ProdutoService);

	// Parte da vulnerabilidade de Reflected XSS e Stored XSS
	private readonly signalRService = inject(SignalRService);
	private readonly sanitizer = inject(DomSanitizer);
	protected readonly mensagemErroPesquisa = signal<SafeHtml | null>(null);

	protected readonly usuarioAutenticado = computed(() => this.authSessionService.autenticado());
	protected readonly viewModel = signal<AvaliacoesProdutoViewModel | null>(null);
	protected readonly carregando = signal(true);
	protected readonly termoBusca = signal('');
	protected readonly nome = signal<string | null>(null);
	protected readonly notaSelecionada = signal<number>(5);

	protected readonly mediaAvaliacoes = computed(() => {
		const modelo = this.viewModel();
		const avaliacoes = modelo?.avaliacoes ?? [];

		const media = avaliacoes.length > 0
			? avaliacoes.reduce((soma, avaliacao) => soma + avaliacao.nota, 0) /
			  avaliacoes.length
			: 0;
		return media;
	});

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
	
	constructor(private router: Router, private readonly dialog: MatDialog) {}

	ngOnInit(): void {
		this.route.queryParamMap.subscribe((params) => {
			const nome = params.get('nome') ?? '';
			this.nome.set(nome);
			this.recarregarProduto(nome);
		});
	}

	private recarregarProduto(nome: string): void {
		this.viewModel.set(null);
		this.carregando.set(true);
		this.mensagemErroPesquisa.set(null);

		this.produtoService.obterProdutoCompletoPorNome(nome)
			.subscribe({
				next: async (produto) => {
					this.viewModel.set(this.paraViewModel(produto));
					this.carregando.set(false);

					// Parte da vulnerabilidade de Stored XSS
					if (produto.avaliacoes?.some(a => (a.usuario?.email?.trim()?.includes(STOREDXSS_PAYLOAD) ?? false))){
						await this.signalRService.SolveDesafioXss("Stored XSS em Avaliações", STOREDXSS_PAYLOAD);
					}
					
				},
				error: async (error: HttpErrorResponse) => {
					// Parte da vulnerabilidade de Reflected XSS
					const mensagemErro = (error.error as ApiError | null)?.detail ?? '';
					this.mensagemErroPesquisa.set(this.sanitizer.bypassSecurityTrustHtml(mensagemErro));
					
					if (mensagemErro.includes(REFLECTEDXSS_PAYLOAD)){
						await this.signalRService.SolveDesafioXss("Reflected XSS", REFLECTEDXSS_PAYLOAD);
					}

					this.viewModel.set(null);
					this.carregando.set(false);
				}
			});
	}

	private paraViewModel(produto: ProdutoCompletoDto): AvaliacoesProdutoViewModel {
		const avaliacoes = produto.avaliacoes ?? [];

		const avaliacoesViewModel: AvaliacaoCardViewModel[] = avaliacoes.map((avaliacao) => ({
			id: avaliacao.id,
			nomeUsuario: avaliacao.usuario?.nomeCompleto ?? 'Usuário desconhecido',
			emailUsuario: this.sanitizer.bypassSecurityTrustHtml(avaliacao.usuario?.email.trim() ?? 'Email desconhecido'),
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
			avaliacoes: avaliacoesViewModel,
		};
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

	protected fazerAvaliacao(id: number): void {
		const ref = this.dialog.open(FazerAvaliacao, {
			data: {
				produto: {
					idProduto: id,
					nomeProduto: this.viewModel()?.nomeProduto ?? '',
					descricaoProduto: this.viewModel()?.descricaoProduto ?? '',
					categoriaProduto: this.viewModel()?.categoriaProduto ?? '',
					urlImagemProduto: this.viewModel()?.urlImagemProduto ?? ''
				}
			},
			width: '1100px',
			maxWidth: '95vw',
			maxHeight: '90vh',
			autoFocus: false
		});

		ref.afterClosed().subscribe((resultado) => {
			if (resultado === true) {
				this.recarregarProduto(this.nome() ?? '');
			}
		});
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
