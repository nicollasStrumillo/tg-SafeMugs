import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import {
	ReposicaoEstoqueResultadoDto,
    AvaliacaoAdminDto
} from './administracao.models';

@Injectable({
	providedIn: 'root',
})
export class AdministracaoService {
    private readonly http = inject(HttpClient);

    public obterAvaliacoes(): Observable<AvaliacaoAdminDto[]> {
        return this.http.get<AvaliacaoAdminDto[]>('/api/administracao/avaliacoes');
    }

    public excluirAvaliacao(avaliacaoId: number): Observable<void> {
        return this.http.delete<void>(`/api/administracao/avaliacoes/${avaliacaoId}`);
    }

    public resolverDesafioAcessoAdministracao(): Observable<void> {
        return this.http.post<void>('/api/administracao/desafio-acesso', null);
    }

    public importarReposicao(arquivoXml: File): Observable<ReposicaoEstoqueResultadoDto> {
        const formData = new FormData();
		formData.append('arquivoXml', arquivoXml);
        return this.http.post<ReposicaoEstoqueResultadoDto>(`/api/administracao/reposicao-estoque`, formData);
    }
}