import { Injectable, computed, signal } from '@angular/core';

import { LoginResponse, UsuarioLogado } from './auth.models';

const CHAVE_STORAGE_USUARIO = 'safemugs.usuario-logado';

@Injectable({
	providedIn: 'root',
})
export class AuthSessionService {
	private readonly usuarioLogadoSignal = signal<UsuarioLogado | null>(this.carregarUsuarioSalvo());

	public readonly usuarioLogado = this.usuarioLogadoSignal.asReadonly();
	public readonly autenticado = computed(() => this.usuarioLogadoSignal() !== null);

	public salvarLogin(resposta: LoginResponse): void {
		const usuarioLogado: UsuarioLogado = {
			usuarioId: resposta.usuarioId,
			nomeCompleto: resposta.nomeCompleto,
			email: resposta.email,
			perfil: resposta.perfil,
			autenticadoEm: new Date().toISOString(),
		};

		this.usuarioLogadoSignal.set(usuarioLogado);
		this.persistirUsuario(usuarioLogado);
	}

	public limparSessao(): void {
		this.usuarioLogadoSignal.set(null);
		this.removerUsuarioPersistido();
	}

	public obterUsuarioLogado(): UsuarioLogado | null {
		return this.usuarioLogadoSignal();
	}

	private carregarUsuarioSalvo(): UsuarioLogado | null {
		if (typeof window === 'undefined') {
			return null;
		}

		const usuarioSalvo = window.localStorage.getItem(CHAVE_STORAGE_USUARIO);
		if (!usuarioSalvo) {
			return null;
		}

		try {
			return JSON.parse(usuarioSalvo) as UsuarioLogado;
		} catch {
			this.removerUsuarioPersistido();
			return null;
		}
	}

	private persistirUsuario(usuario: UsuarioLogado): void {
		if (typeof window === 'undefined') {
			return;
		}

		window.localStorage.setItem(CHAVE_STORAGE_USUARIO, JSON.stringify(usuario));
	}

	private removerUsuarioPersistido(): void {
		if (typeof window === 'undefined') {
			return;
		}

		window.localStorage.removeItem(CHAVE_STORAGE_USUARIO);
	}
}