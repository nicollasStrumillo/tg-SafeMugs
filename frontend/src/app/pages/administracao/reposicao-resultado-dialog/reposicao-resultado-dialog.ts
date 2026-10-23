import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

import { ReposicaoEstoqueResultadoDto } from '../../../services/administracao/administracao.models';

@Component({
  selector: 'sm-reposicao-resultado-dialog',
  imports: [MatButtonModule, MatDialogModule, MatIconModule],
  templateUrl: './reposicao-resultado-dialog.html',
  styleUrl: './reposicao-resultado-dialog.scss',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReposicaoResultadoDialog {
	protected readonly resultado = inject<ReposicaoEstoqueResultadoDto>(MAT_DIALOG_DATA);
}
