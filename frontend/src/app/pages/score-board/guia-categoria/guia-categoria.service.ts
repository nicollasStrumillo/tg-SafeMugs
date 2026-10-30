import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Marked } from 'marked';
import { markedHighlight } from 'marked-highlight';
import hljs from 'highlight.js/lib/core';
import csharp from 'highlight.js/lib/languages/csharp';
import typescript from 'highlight.js/lib/languages/typescript';
import xml from 'highlight.js/lib/languages/xml';
import sql from 'highlight.js/lib/languages/sql';

const GUIAS_CATEGORIAS: GuiaCategoria[] = [
	{ categoriaId: 'SqlInjection', caminhoMarkdown: '/guias-categoria/sqlinjection.md' },
	{ categoriaId: 'IDOR', caminhoMarkdown: '/guias-categoria/idor.md'},
	{ categoriaId: 'ReflectedXSS', caminhoMarkdown: '/guias-categoria/reflectedxss.md'},
	{ categoriaId: 'StoredXSS', caminhoMarkdown: '/guias-categoria/storedxss.md'},
	{ categoriaId: 'DomXSS', caminhoMarkdown: '/guias-categoria/domxss.md'}
];

export interface GuiaCategoria {
	categoriaId: string;
	caminhoMarkdown: string;
}

const marked = new Marked(
	markedHighlight({
		emptyLangClass: 'hljs',
		langPrefix: 'hljs language-',
		highlight(code, lang) {
			if (lang && hljs.getLanguage(lang)) {
				return hljs.highlight(code, { language: lang }).value;
			}
			return code;
		},
	}),
);

hljs.registerLanguage('csharp', csharp);
hljs.registerLanguage('typescript', typescript);
hljs.registerLanguage('xml', xml);
hljs.registerLanguage('sql', sql);

@Injectable({
	providedIn: 'root',
})

export class GuiaCategoriaService {
	private readonly http = inject(HttpClient);

	public parsearMarkdown(markdown: string): string {
		return marked.parse(markdown) as string;
	}

	public obterGuia(categoriaId: string): GuiaCategoria | null {
		return GUIAS_CATEGORIAS.find((guia) => guia.categoriaId === categoriaId) ?? null;
	}

	public obterConteudoGuia(guia: GuiaCategoria): Observable<string> {
		return this.http.get(guia.caminhoMarkdown, { responseType: 'text' });
	}
}


