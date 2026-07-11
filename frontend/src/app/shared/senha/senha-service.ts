import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ResetSenhaRequest {
  email: string;
  novaSenha: string;
}

@Injectable({
  providedIn: 'root'
})
export class SenhaService {

  private readonly http = inject(HttpClient);

  resetSenha(dto: ResetSenhaRequest): Observable<void> {
    return this.http.post<void>('/api/senha/reset', dto);
  }
}
