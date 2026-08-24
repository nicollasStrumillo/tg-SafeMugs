import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject, Inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { finalize } from 'rxjs';

import { PedidoService } from '../../../services/pedido/pedido.service';
import { PedidoDto, PedidoResumoDto } from '../../../services/pedido/pedido.models';
import { NotificationService } from '../../../shared/notification/notification.service';
import { PedidosConfirmarCancelamento } from '../pedidos-confirmar-cancelamento';

@Component({
	selector: 'sm-detalhes-pedido',
	imports: [CommonModule, MatButtonModule, MatCardModule, MatIconModule],
	templateUrl: './detalhes-pedido.html',
	styleUrl: './detalhes-pedido.scss',
	standalone: true,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DetalhesPedido implements OnInit {
	private readonly pedidoService = inject(PedidoService);
	private readonly notificationService = inject(NotificationService);
	private readonly dialog = inject(MatDialog);

	protected readonly pedido = signal<PedidoDto | null>(null);
	protected readonly carregando = signal(true);
	protected readonly erroCarregamento = signal<string | null>(null);
	protected readonly cancelando = signal(false);

	constructor(
		@Inject(MAT_DIALOG_DATA) public data: { pedido: PedidoResumoDto },
		private readonly dialogRef: MatDialogRef<DetalhesPedido>,
	) {}

	ngOnInit(): void {
		this.carregarDetalhes();
	}

	protected fechar(): void {
		this.dialogRef.close();
	}

	protected trackByItemId(_: number, itemId: number): number {
		return itemId;
	}

	protected cancelarPedido(): void {
		if (this.cancelando()) {
			return;
		}

		this.dialog
			.open(PedidosConfirmarCancelamento, {
				width: '520px',
				data: { numeroPedido: this.data.pedido.numeroPedido },
				autoFocus: false,
			})
			.afterClosed()
			.subscribe((confirmado: boolean | undefined) => {
				if (!confirmado) {
					return;
				}

				this.cancelando.set(true);

				this.pedidoService
					.cancelarPedido(this.data.pedido.id)
					.pipe(finalize(() => this.cancelando.set(false)))
					.subscribe({
						next: () => {
							this.notificationService.sucesso(`Pedido ${this.data.pedido.numeroPedido} cancelado.`);
							this.dialogRef.close({ cancelado: true });
						},
						error: (erro: HttpErrorResponse) => this.notificationService.notificarErroApi(erro),
					});
			});
	}

	private carregarDetalhes(): void {
		this.carregando.set(true);

		this.pedidoService
			.obterDetalhesPedido(this.data.pedido.id)
			.pipe(finalize(() => this.carregando.set(false)))
			.subscribe({
				next: (detalhes) => {
					this.pedido.set(detalhes);
					this.erroCarregamento.set(null);
				},
				error: (erro: HttpErrorResponse) => {
					this.erroCarregamento.set('Não foi possível carregar os detalhes do pedido.');
					this.notificationService.notificarErroApi(erro);
				},
			});
	}
}
