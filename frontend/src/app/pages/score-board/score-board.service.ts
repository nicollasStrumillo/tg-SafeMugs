import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { DesafioResponse } from './score-board.models';

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
}