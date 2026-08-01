import { ChangeDetectionStrategy, Component, Inject, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';

import hljs from 'highlight.js/lib/core';
import csharp from 'highlight.js/lib/languages/csharp';
import typescript from 'highlight.js/lib/languages/typescript';
import xml from 'highlight.js/lib/languages/xml';

import { DetalhesDesafioModel, DicaDesafioDto } from '../score-board.models';
import { ScoreBoardService } from '../score-board.service';
import { NotificationService } from '../../../shared/notification/notification.service';
import { BrowserCookieService } from '../../../services/cookies/browser-cookies.service';

hljs.registerLanguage('csharp', csharp);
hljs.registerLanguage('typescript', typescript);
hljs.registerLanguage('xml', xml);

interface FragmentoTexto {
	texto: string;
	payload: boolean;
}

@Component({
	selector: 'sm-detalhes-desafio',
	imports: [
		CommonModule,
		MatButtonModule,
		MatCardModule,
		MatIconModule,
		MatMenuModule,
		MatTooltipModule,
	],
	templateUrl: './detalhes-desafio.html',
	styleUrl: './detalhes-desafio.scss',
	standalone: true,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DetalhesDesafio implements OnInit {
	private readonly scoreBoardService = inject(ScoreBoardService);
	private readonly notificationService = inject(NotificationService);
	private readonly cookieService = inject(BrowserCookieService);
	protected readonly desafio = signal<DetalhesDesafioModel>({} as DetalhesDesafioModel);
	protected readonly linhasSelecionadas = signal<Set<number>>(new Set<number>());
	protected readonly mensagemResultadoQuiz = signal<string | null>(null);
	protected readonly carregandoResolucao = signal(false);
	protected readonly tentouResolver = signal(false);
	
	protected readonly possuiQuiz = computed(() => this.desafio().possuiQuiz);
	protected readonly quizResolvido = computed(() => this.desafio().quizResolvido);
	protected readonly linhasQuiz = computed(() => this.desafio().quizDesafio?.linhasQuiz ?? []);
	protected readonly linhasCorretas = computed(() => this.desafio().quizDesafio?.linhasCorretas ?? []);
	protected readonly mensagemSeguro = computed(() => this.desafio().quizDesafio?.mensagemSeguro ?? '');
	protected readonly linhasSeguro = computed(() => this.desafio().quizDesafio?.linhasCodigoSeguro ?? []);

	protected readonly temDicas = computed(() => (this.desafio().dicasDesafio?.length ?? 0) > 0);
	protected readonly quantidadeDicas = computed(() => this.desafio().dicasDesafio?.length ?? 0);
	protected readonly dicaAtual = signal<DicaDesafioDto | null>(null);

	protected readonly niveisDificuldade = [1, 2, 3, 4, 5];

	constructor(
		@Inject(MAT_DIALOG_DATA) public data: { detalhesDesafio: DetalhesDesafioModel },
		private readonly dialogRef: MatDialogRef<DetalhesDesafio>,
	) {}

	ngOnInit(): void {
		this.desafio.set(this.data.detalhesDesafio);
	}

	protected fechar(): void {
		this.dialogRef.close();
	}

	protected toggleLinha(numeroLinha: number): void {
		if (this.quizResolvido()) return;

		this.linhasSelecionadas.update((selecionadas) => {
			const novo = new Set(selecionadas);
			if (novo.has(numeroLinha)) {
				novo.delete(numeroLinha);
			} else {
				novo.add(numeroLinha);
			}
			return novo;
		});

		this.mensagemResultadoQuiz.set(null);
		this.tentouResolver.set(false);
	}

	protected estaSelecionada(numeroLinha: number): boolean {
		return this.linhasSelecionadas().has(numeroLinha);
	}

	// Utilizado para destacar as linhas corretas na renderizção de Quizzes JÁ RESOLVIDOS
	protected estaCorreta(numeroLinha: number): boolean {
		return this.linhasCorretas().includes(numeroLinha);
	}

	protected resolverQuiz(): void {
		if (this.carregandoResolucao() || this.quizResolvido()) return;

		this.carregandoResolucao.set(true);
		this.tentouResolver.set(true);

		const linhas = Array.from(this.linhasSelecionadas()).sort((a, b) => a - b);

		this.scoreBoardService.resolverQuizDesafio(this.desafio().id, linhas).subscribe({
			next: (resposta) => {
				this.carregandoResolucao.set(false);

				if (resposta.sucesso) {
					this.desafio.update((d) => ({ ...d, quizResolvido: true }));
					this.mensagemResultadoQuiz.set(null);
					this.notificationService.sucesso('Resolvido com sucesso!');

					this.scoreBoardService.gerarBackupQuizzes().subscribe((backup) => {
						if (backup) {
							this.cookieService.setBackupQuizzesCookie(backup);
						} else {
							console.error('Erro ao gerar backup dos quizzes.');
						}
					});
				} else {
					this.mensagemResultadoQuiz.set(resposta.mensagem);
					setTimeout(() => this.tentouResolver.set(false), 300);
				}
			},
			error: () => {
				this.carregandoResolucao.set(false);
				this.notificationService.erro('Erro ao validar solução do desafio de código.');
				this.tentouResolver.set(false);
			},
		});
	}

	protected mostrarDicas(): void {
		if (!this.temDicas()) return;

		const primeiraDica = this.desafio().dicasDesafio.find((d) => d.nrDica === 1);
		if (primeiraDica) {
			this.dicaAtual.set(primeiraDica);
		}
	}

	protected mudarDica(avancar: boolean, event?: Event): void {
		event?.stopPropagation();

		const dicas = this.desafio().dicasDesafio;
		const nrDicaAtual = this.dicaAtual()?.nrDica;
		if (!nrDicaAtual || !dicas) return;

		const proximoNr = avancar ? nrDicaAtual + 1 : nrDicaAtual - 1;
		if (proximoNr < 1 || proximoNr > dicas.length) return;

		const proximaDica = dicas.find((d) => d.nrDica === proximoNr);
		if (proximaDica) {
			this.dicaAtual.set(proximaDica);
		}
	}

	protected highlightCodigo(codigo: string): string {
		const linguagem = this.desafio().quizDesafio?.linguagem?.toLowerCase() ?? 'csharp';
		//O Highlight.js não reconhece "html" como linguagem, mas sim "xml".
		const langMap: Record<string, string> = {
			csharp: 'csharp',
			typescript: 'typescript',
			html: 'xml',
		};
		const lang = langMap[linguagem] ?? 'csharp';

		try {
			return hljs.highlight(codigo, { language: lang }).value;
		} catch {
			return codigo;
		}
	}

	protected dividirDescricaoEPayload(descricao: string): FragmentoTexto[] {
		return descricao.split('|').map((texto, index) => ({
			texto,
			payload: index % 2 === 1,
		}));
	}

	protected async copiar(texto: string): Promise<void> {
		await navigator.clipboard.writeText(texto);
		this.notificationService.info('Payload copiado!');
	}

	protected trackByLinha(index: number): number {
		return index + 1;
	}
}
