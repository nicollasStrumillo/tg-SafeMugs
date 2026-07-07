import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';

import { NotificationService } from '../../shared/notification/notification.service';
import { DesafioResponse, DicaDesafioDto } from './score-board.models';
import { ScoreBoardService } from './score-board.service';

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
	private readonly notificationService = inject(NotificationService);

	protected readonly niveisDificuldade = [1, 2, 3, 4, 5];
	protected readonly desafioResolvidos = signal<Set<number>>(new Set<number>());
	protected readonly desafios = signal<DesafioResponse[]>([]);
	protected readonly categorias = signal<string[]>([]);
	protected readonly carregandoDesafios = signal(true);
	protected readonly carregandoCategorias = signal(true);
	protected readonly erroDesafios = signal<string | null>(null);
	protected readonly erroCategorias = signal<string | null>(null);
	protected readonly termoBusca = signal('');
	protected readonly dificuldadeSelecionada = signal<number | null>(null);
	protected readonly categoriasSelecionadas = signal<Set<string>>(new Set<string>());

	protected readonly totalDesafios = computed(() => this.desafios().length);
	protected readonly totalResolvidos = computed(() => this.desafioResolvidos().size);
	protected readonly desafiosFiltrados = computed(() => {
		const termo = this.normalizarTexto(this.termoBusca());
		const dificuldade = this.dificuldadeSelecionada();
		const categoriasSelecionadas = this.categoriasSelecionadas();

		return this.desafios().filter((desafio) => {
			if (termo) {
				const textoBusca = `${desafio.nome} ${desafio.descricao}`;
				if (!this.normalizarTexto(textoBusca).includes(termo)) {
					return false;
				}
			}

			if (dificuldade !== null && desafio.dificuldade !== dificuldade) {
				return false;
			}

			if (categoriasSelecionadas.size > 0 && !categoriasSelecionadas.has(desafio.categoria)) {
				return false;
			}

			return true;
		});
	});

	protected readonly resolvidosPorDificuldade = computed(() => {
		const resolvidos = this.desafioResolvidos();
		const desafios = this.desafios();

		return this.niveisDificuldade.map((nivel) => ({
			nivel,
			quantidadeTotal: desafios.filter((desafio) => desafio.dificuldade === nivel).length,
			quantidadeResolvida: desafios.filter((desafio) => desafio.dificuldade === nivel && resolvidos.has(desafio.id)).length,
		}));
	});

	protected readonly todasSelecionadas = computed(() => this.categoriasSelecionadas().size === 0);

	protected readonly dicaAtual = signal<DicaDesafioDto | null>(null);

	ngOnInit(): void {
		this.carregarDesafios();
		this.carregarCategorias();
	}

	protected atualizarBusca(event: Event): void {
		const valor = (event.target as HTMLInputElement).value;
		this.termoBusca.set(valor);
	}

	protected limparBusca(): void {
		this.termoBusca.set('');
	}

	protected selecionarDificuldade(nivel: number | null): void {
		this.dificuldadeSelecionada.set(this.dificuldadeSelecionada() === nivel ? null : nivel);
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
		this.dificuldadeSelecionada.set(null);
		this.categoriasSelecionadas.set(new Set<string>());
	}

	protected abrirDesafio(desafio: DesafioResponse): void {
		console.log(desafio.id);
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

	protected ehResolvido(desafio: DesafioResponse): boolean {
		return this.desafioResolvidos().has(desafio.id) || desafio.dicasDesafio.length == 0;
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
				this.desafios.set(desafios);
				this.erroDesafios.set(null);
				this.carregandoDesafios.set(false);
			},
			error: (erro) => {
				console.error('Erro ao carregar lista de desafios:', erro);
				this.erroDesafios.set('Nenhum desafio carregado');
				this.notificationService.erro('Erro ao carregar lista de desafios');
				this.carregandoDesafios.set(false);
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
}