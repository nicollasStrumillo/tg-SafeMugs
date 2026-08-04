import { Injectable, computed, inject, signal } from '@angular/core';

import { LoginResponse, TokenPayload, UsuarioLogado } from './auth.models';
import { BrowserCookieService } from '../../services/cookies/browser-cookies.service';

const TOKEN_COOKIE = 'safemugs.token';

function base64UrlDecode(str: string): string {
	str = str.replace(/-/g, '+').replace(/_/g, '/');
	switch (str.length % 4) {
		case 0: break;
		case 2: str += '=='; break;
		case 3: str += '='; break;
	}
	return atob(str);
}

function decodeToken(token: string): TokenPayload | null {
	try {
		const payload = token.split('.')[1];
		return JSON.parse(base64UrlDecode(payload)) as TokenPayload;
	} catch {
		return null;
	}
}

function isTokenExpired(token: string): boolean {
	const payload = decodeToken(token);
	if (!payload) return true;
	const now = Math.floor(Date.now() / 1000);
	return payload.exp <= now;
}

@Injectable({
	providedIn: 'root',
})
export class AuthSessionService {
	private readonly cookieService = inject(BrowserCookieService);

	private readonly tokenSignal = signal<string | null>(null);
	private readonly usuarioLogadoSignal = signal<UsuarioLogado | null>(null);

	readonly token = this.tokenSignal.asReadonly();
	readonly usuarioLogado = this.usuarioLogadoSignal.asReadonly();
	readonly autenticado = computed(() => this.tokenSignal() !== null);

	constructor() {
		this.carregarSessaoSalva();
	}

	salvarLogin(resposta: LoginResponse): void {
		this.tokenSignal.set(resposta.token);
		this.usuarioLogadoSignal.set({
			usuarioId: resposta.usuarioId,
			nomeCompleto: resposta.nomeCompleto,
			email: resposta.email,
			urlImagemPerfil: resposta.urlImagemPerfil,
			perfil: resposta.perfil,
		});
		this.persistirToken(resposta.token);
	}

	limparSessao(): void {
		this.tokenSignal.set(null);
		this.usuarioLogadoSignal.set(null);
		this.removerTokenPersistido();
	}

	private carregarSessaoSalva(): void {
		const token = this.cookieService.get(TOKEN_COOKIE);
		if (!token || isTokenExpired(token)) {
			if (token) this.removerTokenPersistido();
			return;
		}

		const payload = decodeToken(token);
		if (!payload) {
			this.removerTokenPersistido();
			return;
		}

		this.tokenSignal.set(token);
		this.usuarioLogadoSignal.set({
			usuarioId: Number(payload.nameid),
			nomeCompleto: payload.unique_name,
			email: payload.email,
			urlImagemPerfil: payload.url_imagem_perfil,
			perfil: payload.perfil,
		});
	}

	private persistirToken(token: string): void {
		this.cookieService.set(TOKEN_COOKIE, token);
	}

	private removerTokenPersistido(): void {
		this.cookieService.remove(TOKEN_COOKIE);
	}
}
