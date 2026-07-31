import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { DesafioResponse, DetalhesDesafioModel } from './score-board.models';

@Injectable({
	providedIn: 'root',
})
export class ScoreBoardService {
	private readonly http = inject(HttpClient);

	public listarDesafios(): Observable<DesafioResponse[]> {
		return this.http.get<DesafioResponse[]>(`/api/desafios/lista`);
	}

	public listarCategorias(): Observable<string[]> {
		return this.http.get<string[]>(`/api/desafios/categorias`);
	}

	public buscarDesafioPorNome(nomeDesafio: string): Observable<DesafioResponse | null> {
		return this.http.get<DesafioResponse | null>(`/api/desafios/${nomeDesafio}`);
	}

	public gerarBackupDesafios(): Observable<string | null> {
		return this.http.get('/api/desafios/backup', {responseType: 'text'}) as Observable<string>;
	}

	public restaurarDesafios(backupDesafios: string): Observable<number> {
		return this.http.post<number>(`/api/desafios/restore`, {backupDesafios});
	}

	public detalhesDesafio(id: number): Observable<DetalhesDesafioModel> {
		return this.http.get<DetalhesDesafioModel>(`/api/desafios/detalhes/${id}`);
	}

	public resolverQuizDesafio(id: number, linhasSelecionadas: number[]): Observable<{ sucesso: boolean; mensagem: string }> {
		return this.http.post<{ sucesso: boolean; mensagem: string }>(`/api/desafios/resolver-quiz/${id}`, linhasSelecionadas);
	}
}