import { ChangeDetectionStrategy, Component, computed, inject, signal, type OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { DesafioResponse } from '../../pages/score-board/score-board.models';

import { SignalRService } from '../../services/signalR/signalr.service';
import { BrowserCookieService } from '../../services/cookies/browser-cookies.service';
import { ScoreBoardService } from '../../pages/score-board/score-board.service';
import { NotificationService } from '../notification/notification.service';


interface DesafioSolvedNotification {
  id: number
  nomeDesafio: string
  descricaoDesafio: string
  dificuldade: number
  restored?: boolean
}

interface FragmentoTexto{
	texto: string;
	payload: boolean;
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
  private readonly cookieService = inject(BrowserCookieService);
  private readonly scoreBoardService = inject(ScoreBoardService);
  private readonly notificationService = inject(NotificationService);

  protected readonly notifications = signal<DesafioSolvedNotification[]>([]);
  protected readonly indiceAtual = signal(0);
  protected readonly notificacaoAtual = computed(() => this.notifications()[this.indiceAtual()] ?? null);
  
  ngOnInit(): void {
    const backupString = this.cookieService.getBackupDesafiosCookie(); 
    if (backupString) {
      this.scoreBoardService.restaurarDesafios(backupString).subscribe(restoredCount => {
        if (restoredCount > 0) {
          console.log(`Restaurados ${restoredCount} desafios a partir do backup.`);
        } else {
          console.log("Nenhum desafio foi restaurado a partir do backup.");
        }
      });
    }

    this.signalRService.desafioSolved$.subscribe(async desafio => {
      this.showNotification(desafio);

      if (!desafio.isRestored) {
        await this.saveProgress();
      }
    }); 
  }

  public showNotification(desafio: DesafioResponse): void {
    this.notifications.update(notifications => [{
        id: desafio.id,
        nomeDesafio: desafio.nome,
        descricaoDesafio: desafio.descricao,
        dificuldade: desafio.dificuldade,
        restored: desafio.isRestored ?? false
    }, ...notifications, ]);
  }

  private async saveProgress() : Promise<void> {
    this.scoreBoardService.gerarBackupDesafios().subscribe(backup => {
      if (backup) {
        this.cookieService.setBackupDesafiosCookie(backup);
      }else {
        console.error("Erro ao gerar backup dos desafios.");
      }
    });
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

  protected async limparCookies(): Promise<void> {
    this.cookieService.clear();

    await this.fecharTodasNotificacoes();

    this.notificationService.info('Cookies limpos com sucesso. Reinicie a aplicação para começar de novo.', {durationMs: 90000});
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

  protected dividirDescricaoEPaylaod(descricao: string): FragmentoTexto[]{
		return descricao.split("|")
			.map((texto, index) => ({texto, payload: index % 2 === 1}));
	}

	protected async copiar(texto: string){
		await navigator.clipboard.writeText(texto);
		
		this.notificationService.info("Payload copiado!");
		
	}
}
