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

// Parte da vulnerabilidade de Stored XSS
import {SignalRService} from '../../../services/signalR/signalr.service';
import { DomSanitizer, type SafeHtml } from '@angular/platform-browser'

const STOREDXSS_PAYLOAD = '<iframe src="javascript:alert(`XSS`)">';

interface ComentarioProdutoViewModel extends ComentarioProdutoDto {
  comentarioInseguro: SafeHtml;
}

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

    // Parte da vulnerabilidade de Stored XSS
    private readonly signalRService = inject(SignalRService);
    private readonly sanitizer = inject(DomSanitizer)

    protected readonly comentarioRascunho = signal('');
    protected readonly comentarios = signal<ComentarioProdutoViewModel[]>([]);
    protected readonly carregandoComentarios = signal(true);
    protected readonly enviandoComentario = signal(false);
    protected readonly enviandoEdicao = signal(false);

    protected readonly comentarioEditandoId = signal<number | null>(null);
    protected readonly comentarioEditandoTexto = signal('');

    protected usuarioLogado: UsuarioLogado | null = null;

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: { produto: ProdutoCardViewModel },
        private dialogRef: MatDialogRef<DetalhesProduto>
    ) {}

    ngOnInit(): void {
        this.carregarComentarios();
        this.usuarioLogado = this.authSessionService.usuarioLogado();
        console.log('Usuairo logado:', this.usuarioLogado);
    }

    fechar(): void {
        this.dialogRef.close();
    }

    private async carregarComentarios(): Promise<void> {
        this.carregandoComentarios.set(true);

        await this.catalogoService.obterComentarios(this.data.produto.id)
            .pipe(finalize(() => this.carregandoComentarios.set(false)))
            .subscribe({
                next: async (comentarios) => {
                    // Parte da vulnerabilidade de Stored XSS
                    const comenatariosVuln : ComentarioProdutoViewModel[] = comentarios.map(c => ({...c, comentarioInseguro: this.sanitizer.bypassSecurityTrustHtml(c.comentario.trim())}));
                    
                    await this.VerificarPayloadXSS(comenatariosVuln);
                    this.comentarios.set(comenatariosVuln);
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

    // Parte da vulnerabilidade de Stored XSS
    private async VerificarPayloadXSS(comentarios: ComentarioProdutoViewModel[]): Promise<void> {
        for (const comentario of comentarios) {
            if (comentario.comentario === STOREDXSS_PAYLOAD) {
                await this.signalRService.SolveDesafioStoredXss(STOREDXSS_PAYLOAD);
                break;
            }
        }
    }

    enviarComentario(produtoId: number): void {
        const usuarioLogado: UsuarioLogado | null = this.authSessionService.usuarioLogado();
        const comentario = this.comentarioRascunho().trim();

        if (!comentario || this.enviandoComentario()) {
            return;
        }

        const comentarioRequest: ComentarioRequest = {
            nomeCompleto: usuarioLogado ? usuarioLogado.nomeCompleto : null,
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

    iniciarEdicao(comentario: ComentarioProdutoViewModel): void {
        this.comentarioEditandoId.set(comentario.id);
        this.comentarioEditandoTexto.set(comentario.comentario);
    }

    cancelarEdicao(): void {
        this.comentarioEditandoId.set(null);
        this.comentarioEditandoTexto.set('');
    }

    salvarEdicao(produtoId: number, comentarioId: number): void {
        const texto = this.comentarioEditandoTexto().trim();
        if (!texto || this.enviandoEdicao()) {
            return;
        }

        this.enviandoEdicao.set(true);

        this.catalogoService.atualizarComentario(comentarioId, texto)
            .pipe(finalize(() => this.enviandoEdicao.set(false)))
            .subscribe({
                next: () => {
                    this.cancelarEdicao();
                    this.carregarComentarios();
                    this.notificationService.sucesso('Comentario atualizado.', {
                        icon: 'check_circle',
                    });
                },
                error: (erro: unknown) => {
                    console.error('Erro ao atualizar comentário:', erro);
                    this.notificationService.erro('Erro ao atualizar comentario.', {
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

