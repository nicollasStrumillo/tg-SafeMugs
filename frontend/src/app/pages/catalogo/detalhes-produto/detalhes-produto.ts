import { CommonModule } from '@angular/common';
import { Component, inject, Inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { finalize } from 'rxjs';

import { AuthSessionService } from '../../../shared/auth/auth-session.service';
import { UsuarioLogado } from '../../../shared/auth/auth.models';
import { ComentarioProdutoDto, ComentarioRequest, ProdutoCardViewModel } from '../catalogo.models';
import { CatalogoService } from '../catalogo.service';
import { NotificationService } from '../../../shared/notification/notification.service';

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
  standalone: true,
})
export class DetalhesProduto implements OnInit {
    private readonly authSessionService = inject(AuthSessionService);
    private readonly catalogoService = inject(CatalogoService);
    private readonly notificationService = inject(NotificationService);

    protected readonly comentarioRascunho = signal('');
    protected readonly comentarios = signal<ComentarioProdutoDto[]>([]);
    protected readonly carregandoComentarios = signal(true);
    protected readonly enviandoComentario = signal(false);

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: { produto: ProdutoCardViewModel },
        private dialogRef: MatDialogRef<DetalhesProduto>
    ) {}

    ngOnInit(): void {
        this.carregarComentarios();
        console.log('Usuairo logado:', this.authSessionService.usuarioLogado());
    }

    fechar(): void {
        this.dialogRef.close();
    }

    private carregarComentarios(): void {
        this.carregandoComentarios.set(true);

        this.catalogoService.obterComentarios(this.data.produto.id)
            .pipe(finalize(() => this.carregandoComentarios.set(false)))
            .subscribe({
                next: (comentarios) => {
                    this.comentarios.set(comentarios);
                },
                error: (erro: unknown) => {
                    console.error('Erro ao carregar comentários:', erro);
                    this.comentarios.set([]);
                    this.notificationService.erro('Nao foi possivel carregar os comentarios.', {
                        icon: 'error',
                    });
                }
            });
    }

    enviarComentario(produtoId: number): void {
        const usuarioLogado: UsuarioLogado | null = this.authSessionService.usuarioLogado();
        const comentario = this.comentarioRascunho().trim();

        if (!comentario || this.enviandoComentario()) {
            return;
        }

        const comentarioRequest: ComentarioRequest = {
            usuarioId: usuarioLogado ? usuarioLogado.usuarioId : null,
            comentario,
        };

        this.enviandoComentario.set(true);

        this.catalogoService.fazerComentario(produtoId, comentarioRequest)
            .pipe(finalize(() => this.enviandoComentario.set(false)))
            .subscribe({
                next: () => {
                    this.comentarioRascunho.set('');
                    this.carregarComentarios();
                    this.notificationService.sucesso('Comentario enviado com sucesso.', {
                        icon: 'check_circle',
                    });
                },
                error: (erro: unknown) => {
                    console.error('Erro ao enviar comentário:', erro);
                    this.notificationService.erro('Erro ao enviar comentario.', {
                        icon: 'error',
                    });
                }
            });
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

