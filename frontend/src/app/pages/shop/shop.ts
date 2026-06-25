import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BuscaService } from '../../services/buscaservice';
import { HttpClient } from '@angular/common/http';
import { MatGridListModule} from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'sm-vitrine',
  imports: [CommonModule, RouterModule, MatGridListModule,MatCardModule],
  templateUrl: './shop.html',
  styleUrl: './shop.scss',
  standalone: true
})
export class Shop implements OnInit {
  private http = inject(HttpClient);
  public buscaService = inject(BuscaService);

  todosProdutos = signal<any[]>([]);

  produtosFiltrados = computed(() => {
    const termo = this.buscaService.termoBusca().toLowerCase();

    return this.todosProdutos().filter(p => {
      const bateNome = p.nome.toLowerCase().includes(termo);
      return bateNome;
    });
  });

  ngOnInit() {
    this.http.get<any[]>('http://localhost:5242/api/produtos/lista').subscribe({
      next: (res) => this.todosProdutos.set(res),
      error: (err) => console.error('Erro ao buscar produtos:', err)
    });
  }
  //imagens temporarias
    private mapaImagens: Record<string, string> = {
    'Café': 'https://images.unsplash.com/photo-1442512595331-e89e73853f31?w=500',
    'Cappuccino': 'https://images.unsplash.com/photo-1509785307050-d4066910ec1e?w=500',
    'Latte': 'https://images.unsplash.com/photo-1498804103079-a6351b050096?w=500',
    'Chá Verde': 'https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?w=500',
    'Chá Preto': 'https://images.unsplash.com/photo-1509042239860-f550ce710b93?w=500',
    'Chá de Camomila': 'https://images.unsplash.com/photo-1511920170033-f8396924c348?w=500'
  };
  obterImagem(nome: string): string {
    return this.mapaImagens[nome] || 'https://via.placeholder.com/500?text=MugNãoEncontrada';
  }
  atualizarBusca(event: Event) {
    const valor = (event.target as HTMLInputElement).value; 
    this.buscaService.termoBusca.set(valor);
  }
}
