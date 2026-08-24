import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerInputEvent, MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';

import { PedidoService } from '../../services/pedido/pedido.service';
import { PedidoResumoDto } from '../../services/pedido/pedido.models';
import { AuthSessionService } from '../../services/usuario/auth/auth-session.service';
import { NotificationService } from '../../shared/notification/notification.service';
import { PedidosConfirmarCancelamento } from './pedidos-confirmar-cancelamento';
import { DetalhesPedido } from './detalhes-pedido/detalhes-pedido';

@Component({
	selector: 'sm-pedidos',
	imports: [
		CommonModule,
		MatButtonModule,
		MatCardModule,
		MatDatepickerModule,
		MatFormFieldModule,
		MatIconModule,
		MatInputModule,
	],
	templateUrl: './pedidos.html',
	styleUrl: './pedidos.scss',
	standalone: true,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Pedidos implements OnInit {
	private readonly pedidoService = inject(PedidoService);
	private readonly notificationService = inject(NotificationService);
	private readonly auth = inject(AuthSessionService);
	private readonly router = inject(Router);
	private readonly dialog = inject(MatDialog);

	protected readonly usuarioId = signal<number>(0);

	protected readonly pedidosBase = signal<PedidoResumoDto[]>([]);
	protected readonly carregando = signal(true);
	protected readonly erroCarregamento = signal<string | null>(null);
	protected readonly cancelandoPedidoId = signal<number | null>(null);

	protected readonly termoNumeroPedido = signal('');
	protected readonly dataFiltro = signal<Date | null>(null);

	protected readonly pedidosFiltrados = computed(() => {
		const data = this.dataFiltro();

		if (data === null) return this.pedidosBase();

		return this.pedidosBase().filter((pedido) => this.mesmoDia(this.comoData(pedido.dataPedido), data));
	});

	ngOnInit(): void {
		if (!this.auth.autenticado()) this.router.navigate(['/catalogo']);

		this.usuarioId.set(this.auth.usuarioLogado()!.usuarioId);

		this.carregarPedidos();
	}

	protected atualizarBuscaNumero(event: Event): void {
		const valor = (event.target as HTMLInputElement).value;
		this.termoNumeroPedido.set(valor);
	}

	protected buscarPorNumero(): void {
		const termo = this.termoNumeroPedido();

		if (termo.trim() === '') {
			this.carregarPedidos();
			return;
		}

		this.carregando.set(true);
		this.pedidoService.pesquisarPedido(termo).subscribe({
			next: (pedidos) => {
				this.pedidosBase.set(pedidos);
				this.erroCarregamento.set(null);
				this.carregando.set(false);
			},
			error: (erro: HttpErrorResponse) => {
				this.carregando.set(false);
				this.notificationService.notificarErroApi(erro);
			},
		});
	}

	protected atualizarFiltroData(event: MatDatepickerInputEvent<Date | null>): void {
		this.dataFiltro.set(event.value);
	}

	protected limparTermoBusca(): void {
		this.termoNumeroPedido.set('');
		this.carregarPedidos();
	}

	protected abrirPedido(pedido: PedidoResumoDto): void {
		this.dialog
			.open(DetalhesPedido, {
				data: { pedido },
				width: '760px',
				maxWidth: '95vw',
				maxHeight: '90vh',
				autoFocus: false,
			})
			.afterClosed()
			.subscribe((resultado: { cancelado?: boolean } | undefined) => {
				if (resultado?.cancelado === true) {
					this.removerPedidoCancelado(pedido);
				}
			});
	}

	protected cancelarPedido(pedido: PedidoResumoDto, event: Event): void {
		event.stopPropagation();

		if (this.cancelandoPedidoId() !== null) {
			return;
		}

		this.dialog
			.open(PedidosConfirmarCancelamento, {
				width: '520px',
				data: { numeroPedido: pedido.numeroPedido },
				autoFocus: false
			})
			.afterClosed()
			.subscribe((confirmado: boolean | undefined) => {
				if (!confirmado) {
					return;
				}

				this.cancelandoPedidoId.set(pedido.id);

				this.pedidoService.cancelarPedido(pedido.id).subscribe({
					next: () => {
						this.removerPedidoCancelado(pedido);
						this.cancelandoPedidoId.set(null);
						this.notificationService.sucesso(`Pedido ${pedido.numeroPedido} cancelado.`);
					},
					error: (erro: HttpErrorResponse) => {
						this.cancelandoPedidoId.set(null);
						this.notificationService.notificarErroApi(erro);
					},
				});
			});
	}

	protected trackById(_: number, pedido: PedidoResumoDto): number {
		return pedido.id;
	}

	private removerPedidoCancelado(pedido: PedidoResumoDto): void {
		this.pedidosBase.update((pedidos) => pedidos.filter((p) => p.id !== pedido.id));
	}

	private carregarPedidos(): void {
		this.carregando.set(true);

		this.pedidoService.obterPedidosPorUsuarioId(this.usuarioId()).subscribe({
			next: (pedidos) => {
				this.pedidosBase.set(pedidos);
				this.erroCarregamento.set(null);
				this.carregando.set(false);
			},
			error: (erro: HttpErrorResponse) => {
				this.erroCarregamento.set('Não foi possível carregar seus pedidos no momento.');
				this.carregando.set(false);
				this.notificationService.notificarErroApi(erro);
			},
		});
	}

	private comoData(valor: PedidoResumoDto['dataPedido']): Date {
		return valor instanceof Date ? valor : new Date(valor);
	}

	private mesmoDia(a: Date, b: Date): boolean {
		return (
			a.getFullYear() === b.getFullYear() && a.getMonth() === b.getMonth() && a.getDate() === b.getDate()
		);
	}
}
