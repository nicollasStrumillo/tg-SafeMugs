export interface AvaliacaoAdminDto {
    id: number;
    nota: number;
    comentario: string;
    dtCadastro: string;

    usuarioId: number;
    usuarioNome: string;
    usuarioEmail: string;
    usuarioUrlImagemPerfil: string | null;
    usuarioAtivo: boolean;

    produtoId: number;
    produtoNome: string;
}

export interface ReposicaoEstoqueResultadoDto {
    sucesso: boolean;
    erro: string | null;

    totalLinhas: number;
    linhasProcessadas: number;

    linhas: ReposicaoEstoqueLinhaDto[];
}

export interface ReposicaoEstoqueLinhaDto {
    sucesso: boolean;

    idRecebido: string;
    quantidadeRecebida: string;

    produtoId: number | null;
    produtoNome: string | null;
    quantidade: number | null;

    estoqueAnterior: number | null;
    estoqueNovo: number | null;

    erro: string | null;
}