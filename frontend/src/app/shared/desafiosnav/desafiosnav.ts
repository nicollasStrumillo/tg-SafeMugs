import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal, DestroyRef } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { SignalRService } from '../../services/signalR/signalr.service';

import { NotificationService } from '../notification/notification.service';

import { DesafiosnavService } from './desafiosnav.service';

interface FragmentoTexto {
	texto: string;
	payload: boolean;
}

@Component({
	selector: 'sm-desafionav',
	imports: [MatButtonModule, MatIconModule, MatTooltipModule],
	templateUrl: './desafiosnav.html',
	styleUrl: './desafiosnav.scss',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Desafiosnav implements OnInit {
	private readonly signalRService = inject(SignalRService);
	private readonly desafiosnavService = inject(DesafiosnavService);
	private readonly notificationService = inject(NotificationService);
	private readonly router = inject(Router);
	private readonly destroyRef = inject(DestroyRef);

	private readonly itensDesafio = this.desafiosnavService.itensDesafio;

	protected readonly niveisDificuldade = [1, 2, 3, 4, 5];

	protected readonly itensRotaAtiva = computed(() => this.itensDesafio().filter((d) => !d.resolvido && d.isRotaAtiva));
	protected readonly itensNaoResolvidos = computed(() => this.itensDesafio().filter((d) => !d.resolvido && !d.isRotaAtiva));
	protected readonly itensResolvidos = computed(() => this.itensDesafio().filter((d) => d.resolvido));
	
	protected readonly expanded = signal(true);

	protected readonly toggleIcon = computed(() => (this.expanded() ? 'chevron_right' : 'chevron_left'));
	protected readonly toggleTooltip = computed(() => (this.expanded() ? 'Recolher menu' : 'Expandir menu'));

	ngOnInit(): void {
		this.router.events
			.pipe(filter((event) => event instanceof NavigationEnd), takeUntilDestroyed(this.destroyRef))
			.subscribe((event) => {
				const url = (event as NavigationEnd).urlAfterRedirects; // "/catalogo", "/carrinho"...
				this.desafiosnavService.marcarRotaAtivaDesafios(url);
			});

		this.signalRService.desafioSolved$.subscribe((desafio) => {
			this.desafiosnavService.marcarResolucaoDesafio(desafio.id);
		});
	}

	public adicionarItemDesafio(
		id: number,
		nome: string,
		descricao: string,
		dificuldade: number,
		resolvido: boolean,
		rota: string | null,
	): void {
		this.desafiosnavService.adicionarItemDesafio(id, nome, descricao, dificuldade, resolvido, rota);
	}

	public removerItemDesafio(id: number): void {
		this.desafiosnavService.removerItemDesafio(id);
	}

	protected dividirDescricaoEPaylaod(descricao: string): FragmentoTexto[] {
		return descricao.split('|').map((texto, index) => ({ texto, payload: index % 2 === 1 }));
	}

	protected async copiar(texto: string, event?: Event): Promise<void> {
		event?.stopPropagation();
		await navigator.clipboard.writeText(texto);

		this.notificationService.info('Texto copiado!');
	}

    protected toggle(): void {
		this.expanded.update((v) => !v);
	}
}