import { ChangeDetectionStrategy, Component, computed, inject, signal, type OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { SignalRService } from '../signalR/signalr.service';
import { DesafioResponse } from '../../pages/score-board/score-board.models';

interface DesafioSolvedNotification {
  id: number
  nomeDesafio: string
  descricaoDesafio: string
  dificuldade: number
}

@Component({
  selector: 'sm-desafio-notification',
  imports: [
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './desafio-notification.html',
  styleUrl: './desafio-notification.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true
})
export class DesafioNotification implements OnInit {
  private readonly signalRService = inject(SignalRService);

  protected readonly notifications = signal<DesafioSolvedNotification[]>([]);
  protected readonly indiceAtual = signal(0);
  protected readonly notificacaoAtual = computed(() => this.notifications()[this.indiceAtual()] ?? null);
  
  ngOnInit(): void {
    this.signalRService.desafioSolved$.subscribe(async desafio => {
      this.showNotification(desafio);
    });
  }

  public showNotification(desafio: DesafioResponse): void {
    this.notifications.update(notifications => [{
        id: desafio.id,
        nomeDesafio: desafio.nome,
        descricaoDesafio: desafio.descricao,
        dificuldade: desafio.dificuldade
    }, ...notifications, ]);
  }

  protected async fecharNotificacao(): Promise<void> {
    const indiceAtual = this.indiceAtual();
    const notifications = this.notifications();

    if (notifications.length === 0) {
      return;
    }

    const notificationToRemove = notifications[indiceAtual];
    await this.signalRService.aknowledgeNotification(notificationToRemove.id);

    const notificacoesAtualizadas = notifications.filter((_, indice) => indice !== indiceAtual);
    this.notifications.set(notificacoesAtualizadas);

    if (notificacoesAtualizadas.length === 0) {
      this.indiceAtual.set(0);
      return;
    }

    this.indiceAtual.set(Math.min(indiceAtual, notificacoesAtualizadas.length - 1));
  }

  protected async fecharTodasNotificacoes(): Promise<void> {
    const notifications = this.notifications();
    for (const notification of notifications) {
      await this.signalRService.aknowledgeNotification(notification.id);
    }
    this.notifications.set([]);
    this.indiceAtual.set(0);
  }

  protected anteriorNotificacao(): void {
    if (this.indiceAtual() === 0) {
      return;
    }

    this.indiceAtual.update(indice => indice - 1);
  }

  protected proximaNotificacao(): void {
    if (this.indiceAtual() >= this.notifications().length - 1) {
      return;
    }

    this.indiceAtual.update(indice => indice + 1);
  }

  trackById(_: number, notification: DesafioSolvedNotification): number {
    return notification.id;
  }
}
