import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { AuthSessionService } from './auth-session.service';
import { UsuarioApiService } from '../usuario-api.service';

@Injectable({
  providedIn: 'root',
})
export class AuthInitService {
  private readonly auth = inject(AuthSessionService);
  private readonly usuarioApi = inject(UsuarioApiService);

  async verificarSessao(): Promise<void> {
    if (!this.auth.autenticado()) return;

    const usuario = this.auth.usuarioLogado();
    if (!usuario) return;

    try {
      const detalhes = await firstValueFrom(this.usuarioApi.detalhes(usuario.usuarioId));
      if (!detalhes) {
        this.auth.limparSessao();
      }
    } catch {
      this.auth.limparSessao();
    }
  }
}
