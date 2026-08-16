import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import {
	PedidoDto
} from './pedido.models';

@Injectable({
	providedIn: 'root',
})
export class PedidoService {
    private readonly http = inject(HttpClient);

    public criarPedido(usuarioId: number): Observable<PedidoDto> {
        return this.http.post<PedidoDto>(`/api/pedido/criar/${usuarioId}`, {});
    }
}