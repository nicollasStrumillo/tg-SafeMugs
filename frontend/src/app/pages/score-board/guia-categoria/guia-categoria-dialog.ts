import { ChangeDetectionStrategy, Component, Inject, OnInit, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { GuiaCategoria, GuiaCategoriaService } from './guia-categoria.service';

export interface GuiaCategoriaDialogData {
	titulo: string;
	guia: GuiaCategoria;
}

@Component({
	selector: 'sm-guia-categoria-dialog',
	imports: [MatButtonModule, MatIconModule],
	templateUrl: './guia-categoria-dialog.html',
	styleUrl: './guia-categoria-dialog.scss',
	standalone: true,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GuiaCategoriaDialog implements OnInit {
	private readonly guiaCategoriaService = inject(GuiaCategoriaService);

	protected readonly carregando = signal(true);
	protected readonly erro = signal<string | null>(null);
	protected readonly conteudoHtml = signal('');

	constructor(
		@Inject(MAT_DIALOG_DATA) public data: GuiaCategoriaDialogData,
		private readonly dialogRef: MatDialogRef<GuiaCategoriaDialog>,
	) {}

	ngOnInit(): void {
		this.guiaCategoriaService.obterConteudoGuia(this.data.guia).subscribe({
			next: (markdown) => {
				this.conteudoHtml.set(this.guiaCategoriaService.parsearMarkdown(markdown));
				this.carregando.set(false);
			},
			error: (erro) => {
				console.error('Erro ao carregar guia de categoria:', erro);
				this.erro.set('Não foi possível carregar o guia desta categoria.');
				this.carregando.set(false);
			},
		});
	}

	protected fechar(): void {
		this.dialogRef.close();
	}
}

