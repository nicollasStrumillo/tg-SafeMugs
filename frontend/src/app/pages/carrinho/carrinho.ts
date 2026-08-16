import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

import { CarrinhoDto, ItemCarrinhoDto } from '../../services/carrinho/carrinho.models';
import { CarrinhoService } from '../../services/carrinho/carrinho.service';
import { PedidoService } from '../../services/pedido/pedido.service';
import { EnderecoDto } from '../../services/usuario/usuario.models';
import { AuthSessionService } from '../../services/usuario/auth/auth-session.service';
import { NotificationService } from '../../shared/notification/notification.service';
import { CarrinhoConfirmarPedido } from './carrinho-confirmar-pedido';

@Component({
	selector: 'sm-carrinho',
	imports: [CommonModule, MatButtonModule, MatCardModule, MatIconModule],
	templateUrl: './carrinho.html',
	styleUrl: './carrinho.scss',
	standalone: true,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Carrinho implements OnInit {
	protected readonly carrinhoService = inject(CarrinhoService);
	protected readonly pedidoService = inject(PedidoService);
	protected readonly authSessionService = inject(AuthSessionService);
	protected readonly notificationService = inject(NotificationService);
	protected readonly router = inject(Router);
	private readonly dialog = inject(MatDialog);

	protected readonly carrinho = signal<CarrinhoDto | null>(null);
	protected readonly itensCarrinho = signal<ItemCarrinhoDto[]>([]);
	protected readonly enderecoUsuario = signal<EnderecoDto | null>(null);

	protected readonly carregando = signal(true);
	protected readonly operandoItemId = signal<number | null>(null);
	protected readonly enviandoPedido = signal(false);

	protected readonly usuarioAutenticado = computed(() => this.authSessionService.usuarioLogado());
	protected readonly carrinhoVazio = computed(() => this.itensCarrinho().length === 0);
	protected readonly temEndereco = computed(() => this.enderecoUsuario() !== null);
	protected readonly totalCarrinho = computed(() => this.carrinho()?.total ?? 0);
	protected readonly quantidadeItensCarrinho = computed(() =>
		this.itensCarrinho().reduce((total, item) => total + item.quantidade, 0),
	);
	protected readonly podeFinalizar = computed(
		() => this.temEndereco() && !this.carrinhoVazio() && !this.enviandoPedido() && !this.carregando(),
	);

	private usuarioId: number | null = null;

	ngOnInit(): void {
		const usuario = this.usuarioAutenticado();
		if (usuario === null) {
			this.router.navigate(['/catalogo']);
			return;
		}

		this.usuarioId = usuario.usuarioId;
		this.atualizarCarrinho();
	}

	protected aumentarQuantidade(item: ItemCarrinhoDto): void {
		if (this.operandoItemId() !== null || this.usuarioId === null) {
			return;
		}

		this.operandoItemId.set(item.id);
		this.carrinhoService
			.adicionarUnidadeProdutoAoCarrinho(this.usuarioId, item.produto.id, 1)
			.pipe(finalize(() => this.operandoItemId.set(null)))
			.subscribe({
				next: () => this.atualizarCarrinho(),
				error: (erro: HttpErrorResponse) => this.notificationService.notificarErroApi(erro),
			});
	}

	protected diminuirQuantidade(item: ItemCarrinhoDto): void {
		if (this.operandoItemId() !== null || this.usuarioId === null) {
			return;
		}

		this.operandoItemId.set(item.id);
		this.carrinhoService
			.removerUnidadeProdutoDoCarrinho(this.usuarioId, item.produto.id, 1)
			.pipe(finalize(() => this.operandoItemId.set(null)))
			.subscribe({
				next: () => this.atualizarCarrinho(),
				error: (erro: HttpErrorResponse) => this.notificationService.notificarErroApi(erro),
			});
	}

	protected removerItem(item: ItemCarrinhoDto): void {
		if (this.operandoItemId() !== null || this.usuarioId === null || item.quantidade <= 0) {
			return;
		}

		this.operandoItemId.set(item.id);
		this.carrinhoService
			.removerUnidadeProdutoDoCarrinho(this.usuarioId, item.produto.id, item.quantidade)
			.pipe(finalize(() => this.operandoItemId.set(null)))
			.subscribe({
				next: () => this.atualizarCarrinho(),
				error: (erro: HttpErrorResponse) => this.notificationService.notificarErroApi(erro),
			});
	}

	protected linhaOperando(item: ItemCarrinhoDto): boolean {
		return this.operandoItemId() === item.id;
	}

	protected fazerPedido(): void {
		if (!this.podeFinalizar()) {
			return;
		}

		const endereco = this.enderecoUsuario();
		if (endereco === null) {
			return;
		}

		this.dialog
			.open(CarrinhoConfirmarPedido, {
				data: {
					total: this.totalCarrinho(),
					quantidadeItens: this.quantidadeItensCarrinho(),
					endereco,
				},
				width: '440px',
				maxWidth: '95vw',
				autoFocus: false,
			})
			.afterClosed()
			.subscribe((confirmado: boolean | undefined) => {
				if (confirmado === true) {
					this.criarPedido();
				}
			});
	}

	private criarPedido(): void {
		if (this.usuarioId === null || this.enviandoPedido()) {
			return;
		}

		this.enviandoPedido.set(true);
		this.pedidoService
			.criarPedido(this.usuarioId)
			.pipe(finalize(() => this.enviandoPedido.set(false)))
			.subscribe({
				next: (pedido) => {
					this.notificationService.sucesso(`Pedido ${pedido.numeroPedido} criado com sucesso!`);
					this.router.navigate(['/catalogo']);
				},
				error: (erro: HttpErrorResponse) => this.notificationService.notificarErroApi(erro),
			});
	}

	protected trackByItemId(_: number, item: ItemCarrinhoDto): number {
		return item.id;
	}

	private atualizarCarrinho(): void {
		if (this.usuarioId === null) {
			return;
		}

		this.carregando.set(true);
		this.carrinhoService
			.obterOuCriarCarrinhoAtivo(this.usuarioId)
			.pipe(finalize(() => this.carregando.set(false)))
			.subscribe({
				next: (carrinho) => {
					this.carrinho.set(carrinho);
					this.itensCarrinho.set(carrinho.itens);
					this.enderecoUsuario.set(carrinho.usuario.endereco);
				},
				error: (erro: HttpErrorResponse) => {
					this.notificationService.notificarErroApi(erro);
					this.router.navigate(['/catalogo']);
				},
			});
	}
}