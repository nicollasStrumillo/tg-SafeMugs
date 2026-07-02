import { CommonModule } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatDialog } from '@angular/material/dialog';

import { CatalogoService } from './catalogo.service';
import { ProdutoCardViewModel } from './catalogo.models';
import { DetalhesProduto } from './detalhes-produto/detalhes-produto';

@Component({
  selector: 'sm-catalogo',
  imports: [
    CommonModule,
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
  private readonly catalogoService = inject(CatalogoService);

  readonly carregando = signal(true);
  readonly erroCarregamento = signal<string | null>(null);
  readonly todosProdutos = signal<ProdutoCardViewModel[]>([]);

  readonly termoBusca = signal<string>('');

  readonly produtosFiltrados = computed(() => {
    const termo = this.normalizarTexto(this.termoBusca());

    if (!termo) {
      return this.todosProdutos();
    }

    return this.todosProdutos().filter((produto) => {
      const camposBusca = [produto.nome, produto.descricao, produto.categoria].join(' ');
      return this.normalizarTexto(camposBusca).includes(termo);
    });
  });

  constructor(private readonly dialog: MatDialog) {}

  ngOnInit() {
    this.catalogoService.listarProdutos().subscribe({
      next: (produtos) => {
        this.todosProdutos.set(produtos);
        this.erroCarregamento.set(null);
        this.carregando.set(false);
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

  trackById(_: number, produto: ProdutoCardViewModel): number {
    return produto.id;
  }

  limparBusca(): void {
    this.termoBusca.set('');
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
      maxHeight: '90vh'
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
