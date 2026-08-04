import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

import { AuthSessionService } from '../../shared/auth/auth-session.service';
import { ScoreBoardService } from '../../pages/score-board/score-board.service';
import { SignalRService } from '../../services/signalR/signalr.service';

@Component({
	selector: 'sm-sidenav',
	imports: [MatIconModule, MatTooltipModule, RouterLink, RouterLinkActive],
	templateUrl: './sidenav.html',
	styleUrl: './sidenav.scss',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidenavComponent implements OnInit {
	private readonly scoreBoardService = inject(ScoreBoardService);
	private readonly signalRService = inject(SignalRService);
	protected readonly auth = inject(AuthSessionService);
	private readonly router = inject(Router);

	protected readonly expanded = signal(false);
	protected readonly showScoreBoard = signal(false);

	protected readonly toggleIcon = computed(() => (this.expanded() ? 'chevron_left' : 'chevron_right'));
	protected readonly toggleLabel = computed(() => (this.expanded() ? '' : ''));
	protected readonly toggleTooltip = computed(() => (this.expanded() ? 'Recolher menu' : 'Expandir menu'));

	ngOnInit(): void {
		this.scoreBoardService.buscarDesafioPorNome('Encontrar a Score-Board').subscribe((desafio) => {
			if (desafio && desafio.resolvido) {
				this.showScoreBoard.set(true);
			}
		});

		this.signalRService.desafioSolved$.subscribe((desafio) => {
			if (desafio && desafio.resolvido && desafio.nome === 'Encontrar a Score-Board') {
				this.showScoreBoard.set(true);
			}
		});
	}

	protected toggle(): void {
		this.expanded.update((v) => !v);
	}

	protected carrinhoClick(): void {
		console.log('icone Carrinho clicado');
	}

	protected avaliacoesClick(): void {
		console.log('icone Avaliações clicado');
	}

	protected githubClick(): void {
		console.log('icone GitHub clicado');
	}

	protected logout(): void {
		this.auth.limparSessao();
		void this.router.navigate(['/catalogo']);
	}

	protected onFotoError(event: Event): void {
		const img = event.target as HTMLImageElement;
		img.src = '/imagens/perfil/generic_profile.jpg';
	}
}