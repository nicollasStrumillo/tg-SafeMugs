import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';

@Component({
	selector: 'sm-perfil-confirmar-exclusao',
	standalone: true,
	imports: [MatButtonModule, MatDialogModule],
	template: `
		<h2 mat-dialog-title>Deletar conta</h2>
		<mat-dialog-content>
			<p>
				Tem certeza que deseja deletar sua conta? Você será desconectado e não poderá mais acessar a loja com este usuário.
			</p>
		</mat-dialog-content>
		<mat-dialog-actions align="end">
			<button type="button" mat-button mat-dialog-close>Cancelar</button>
			<button type="button" mat-flat-button class="confirm-delete" [mat-dialog-close]="true">
				Confirmar exclusão
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

		.confirm-delete {
			background: #ef5350;
			color: #fff;
		}
	`,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PerfilConfirmarExclusao {}
