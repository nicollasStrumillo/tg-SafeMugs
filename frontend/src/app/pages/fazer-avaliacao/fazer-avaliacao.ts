import { ChangeDetectionStrategy, Component, Inject, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { finalize } from 'rxjs';

import { NotificationService } from '../../shared/notification/notification.service';
import { ProdutoService } from '../../services/produto/produto.service';
import { AuthSessionService } from '../../services/usuario/auth/auth-session.service';

interface produtoData {
	idProduto: number;
	nomeProduto: string;
	descricaoProduto: string;
	categoriaProduto: string;
	urlImagemProduto: string;
}

@Component({
	selector: 'sm-fazer-avaliacao',
	imports: [
		CommonModule,
		FormsModule,
		MatButtonModule,
		MatCardModule,
		MatFormFieldModule,
		MatIconModule,
		MatInputModule,
	],
	templateUrl: './fazer-avaliacao.html',
	styleUrl: './fazer-avaliacao.scss',
	standalone: true,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FazerAvaliacao implements OnInit {
	private readonly notificationService = inject(NotificationService);
	private readonly produtoService = inject(ProdutoService);
	private readonly auth = inject(AuthSessionService);

	protected readonly nota = signal(0.5);
	protected readonly notaHover = signal<number | null>(null);
	protected readonly comentario = signal('');
	protected readonly enviando = signal(false);
	protected readonly usuarioId = signal<number | null>(null);
	protected readonly emailUsuario = signal('');

	protected readonly podeEnviar = computed(
		() => this.comentario().trim().length > 0 && !this.enviando() && this.usuarioId() !== null,
	);

	constructor(
		@Inject(MAT_DIALOG_DATA) public data: { produto: produtoData },
		private readonly dialogRef: MatDialogRef<FazerAvaliacao>,
	) {}

	ngOnInit(): void {
		if (!this.auth.autenticado()) {
			this.dialogRef.close();
			return;
		}

		const usuarioLogado = this.auth.usuarioLogado();
		if (usuarioLogado) {
			this.usuarioId.set(usuarioLogado.usuarioId);
			this.emailUsuario.set(usuarioLogado.email);
		}
	}

	fechar(): void {
		this.dialogRef.close();
	}

	protected estrelaIcone(indice: number): string {
		const notaExibida = this.notaHover() ?? this.nota();
		if (notaExibida >= indice) return 'star';
		if (notaExibida >= indice - 0.5) return 'star_half';
		return 'star_border';
	}

	protected hoverEstrela(indice: number, metade: 'esquerda' | 'direita'): void {
		this.notaHover.set(metade === 'esquerda' ? indice - 0.5 : indice);
	}

	protected sairHover(): void {
		this.notaHover.set(null);
	}

	protected selecionarEstrela(indice: number, metade: 'esquerda' | 'direita'): void {
		this.nota.set(metade === 'esquerda' ? indice - 0.5 : indice);
	}

	protected atualizarComentario(event: Event): void {
		this.comentario.set((event.target as HTMLTextAreaElement).value);
	}

	protected enviar(): void {
		const usuarioId = this.usuarioId();
		if (usuarioId === null) {
			this.notificationService.erro('Usuário não autenticado.');
			return;
		}

		const comentario = this.comentario().trim();
		if (!comentario || this.enviando()) return;

		this.enviando.set(true);

		this.produtoService
			.fazerAvaliacao(this.data.produto.idProduto, usuarioId, this.nota(), comentario)
			.pipe(finalize(() => this.enviando.set(false)))
			.subscribe({
				next: () => {
					this.notificationService.sucesso('Avaliação enviada com sucesso!', { icon: 'check_circle' });
					this.dialogRef.close(true);
				},
				error: () => {
					this.notificationService.erro('Erro ao enviar avaliação.', { icon: 'error' });
				},
			});
	}
}