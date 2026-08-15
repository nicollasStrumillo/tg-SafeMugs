import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import {
	CarrinhoDto
} from './carrinho.models';

@Injectable({
	providedIn: 'root',
})
export class CarrinhoService {
    private readonly http = inject(HttpClient);

    public obterOuCriarCarrinhoAtivo(usuarioId: number): Observable<CarrinhoDto> {
        return this.http.get<CarrinhoDto>(`/api/carrinho/${usuarioId}`);
    }

    public adicionarUnidadeProdutoAoCarrinho(usuarioId: number, produtoId: number, quantidade: number): Observable<void> {
        const requestBody = { usuarioId, produtoId, quantidade };
        return this.http.patch<void>(`/api/carrinho/adicionar`, requestBody);
    }

    public removerUnidadeProdutoDoCarrinho(usuarioId: number, produtoId: number, quantidade: number): Observable<void> {
        const requestBody = { usuarioId, produtoId, quantidade };
        return this.http.patch<void>(`/api/carrinho/remover`, requestBody);
    }
}