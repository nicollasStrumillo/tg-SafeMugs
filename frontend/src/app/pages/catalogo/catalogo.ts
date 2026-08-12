import { CommonModule } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';

// Parte da vulnerabilidade de DOM XSS
import { DomSanitizer, type SafeHtml } from '@angular/platform-browser'

import { ProdutoService } from '../../services/produto/produto.service';
import { SignalRService } from '../../services/signalR/signalr.service';

import { ProdutoCardViewModel } from '../../services/produto/produto.models';
import { DetalhesProduto } from './detalhes-produto/detalhes-produto';

// Parte da vulnerabilidade de DOM XSS
const DOMXSS_PAYLOAD = '<iframe src="javascript:alert(`XSS`)">';

@Component({
  selector: 'sm-catalogo',
  imports: [
    CommonModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
  ],
  templateUrl: './catalogo.html',
  styleUrl: './catalogo.scss',
  standalone: true
})
export class Catalogo implements OnInit {
  private readonly produtoService = inject(ProdutoService);
  private readonly signalRService = inject(SignalRService);
  private readonly route = inject(ActivatedRoute);

  // Parte da vulnerabilidade de DOM XSS
  private readonly sanitizer = inject(DomSanitizer)

  readonly carregando = signal(true);
  readonly erroCarregamento = signal<string | null>(null);
  readonly todosProdutos = signal<ProdutoCardViewModel[]>([]);

  readonly termoBusca = signal<string>('');

  // Parte da vulnerabilidade de DOM XSS
  readonly termoBuscaInseguro = computed<SafeHtml>(() => {
    let termo = this.termoBusca();
    termo = termo.trim();
    return this.sanitizer.bypassSecurityTrustHtml(termo);
  });

  readonly produtosFiltrados = signal<ProdutoCardViewModel[]>([]);

  constructor(private readonly dialog: MatDialog) {}

  ngOnInit() {
    let idProdutoQuery: number | null = null;

    this.route.queryParamMap.subscribe((params) => {
			const id = params.get('idProduto') ?? '';
      if (id) idProdutoQuery = Number(id);
		});

    this.produtoService.listarProdutos().subscribe({
      next: (produtos) => {
        this.todosProdutos.set(produtos);
        this.produtosFiltrados.set(produtos);
        this.erroCarregamento.set(null);
        this.carregando.set(false);

        if (idProdutoQuery) {
          const cardViewModel = produtos.find(p => p.id === Number(idProdutoQuery));
          if (cardViewModel) 
            this.abrirDetalhes(cardViewModel);
        }
      },
      error: (err) => {
        console.error('Erro ao buscar produtos:', err);
        this.erroCarregamento.set('Nao foi possivel carregar o catalogo no momento.');
        this.carregando.set(false);
      }
    });
  }

  atualizarBusca(event: Event) {
    const valor = (event.target as HTMLInputElement).value;
    this.termoBusca.set(valor);
  }

  async buscar() {
    // Parte da vulnerabilidade de DOM XSS
    if (this.termoBusca().trim() === DOMXSS_PAYLOAD) {
      await this.signalRService.SolveDesafioDomXss(DOMXSS_PAYLOAD);
    }

    const termo = this.normalizarTexto(this.termoBusca());
    if (!termo) {
      this.produtosFiltrados.set(this.todosProdutos());
      return;
    }

    this.produtosFiltrados.set(this.todosProdutos().filter((produto) => {
      const camposBusca = [produto.nome, produto.descricao, produto.categoria].join(' ');
      return this.normalizarTexto(camposBusca).includes(termo);
    }));
  }

  trackById(_: number, produto: ProdutoCardViewModel): number {
    return produto.id;
  }

  limparBusca(): void {
    this.termoBusca.set('');
    this.produtosFiltrados.set(this.todosProdutos());
  }

  resumirDescricao(descricao: string, limite = 120): string {
    if (descricao.length <= limite) {
      return descricao;
    }

    return `${descricao.slice(0, limite).trimEnd()}...`;
  }

  formataAvaliacao(media: number | null): string {
    return media ? media.toFixed(1) : 'Novo';
  }

  abrirDetalhes(produto: ProdutoCardViewModel): void {
    this.dialog.open(DetalhesProduto, {
      data: {
        produto: produto
      },
      width: '1100px',
      maxWidth: '95vw',
      maxHeight: '90vh',
      autoFocus: false
    });

    console.debug('Abrir detalhes do produto', produto.id);
  }

  private normalizarTexto(valor: string): string {
    return valor
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .toLowerCase();
  }
}
