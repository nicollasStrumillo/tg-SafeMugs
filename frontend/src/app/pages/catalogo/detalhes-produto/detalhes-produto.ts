import { CommonModule } from '@angular/common';
import { Component, Inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { ProdutoCardViewModel } from '../catalogo.models';


@Component({
  selector: 'sm-detalhes-produto',
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
  ],
  templateUrl: './detalhes-produto.html',
  styleUrl: './detalhes-produto.scss',
  standalone: true
})

export class DetalhesProduto {
    protected readonly comentarioRascunho = signal('');

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: { produto: ProdutoCardViewModel },
        private dialogRef: MatDialogRef<DetalhesProduto>
    ) {}

    fechar(): void {
        this.dialogRef.close();
    }

    enviarComentario(): void {
        const comentario = this.comentarioRascunho().trim();

        console.log(comentario);
    }

    adicionarAoCarrinho(): void {
        console.log('Adicionado ao carrinho');
    }

    atualizarComentario(valor: string): void {
        this.comentarioRascunho.set(valor);
    }

    formataAvaliacao(media: number | null): string {
        return media ? media.toFixed(1) : 'Novo';
    }

}

