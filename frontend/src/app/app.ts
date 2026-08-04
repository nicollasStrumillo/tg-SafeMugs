import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { AuthSessionService } from './services/usuario/auth/auth-session.service';
import { CookieNotification } from './shared/cookie-notification/cookie-notification';
import { DesafioNotification } from './shared/desafio-notification/desafio-notification';
import { FooterComponent } from './shared/footer/footer';
import { NotificationHostComponent } from './shared/notification/notification-host';
import { SidenavComponent } from './shared/sidenav/sidenav';

@Component({
	selector: 'sm-root',
	imports: [
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
	],
	templateUrl: './app.html',
	styleUrl: './app.scss',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class App {
	protected readonly appName = 'SafeMugs';
	protected readonly auth = inject(AuthSessionService);
	private readonly router = inject(Router);

	protected logout(): void {
		this.auth.limparSessao();
		void this.router.navigate(['/catalogo']);
	}

	protected onFotoError(event: Event): void {
		const img = event.target as HTMLImageElement;
		img.src = '/imagens/perfil/generic_profile.jpg';
	}
}