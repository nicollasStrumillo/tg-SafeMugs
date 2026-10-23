import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

import hljs from 'highlight.js/lib/core';
import xml from 'highlight.js/lib/languages/xml';

const XML_EXEMPLO = `<?xml version="1.0" encoding="utf-8"?>
<reposicao>
  <produto>
    <id>1</id>
    <quantidade>5</quantidade>
  </produto>
</reposicao>`;

hljs.registerLanguage('xml', xml);

@Component({
	selector: 'sm-reposicao-ajuda-dialog',
	imports: [MatButtonModule, MatDialogModule, MatIconModule],
	templateUrl: './reposicao-ajuda-dialog.html',
	styleUrl: './reposicao-ajuda-dialog.scss',
	standalone: true,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReposicaoAjudaDialog {
	protected readonly linhasExemplo = XML_EXEMPLO.split('\n');

	protected highlightCodigo(linha: string): string {
		try {
			return hljs.highlight(linha || ' ', { language: 'xml' }).value;
		} catch {
			return linha;
		}
	}
}
