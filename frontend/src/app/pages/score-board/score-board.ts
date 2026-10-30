import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';

import { NotificationService } from '../../shared/notification/notification.service';
import { DesafioResponse, DicaDesafioDto } from './score-board.models';
import { ScoreBoardService } from './score-board.service';
import { SignalRService } from '../../services/signalR/signalr.service';
import { DetalhesDesafio } from './detalhes-desafio/detalhes-desafio';
import { GuiaCategoria } from './guia-categoria/guia-categoria.service';
import { GuiaCategoriaService } from './guia-categoria/guia-categoria.service';
import { GuiaCategoriaDialog } from './guia-categoria/guia-categoria-dialog';
import { DesafiosnavService } from './../../shared/desafiosnav/desafiosnav.service';

interface FragmentoTexto{
	texto: string;
	payload: boolean;
} 

type ItemSugestao = {
	tipo: 'quiz' | 'desafio';
	desafio: DesafioResponse;
};

@Component({
	selector: 'sm-score-board',
	imports: [
		CommonModule,
		MatButtonModule,
		MatCardModule,
		MatFormFieldModule,
		MatIconModule,
		MatInputModule,
		MatTooltipModule,
		MatMenuModule
	],
	templateUrl: './score-board.html',
	styleUrl: './score-board.scss',
	standalone: true,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ScoreBoard implements OnInit {
	private readonly scoreBoardService = inject(ScoreBoardService);
	private readonly guiaCategoriaService = inject(GuiaCategoriaService);
	private readonly notificationService = inject(NotificationService);
	private readonly signalRService = inject(SignalRService);
	private readonly desafionavService = inject(DesafiosnavService);
	private readonly route = inject(ActivatedRoute);

	protected readonly niveisDificuldade = [1, 2, 3, 4, 5];
	protected readonly desafios = signal<DesafioResponse[]>([]);
	protected readonly categorias = signal<string[]>([]);
	protected readonly carregandoDesafios = signal(true);
	protected readonly carregandoCategorias = signal(true);
	protected readonly erroDesafios = signal<string | null>(null);
	protected readonly erroCategorias = signal<string | null>(null);
	protected readonly termoBusca = signal('');
	protected readonly dificuldadesSelecionadas = signal<Set<number>>(new Set<number>());
	protected readonly categoriasSelecionadas = signal<Set<string>>(new Set<string>());

	protected readonly totalDesafios = computed(() => this.desafios().length);
	protected readonly totalResolvidos = computed(() => this.desafios().filter((d) => d.resolvido).length);
	protected readonly desafiosFixados = computed(() => this.desafionavService.itensDesafio().map((d) => d.id));
	protected readonly desafiosFiltrados = computed(() => {
		const termo = this.normalizarTexto(this.termoBusca());
		const dificuldadesSelecionadas = this.dificuldadesSelecionadas();
		const categoriasSelecionadas = this.categoriasSelecionadas();
	
		return this.desafios().filter((desafio) => {
			if (termo) {
				const textoBusca = `${desafio.nome} ${desafio.descricao}`;
				if (!this.normalizarTexto(textoBusca).includes(termo)) {
					return false;
				}
			}

			if (dificuldadesSelecionadas.size > 0 && !dificuldadesSelecionadas.has(desafio.dificuldade)) {
				return false;
			}

			if (categoriasSelecionadas.size > 0 && !categoriasSelecionadas.has(desafio.categoria)) {
				return false;
			}

			return true;
		});
	});

	protected readonly resolvidosPorDificuldade = computed(() => {
		const desafios = this.desafios();

		return this.niveisDificuldade.map((nivel) => ({
			nivel,
			quantidadeTotal: desafios.filter((desafio) => desafio.dificuldade === nivel).length,
			quantidadeResolvida: desafios.filter((desafio) => desafio.dificuldade === nivel && desafio.resolvido).length,
		}));
	});

	protected readonly totalQuizzes = computed(() => {
		const desafios = this.desafios();

		return desafios.filter((d) => d.possuiQuiz).length;
	});
	protected readonly totalQuizzesResolvidos = computed(() => {
		const desafios = this.desafios();

		return desafios.filter((d) => d.possuiQuiz && d.quizResolvido).length;
	});

	protected readonly todasDificuldadesSelecionadas = computed(() => this.dificuldadesSelecionadas().size === 0);
	protected readonly todasSelecionadas = computed(() => this.categoriasSelecionadas().size === 0);

	protected readonly dicaAtual = signal<DicaDesafioDto | null>(null);

	private readonly mudancasPendentes: number[] = []; //variavel criada exclusivamente para lidar com a race condition entre o WebSocket e a requisição HTTP

	// Painel de sugestões
	protected readonly carregandoSugestoes = signal(true);
	protected readonly erroSugestoes = signal<string | null>(null);

	protected readonly desafiosSugeridos = signal<DesafioResponse[]>([]);
	protected readonly QuizDesafioSugerido = signal<DesafioResponse | null>(null);

	protected readonly indiceSugestaoAtual = signal(0);

	protected readonly itensSugestoes = computed<ItemSugestao[]>(() => {
		const itens: ItemSugestao[] = [];

		const quizSugerido = this.QuizDesafioSugerido();
		if (quizSugerido !== null) itens.push({ tipo: 'quiz', desafio: quizSugerido });

		for (const desafio of this.desafiosSugeridos()) {
			itens.push({ tipo: 'desafio', desafio });
		}

		return itens;
	});

	protected readonly sugestaoAtual = computed<ItemSugestao | null>(
		() => this.itensSugestoes()[this.indiceSugestaoAtual()] ?? null,
	);

	// Para abrir Detalhes Desafio imediatamente se o parametro de query idDesafio estiver presente na URL
	private idDesafioQuery: number | null = null;

	constructor(private readonly dialog: MatDialog) {}

	ngOnInit(): void {
		this.route.queryParamMap.subscribe((params) => {
			const id = params.get('idDesafio') ?? '';
			if (id) this.idDesafioQuery = Number(id);
		});

		this.carregarDesafios();
		this.carregarCategorias();

		this.signalRService.desafioSolved$.subscribe(async (desafio) => {
			if (this.desafios().length === 0) {
				this.mudancasPendentes.push(desafio.id);
				return;
			}

			this.desafios.update((desafios) => 
				desafios.map((d) => d.id === desafio.id ? { ...d, resolvido: true } : d)
			);
		});
	}

	protected atualizarBusca(event: Event): void {
		const valor = (event.target as HTMLInputElement).value;
		this.termoBusca.set(valor);
	}

	protected limparBusca(): void {
		this.termoBusca.set('');
	}

	protected dificuldadeEstaSelecionada(nivel: number): boolean {
		return this.dificuldadesSelecionadas().has(nivel);
	}

	protected selecionarTodasDificuldades(): void {
		this.dificuldadesSelecionadas.set(new Set<number>());
	}

	protected alternarDificuldade(nivel: number): void {
		const selecionadas = new Set(this.dificuldadesSelecionadas());

		if (selecionadas.has(nivel)) {
			selecionadas.delete(nivel);
		} else {
			selecionadas.add(nivel);
		}

		this.dificuldadesSelecionadas.set(selecionadas);
	}

	protected categoriaEstaSelecionada(categoria: string): boolean {
		return this.categoriasSelecionadas().has(categoria);
	}

	protected selecionarTodasCategorias(): void {
		this.categoriasSelecionadas.set(new Set<string>());
	}

	protected alternarCategoria(categoria: string): void {
		const selecionadas = new Set(this.categoriasSelecionadas());

		if (selecionadas.has(categoria)) {
			selecionadas.delete(categoria);
		} else {
			selecionadas.add(categoria);
		}

		this.categoriasSelecionadas.set(selecionadas);
	}

	protected limparFiltros(): void {
		this.termoBusca.set('');
		this.dificuldadesSelecionadas.set(new Set<number>());
		this.categoriasSelecionadas.set(new Set<string>());
	}

	protected abrirDesafio(desafio: DesafioResponse): void {
		if (!desafio.resolvido) return;

		this.dialog.closeAll(); // para corrigir o bug de abrir múltiplos diálogos ao mesmo tempo através da desafio-notification

		this.scoreBoardService.detalhesDesafio(desafio.id).subscribe({
			next: (detalhesDesafio) => {
				this.dialog.open(DetalhesDesafio, {
					data: { detalhesDesafio },
					width: '1200px',
					maxWidth: 'none',
					maxHeight: '92vh',
					panelClass: 'detalhes-desafio-dialog',
					autoFocus: false
				});
			},
			error: () => {
				this.notificationService.erro('Erro ao carregar detalhes do desafio');
				return;
			}
		});
	}

	protected guiaDaCategoria(categoriaId: string): GuiaCategoria | null {
		return this.guiaCategoriaService.obterGuia(categoriaId);
	}

	protected abrirGuiaCategoria(guia: GuiaCategoria, categoria: string, event?: Event): void {
		event?.stopPropagation();

		this.dialog.open(GuiaCategoriaDialog, {
			data: { titulo: categoria, guia },
			width: '1000px',
			maxWidth: '95vw',
			maxHeight: '92vh',
			panelClass: 'guia-categoria-dialog',
			ariaLabel: `Guia da categoria ${categoria}`,
			autoFocus: false,
		});
	}

	protected verDicas(desafio: DesafioResponse, event?: Event): void {
		event?.stopPropagation();

		if(desafio.dicasDesafio == null || desafio.dicasDesafio.length == 0) return;

		var primeiraDica = desafio.dicasDesafio.find((desafio) => desafio.nrDica == 1)!;
		this.dicaAtual.set(primeiraDica);
	}

	protected mudarDica(desafio: DesafioResponse, avancar: boolean, event?: Event): void{
		event?.stopPropagation();

		const nrDicaAtual = this.dicaAtual()?.nrDica;
		if(!nrDicaAtual) return;

		if(avancar){
			if(nrDicaAtual == desafio.dicasDesafio.length) return; 
			this.dicaAtual.set(desafio.dicasDesafio.find((desafio) => desafio.nrDica == nrDicaAtual + 1)!)
			return;
		}

		if(nrDicaAtual == 1) return;
		this.dicaAtual.set(desafio.dicasDesafio.find((desafio) => desafio.nrDica == nrDicaAtual - 1)!)
	}

	protected trackById(_: number, desafio: DesafioResponse): number {
		return desafio.id;
	}

	protected trackByCategoria(_: number, categoria: string): string {
		return categoria;
	}

	protected trackByNivel(_: number, item: { nivel: number; quantidadeTotal: number; quantidadeResolvida: number }): number {
		return item.nivel;
	}

	protected estrelasPara(nivel: number): number[] {
		return this.niveisDificuldade.slice(0, nivel);
	}

	protected estrelasVaziasPara(nivel: number): number[] {
		return this.niveisDificuldade.slice(nivel);
	}

	private carregarDesafios(): void {
		this.carregandoDesafios.set(true);

		this.scoreBoardService.listarDesafios().subscribe({
			next: (desafios) => {
				if (this.mudancasPendentes.length > 0) {
					desafios = desafios.map((desafio) => {
						if (this.mudancasPendentes.includes(desafio.id)) return { ...desafio, resolvido: true };						
						return desafio;
					});
				}
				this.desafios.set(desafios);
				this.erroDesafios.set(null);
				this.carregandoDesafios.set(false);

				// Depois de obter os desafios, monta as sugestões de desafios
				this.montarSugestoes();

				// Para abrir Detalhes Desafio imediatamente se o parametro de query idDesafio estiver presente na URL
				if (this.idDesafioQuery !== null) {
					const cardViewModel = this.desafios().find(p => p.id === Number(this.idDesafioQuery));
					if (cardViewModel) 
						this.abrirDesafio(cardViewModel);
				}
			},
			error: (erro) => {
				console.error('Erro ao carregar lista de desafios:', erro);
				this.erroDesafios.set('Nenhum desafio carregado');
				this.notificationService.erro('Erro ao carregar lista de desafios');
				this.carregandoDesafios.set(false);
				this.erroSugestoes.set('Nenhuma sugestão carregada');
				this.carregandoSugestoes.set(false);
			},
		});
	}

	private carregarCategorias(): void {
		this.carregandoCategorias.set(true);

		this.scoreBoardService.listarCategorias().subscribe({
			next: (categorias) => {
				this.categorias.set(categorias);
				this.erroCategorias.set(null);
				this.carregandoCategorias.set(false);
			},
			error: (erro) => {
				console.error('Erro ao carregar categorias:', erro);
				this.categorias.set([]);
				this.erroCategorias.set('Nenhuma categoria carregada');
				this.notificationService.erro('Erro ao carregar categorias');
				this.carregandoCategorias.set(false);
			},
		});
	}

	private normalizarTexto(valor: string): string {
		return valor
			.normalize('NFD')
			.replace(/[\u0300-\u036f]/g, '')
			.toLowerCase();
	}

	protected dividirDescricaoEPaylaod(descricao: string): FragmentoTexto[]{
		return descricao.split("|")
			.map((texto, index) => ({texto, payload: index % 2 === 1}));
	}

	protected async copiar(texto: string, event?: Event): Promise<void>{
		event?.stopPropagation();
		await navigator.clipboard.writeText(texto);
		
		this.notificationService.info("Texto copiado!");
	}

	protected mudarEstadoFixarDesafio(desafio: DesafioResponse, event?: Event): void {
		event?.stopPropagation();
		
		if (this.desafiosFixados().find((d) => d == desafio.id)){
			this.desafionavService.removerItemDesafio(desafio.id);
		} else{
			this.desafionavService.adicionarItemDesafio(
				desafio.id,
				desafio.nome,
				desafio.descricao,
				desafio.dificuldade,
				desafio.resolvido,
				desafio.rota,
			);
		}
	}

	// Painel de sugestões
	protected mudarSugestao(avancar: boolean): void {
		const total = this.itensSugestoes().length;
		if (total === 0) return;

		const indice = this.indiceSugestaoAtual();
		this.indiceSugestaoAtual.set((indice + (avancar ? 1 : -1) + total) % total);
	}

	private montarSugestoes(): void {
		this.carregandoSugestoes.set(true);

		this.scoreBoardService.ObterSugestoes(3).subscribe({
			next: (desafios) => {
				this.desafiosSugeridos.set(desafios);
				this.indiceSugestaoAtual.set(0);

				this.atribuirSugestaoQuizDesafio();
				this.erroSugestoes.set(null);
				this.carregandoSugestoes.set(false);
			},
			error: () => {
				this.notificationService.erro('Erro ao carregar sugestões de desafios');
				this.erroSugestoes.set('Nenhuma sugestão carregada');
				this.carregandoSugestoes.set(false);
			},
		});
	}

	private atribuirSugestaoQuizDesafio(): void {
		if (this.desafios().length == 0) return;

		const desafio = this.desafios().find((d) => d.possuiQuiz && !d.quizResolvido && d.resolvido) ?? null;
		this.QuizDesafioSugerido.set(desafio);
	}
}