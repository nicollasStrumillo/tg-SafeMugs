import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { CadastroRequest, LoginRequest, LoginResponse } from './usuario.models';

@Injectable({
	providedIn: 'root',
})
export class UsuarioApiService {
	private readonly http = inject(HttpClient);

	public login(request: LoginRequest): Observable<LoginResponse> {
		return this.http.post<LoginResponse>(`/api/usuario/login`, request);
	}

	public cadastrar(request: CadastroRequest): Observable<void> {
		return this.http.post<void>(`/api/usuario/cadastro`, request);
	}
}