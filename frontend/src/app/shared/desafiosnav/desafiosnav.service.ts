import { Injectable, inject, signal } from '@angular/core';

import { BrowserCookieService } from '../../services/cookies/browser-cookies.service';
import { ScoreBoardService } from '../../pages/score-board/score-board.service';
import { NotificationService } from '../notification/notification.service';

export interface ItemDesafio {
  id: number;
  nome: string;
  descricao: string;
  dificuldade: number;
  resolvido: boolean;
  rota: string | null;
  isRotaAtiva: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class DesafiosnavService {
  private readonly cookiesService = inject(BrowserCookieService);
  private readonly scoreBoardService = inject(ScoreBoardService);
  private readonly notificationService = inject(NotificationService);

  private readonly _itensDesafio = signal<ItemDesafio[]>([]);
  readonly itensDesafio = this._itensDesafio.asReadonly();

  private rotaAtivaAtual: string | null = null;

  constructor() {
    const ids = this.cookiesService.getItensDesafionav();
    if (ids && ids.length > 0) {
      this.recuperLista(ids);
    }
  }

  public adicionarItemDesafio(
    id: number,
    nome: string,
    descricao: string,
    dificuldade: number,
    resolvido: boolean,
    rota: string | null,
  ): void {
    if (this._itensDesafio().some((i) => i.id === id)) return;

    const novoItem = this.criarItemDesafio(id, nome, descricao, dificuldade, resolvido, rota);
    novoItem.isRotaAtiva = this.rotaAtivaAtual !== null && novoItem.rota === this.rotaAtivaAtual;
    this._itensDesafio.update((itens) => [...itens, novoItem]);
    this.persistirIds();
  }

  public removerItemDesafio(id: number): void {
    const antes = this._itensDesafio().length;
    this._itensDesafio.update((itens) => itens.filter((i) => i.id !== id));
    if (this._itensDesafio().length !== antes) {
      this.persistirIds();
    }
  }

  private recuperLista(ids: number[]): void {
    this.scoreBoardService.listarDesafios().subscribe({
      next: (desafios) => {
        const desafiosFixados = desafios.filter((d) => ids.includes(d.id));
        this._itensDesafio.set(
          desafiosFixados.map((d) =>
            this.criarItemDesafio(d.id, d.nome, d.descricao, d.dificuldade, d.resolvido, d.rota),
          ),
        );
        this.aplicarRotaAtiva();
      },
      error: () => {
        this.notificationService.erro('Não foi possível recuperar os desafios fixados no menu lateral.');
      },
    });
  }

  public marcarResolucaoDesafio(id: number): void {
    this._itensDesafio.update((itens) =>
      itens.map((d) => (d.id === id ? { ...d, resolvido: true } : d)),
    );
  }

  public marcarRotaAtivaDesafios(rota: string): void {
    this.rotaAtivaAtual = rota.split('?')[0];
    this.aplicarRotaAtiva();
  }

  private aplicarRotaAtiva(): void {
    if (this.rotaAtivaAtual === null) return;

    const rotaAtiva = this.rotaAtivaAtual;
    this._itensDesafio.update((itens) =>
      itens.map((d) => ({ ...d, isRotaAtiva: d.rota === rotaAtiva })),
    );
  }

  private criarItemDesafio(
    id: number,
    nome: string,
    descricao: string,
    dificuldade: number,
    resolvido: boolean,
    rota: string | null
  ): ItemDesafio {
    return { id, nome, descricao, dificuldade, resolvido, rota, isRotaAtiva: false };
  }

  private persistirIds(): void {
    const ids = this._itensDesafio().map((i) => i.id);
    this.cookiesService.setItensDesafionav(ids);
  }
}
