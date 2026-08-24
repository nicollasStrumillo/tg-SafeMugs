import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import {
	PedidoDto,
    PedidoResumoDto
} from './pedido.models';

@Injectable({
	providedIn: 'root',
})
export class PedidoService {
    private readonly http = inject(HttpClient);

    public criarPedido(usuarioId: number): Observable<PedidoDto> {
        return this.http.post<PedidoDto>(`/api/pedido/criar/${usuarioId}`, {});
    }

    public obterPedidosPorUsuarioId(usuarioId: number): Observable<PedidoResumoDto[]> {
        return this.http.get<PedidoResumoDto[]>(`/api/pedido/listar/${usuarioId}`);
    }

    public obterDetalhesPedido(pedidoId: number): Observable<PedidoDto> {
        return this.http.get<PedidoDto>(`/api/pedido/detalhes/${pedidoId}`);
    }

    public pesquisarPedido(numeroPedido: string): Observable<PedidoResumoDto[]> {
        return this.http.get<PedidoResumoDto[]>(`/api/pedido/pesquisar-pedido/${numeroPedido}`);
    }

    public cancelarPedido(pedidoId: number): Observable<void> {
        return this.http.delete<void>(`/api/pedido/cancelar/${pedidoId}`);
    }
}