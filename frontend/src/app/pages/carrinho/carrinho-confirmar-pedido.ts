import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

import { EnderecoDto } from '../../services/usuario/usuario.models';

export interface ConfirmarPedidoData {
	total: number;
	quantidadeItens: number;
	endereco: EnderecoDto;
}

@Component({
	selector: 'sm-carrinho-confirmar-pedido',
	standalone: true,
	imports: [CommonModule, MatButtonModule, MatDialogModule, MatIconModule],
	template: `
		<h1 class="titulo" mat-dialog-title>Confirmar pedido</h1>

		<mat-dialog-content>
			<div class="resumo-linha">
				<span class="resumo-rotulo">Total</span>
				<strong class="resumo-valor resumo-valor--total">{{ data.total | currency: 'BRL' }}</strong>
			</div>

			<div class="resumo-linha">
				<span class="resumo-rotulo">Itens</span>
				<strong class="resumo-valor">
					{{ data.quantidadeItens }} {{ data.quantidadeItens === 1 ? 'item' : 'itens' }}
				</strong>
			</div>

			<div class="endereco-bloco">
				<span class="endereco-rotulo">
					<mat-icon aria-hidden="true">location_on</mat-icon>
					Endereço de entrega
				</span>
				<span class="endereco-localidade">{{ data.endereco.cidade }} - {{ data.endereco.estado }}</span>
				<span class="endereco-logradouro">{{ data.endereco.logradouro }}, {{ data.endereco.numero }}</span>
			</div>
		</mat-dialog-content>

		<mat-dialog-actions align="end">
			<button type="button" mat-button mat-dialog-close>Cancelar</button>
			<button type="button" mat-flat-button class="confirmar-btn" [mat-dialog-close]="true">
				Confirmar pedido
			</button>
		</mat-dialog-actions>
	`,
	styles: `
		.titulo {
			font-family: 'Manrope', sans-serif;
			font-weight: 700;
		}

		.resumo-linha {
			display: flex;
			align-items: baseline;
			justify-content: space-between;
			gap: 1rem;
			padding: 0.65rem 0;
			border-bottom: 1px solid var(--sm-border);
		}

		.resumo-rotulo {
			color: var(--sm-muted);
			font-size: 0.9rem;
		}

		.resumo-valor {
			color: var(--sm-ink);
			font-weight: 700;
		}

		.resumo-valor--total {
			color: var(--sm-accent-strong);
			font-family: var(--sm-display);
			font-size: 1.25rem;
			font-weight: 800;
		}

		.endereco-bloco {
			display: grid;
			gap: 0.2rem;
			margin-top: 1rem;
			padding: 0.9rem 1rem;
			border: 1px solid var(--sm-border);
			border-radius: 0.6rem;
			background: rgba(255, 255, 255, 0.02);
		}

		.endereco-rotulo {
			display: inline-flex;
			align-items: center;
			gap: 0.45rem;
			color: var(--sm-ink);
			font-weight: 600;
			font-size: 0.95rem;
			margin-bottom: 0.25rem;
		}

		.endereco-rotulo .mat-icon {
			font-size: 20px;
			width: 20px;
			height: 20px;
			color: var(--sm-accent-strong);
		}

		.endereco-localidade {
			color: var(--sm-ink);
			font-weight: 700;
		}

		.endereco-logradouro {
			color: var(--sm-muted);
			font-size: 0.9rem;
		}

		.confirmar-btn {
			min-width: 8rem;
		}
	`,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CarrinhoConfirmarPedido {
	constructor(@Inject(MAT_DIALOG_DATA) public data: ConfirmarPedidoData) {}
}
