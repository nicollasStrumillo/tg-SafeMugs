import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { CadastroRequest, LoginRequest, AuthTokenResponse, DetalhesUsuarioResponse, EditarUsuarioRequest, UploadFotoPerfilUrlRequest, MudarSenhaRequest} from './usuario.models';

@Injectable({
	providedIn: 'root',
})
export class UsuarioApiService {
	private readonly http = inject(HttpClient);

	public login(request: LoginRequest): Observable<AuthTokenResponse> {
		return this.http.post<AuthTokenResponse>(`/api/usuario/login`, request);
	}

	public cadastrar(request: CadastroRequest): Observable<void> {
		return this.http.post<void>(`/api/usuario/cadastro`, request);
	}

	public detalhes(id: number): Observable<DetalhesUsuarioResponse> {
		return this.http.get<DetalhesUsuarioResponse>(`/api/usuario/detalhes/${id}`);
	}

	// Identifica qual usuario deve ser editado pelo header de autenticação
	public editarUsuario(request: EditarUsuarioRequest): Observable<AuthTokenResponse> {
		return this.http.patch<AuthTokenResponse>(`/api/usuario/editar`, request);
	}

	public uploadFotoPerfil(foto: File): Observable<AuthTokenResponse> {
		const formData = new FormData();
		formData.append('foto', foto);
		return this.http.patch<AuthTokenResponse>(`/api/usuario/foto-perfil/upload`, formData);
	}

	public uploadFotoPerfilUrl(request: UploadFotoPerfilUrlRequest): Observable<AuthTokenResponse> {
		return this.http.patch<AuthTokenResponse>(`/api/usuario/foto-perfil/url`, request);
	}

	public mudarSenha(request: MudarSenhaRequest): Observable<void> {
		return this.http.patch<void>(`/api/usuario/mudar-senha`, request);
	}

	public desativarUsuario(usuarioId: number): Observable<void> {
		return this.http.patch<void>(`/api/usuario/desativar/${usuarioId}`, {});
	}
}