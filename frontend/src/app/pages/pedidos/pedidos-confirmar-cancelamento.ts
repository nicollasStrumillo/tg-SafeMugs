import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';

@Component({
	selector: 'sm-pedidos-confirmar-cancelamento',
	standalone: true,
	imports: [MatButtonModule, MatDialogModule],
	template: `
		<h2 mat-dialog-title>Cancelar pedido</h2>
		<mat-dialog-content>
			<p>
				Tem certeza que deseja cancelar o pedido {{ dados.numeroPedido }}? Esta ação não pode ser desfeita.
			</p>
		</mat-dialog-content>
		<mat-dialog-actions align="end">
			<button type="button" mat-button mat-dialog-close>Voltar</button>
			<button type="button" mat-flat-button class="confirmar-cancelamento" [mat-dialog-close]="true">
				Confirmar cancelamento
			</button>
		</mat-dialog-actions>
	`,
	styles: `
		p {
			margin: 0;
			color: var(--sm-ink);
			line-height: 1.6;
			max-width: 36rem;
		}

		.confirmar-cancelamento {
			background: #ef5350;
			color: #fff;
		}
	`,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PedidosConfirmarCancelamento {
	protected readonly dados = inject<{ numeroPedido: string }>(MAT_DIALOG_DATA);
}
