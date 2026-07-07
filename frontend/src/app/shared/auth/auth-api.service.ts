import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { CadastroRequest, LoginRequest, LoginResponse } from './auth.models';

@Injectable({
	providedIn: 'root',
})
export class AuthApiService {
	private readonly http = inject(HttpClient);

	public login(request: LoginRequest): Observable<LoginResponse> {
		return this.http.post<LoginResponse>(`/api/auth/login`, request);
	}

	public cadastrar(request: CadastroRequest): Observable<void> {
		return this.http.post<void>(`/api/auth/cadastro`, request);
	}
}