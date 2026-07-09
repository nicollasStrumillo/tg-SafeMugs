import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { FooterComponent } from './shared/footer/footer';
import { CookieNotification } from './shared/cookie-notification/cookie-notification';
import { NotificationHostComponent } from './shared/notification/notification-host';
import { DesafioNotification } from './shared/desafio-notification/desafio-notification';

@Component({
  selector: 'sm-root',
  imports: [
    CookieNotification,
    FooterComponent,
    DesafioNotification,
    MatButtonModule,
    MatToolbarModule,
    NotificationHostComponent,
    RouterLink,
    RouterLinkActive,
    RouterOutlet,
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class App {
  protected readonly appName = 'SafeMugs';

}
