import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { AuthSessionService } from './services/usuario/auth/auth-session.service';
import { ScoreBoardService } from './pages/score-board/score-board.service';
import { SignalRService } from './services/signalR/signalr.service'; 
import { BoasVindasNotification } from './shared/boas-vindas-notification/boas-vindas-notification';
import { CookieNotification } from './shared/cookie-notification/cookie-notification';
import { DesafioNotification } from './shared/desafio-notification/desafio-notification';
import { FooterComponent } from './shared/footer/footer';
import { NotificationHostComponent } from './shared/notification/notification-host';
import { SidenavComponent } from './shared/sidenav/sidenav';
import { Desafiosnav } from './shared/desafiosnav/desafiosnav';

@Component({
	selector: 'sm-root',
	imports: [
    BoasVindasNotification,
    CookieNotification,
    DesafioNotification,
    FooterComponent,
    MatButtonModule,
    MatIcon,
    MatToolbarModule,
    NotificationHostComponent,
    RouterLink,
    RouterLinkActive,
    RouterOutlet,
    SidenavComponent,
    DesafioNotification,
    Desafiosnav
],
	templateUrl: './app.html',
	styleUrl: './app.scss',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class App {
	protected readonly appName = 'SafeMugs';
	protected readonly auth = inject(AuthSessionService);
	private readonly router = inject(Router);
	private readonly scoreBoardService = inject(ScoreBoardService);
	private readonly signalRService = inject(SignalRService);

	protected readonly showDesafiosNav = signal(false);

	ngOnInit(): void {
		this.scoreBoardService.buscarDesafioPorNome('Encontrar a Score-Board').subscribe((desafio) => {
			if (desafio && desafio.resolvido) {
				this.showDesafiosNav.set(true);
			}
		});

		this.signalRService.desafioSolved$.subscribe((desafio) => {
			if (desafio && desafio.resolvido && desafio.nome === 'Encontrar a Score-Board') {
				this.showDesafiosNav.set(true);
			}
		});
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