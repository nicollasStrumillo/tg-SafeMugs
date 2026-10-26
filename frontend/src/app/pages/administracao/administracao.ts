import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal,} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { DomSanitizer, type SafeHtml } from '@angular/platform-browser';
import { finalize } from 'rxjs';

import { AdministracaoService } from '../../services/administracao/administracao.service';
import { AvaliacaoAdminDto } from '../../services/administracao/administracao.models';
import { ProdutoCardViewModel } from '../../services/produto/produto.models';
import { ProdutoService } from '../../services/produto/produto.service';
import { NotificationService } from '../../shared/notification/notification.service';
import { ReposicaoAjudaDialog } from './reposicao-ajuda-dialog/reposicao-ajuda-dialog';
import { ReposicaoResultadoDialog } from './reposicao-resultado-dialog/reposicao-resultado-dialog';
import {SignalRService} from '../../services/signalR/signalr.service';

const STOREDXSS_PAYLOAD = '<iframe src="javascript:alert(`XSS`)">';

// Stored XSS.
interface AvaliacaoAdminCardViewModel {
	id: number;
	produtoNome: string;
	emailUsuario: SafeHtml;
	nota: number;
	comentario: string;
}

@Component({
	selector: 'sm-administracao',
	imports: [
		CommonModule,
		MatButtonModule,
		MatCardModule,
		MatFormFieldModule,
		MatIconModule,
		MatInputModule,
		MatProgressSpinnerModule,
		MatTooltipModule,
	],
	templateUrl: './administracao.html',
	styleUrl: './administracao.scss',
	standalone: true,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Administracao implements OnInit {
	private readonly administracaoService = inject(AdministracaoService);
	private readonly produtoService = inject(ProdutoService);
	private readonly notificationService = inject(NotificationService);
	private readonly dialog = inject(MatDialog);

	// Stored XSS.
	private readonly signalRService = inject(SignalRService);
	private readonly sanitizer = inject(DomSanitizer);

	protected readonly avaliacoes = signal<AvaliacaoAdminCardViewModel[]>([]);
	protected readonly carregandoAvaliacoes = signal(true);
	protected readonly erroAvaliacoes = signal<string | null>(null);
	protected readonly avaliacaoExcluindo = signal<number | null>(null);
	protected readonly produtos = signal<ProdutoCardViewModel[]>([]);
	protected readonly carregandoProdutos = signal(true);
	protected readonly erroProdutos = signal<string | null>(null);
	protected readonly arquivoSelecionado = signal<File | null>(null);
	protected readonly nomeArquivoSelecionado = computed(() => this.arquivoSelecionado()?.name ?? null);
	protected readonly enviandoReposicao = signal(false);

	protected readonly podeImportar = computed(
		() => this.arquivoSelecionado() !== null && !this.enviandoReposicao(),
	);

	ngOnInit(): void {
		this.carregarAvaliacoes();
		this.carregarProdutos();
		this.resolverDesafioAcessoAdministracao();
	}

	private resolverDesafioAcessoAdministracao(): void {
		this.administracaoService.resolverDesafioAcessoAdministracao().subscribe({
			error: (erro: HttpErrorResponse) => this.notificationService.notificarErroApi(erro),
		});
	}

	protected abrirAjudaEstruturaXml(): void {
		this.dialog.open(ReposicaoAjudaDialog, {
			width: '600px',
			maxWidth: '90vw',
			autoFocus: false,
			disableClose: false,
		});
	}

	protected carregarAvaliacoes(): void {
		this.carregandoAvaliacoes.set(true);
		this.erroAvaliacoes.set(null);

		this.administracaoService.obterAvaliacoes().subscribe({
			next: async (avaliacoes) => {
				this.avaliacoes.set(avaliacoes.map((avaliacao) => this.paraCardViewModel(avaliacao)));
				// Parte da vulnerabilidade de Stored XSS
				if (avaliacoes?.some(a => (a.usuarioEmail?.trim()?.includes(STOREDXSS_PAYLOAD) ?? false))){
					await this.signalRService.SolveDesafioXss("Stored XSS em Administração", STOREDXSS_PAYLOAD);
				}
				this.carregandoAvaliacoes.set(false);
			},
			error: (erro: HttpErrorResponse) => {
				this.erroAvaliacoes.set('Não foi possível carregar as avaliações.');
				this.carregandoAvaliacoes.set(false);
				this.notificationService.notificarErroApi(erro);
			},
		});
	}

	protected carregarProdutos(): void {
		this.carregandoProdutos.set(true);
		this.erroProdutos.set(null);

		this.produtoService.listarProdutos().subscribe({
			next: (produtos) => {
				this.produtos.set(produtos);
				this.carregandoProdutos.set(false);
			},
			error: (erro: HttpErrorResponse) => {
				this.erroProdutos.set('Não foi possível carregar os produtos.');
				this.carregandoProdutos.set(false);
				this.notificationService.notificarErroApi(erro);
			},
		});
	}

	protected excluirAvaliacao(avaliacao: AvaliacaoAdminCardViewModel): void {
		if (this.avaliacaoExcluindo() !== null) {
			return;
		}

		this.avaliacaoExcluindo.set(avaliacao.id);

		this.administracaoService
			.excluirAvaliacao(avaliacao.id)
			.pipe(finalize(() => this.avaliacaoExcluindo.set(null)))
			.subscribe({
				next: () => {
					this.avaliacoes.update((lista) => lista.filter((a) => a.id !== avaliacao.id));
					this.notificationService.sucesso('Avaliação excluída.');
					// Médias e contagens de avaliações mudam: recarrega a lista de produtos.
					this.carregarProdutos();
				},
				error: (erro: HttpErrorResponse) => this.notificationService.notificarErroApi(erro),
			});
	}

	protected estaExcluindo(avaliacaoId: number): boolean {
		return this.avaliacaoExcluindo() === avaliacaoId;
	}

	protected aoSelecionarArquivo(event: Event): void {
		const input = event.target as HTMLInputElement;
		this.arquivoSelecionado.set(input.files?.[0] ?? null);
	}

	protected limparArquivo(): void {
		this.arquivoSelecionado.set(null);
	}

	protected importarReposicao(): void {
		const arquivo = this.arquivoSelecionado();
		if (arquivo === null || this.enviandoReposicao()) {
			return;
		}

		this.enviandoReposicao.set(true);

		this.administracaoService
			.importarReposicao(arquivo)
			.pipe(finalize(() => this.enviandoReposicao.set(false)))
			.subscribe({
				next: (resultado) => {
					this.dialog.open(ReposicaoResultadoDialog, {
						data: resultado,
						width: '720px',
						maxWidth: '90vw',
						maxHeight: '90vh',
						autoFocus: false,
						disableClose: false,
					});

					if (resultado.sucesso) {
						this.carregarProdutos();
					}
				},
				error: (erro: HttpErrorResponse) => this.notificationService.notificarErroApi(erro),
			});
	}

	protected trackByAvaliacaoId(_: number, avaliacao: AvaliacaoAdminCardViewModel): number {
		return avaliacao.id;
	}

	protected trackByProdutoId(_: number, produto: ProdutoCardViewModel): number {
		return produto.id;
	}

	protected contarEstrelas(nota: number): { cheias: number; metade: number; vazias: number } {
		let metade = 0;
		let cheias = Math.floor(nota);

		const decimal = nota - cheias;
		if (decimal >= 0.85) cheias += 1;
		else if (decimal >= 0.4) metade += 1;

		const vazias = 5 - cheias - metade;

		return { cheias, metade, vazias };
	}

	protected estrelaIcone(indice: number, nota: number): string {
		const estrelas = this.contarEstrelas(nota);
		if (indice <= estrelas.cheias) return 'star';
		if (indice === estrelas.cheias + 1 && estrelas.metade > 0) return 'star_half';
		return 'star_border';
	}

	private paraCardViewModel(avaliacao: AvaliacaoAdminDto): AvaliacaoAdminCardViewModel {
		return {
			id: avaliacao.id,
			produtoNome: avaliacao.produtoNome,
			// Stored XSS.
			emailUsuario: this.sanitizer.bypassSecurityTrustHtml(avaliacao.usuarioEmail),
			nota: avaliacao.nota,
			comentario: avaliacao.comentario,
		};
	}
}
