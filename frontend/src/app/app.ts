import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { FooterComponent } from './shared/footer/footer';
import { CookieNotification } from './shared/cookie-notification/cookie-notification';
import { NotificationHostComponent } from './shared/notification/notification-host';
import { DesafioNotification } from './shared/desafio-notification/desafio-notification';
import { MatIcon } from "@angular/material/icon";
import { SignalRService } from './services/signalR/signalr.service';
import { ScoreBoardService } from './pages/score-board/score-board.service';

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
    MatIcon
],
  templateUrl: './app.html',
  styleUrl: './app.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class App implements OnInit {
  protected readonly appName = 'SafeMugs';

  private readonly signalRService = inject(SignalRService);
  private readonly scoreBoardService = inject(ScoreBoardService);

  protected readonly showScoreBoard = signal(false);

  ngOnInit(): void {
    this.scoreBoardService.buscarDesafioPorNome("Encontrar a Score-Board").subscribe(desafio => {
      if (desafio && desafio.resolvido) {
        this.showScoreBoard.set(true);
      }
    });

    this.signalRService.desafioSolved$.subscribe(async desafio => {
      if (desafio && desafio.resolvido) {
        this.showScoreBoard.set(true);
      }
    });
  }
}
