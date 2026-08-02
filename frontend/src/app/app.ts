import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

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
}