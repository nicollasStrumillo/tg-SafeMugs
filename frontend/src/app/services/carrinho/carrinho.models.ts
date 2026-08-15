import { ProdutoResumoDto} from '../produto/produto.models';
import { DetalhesUsuarioResponse } from '../usuario/usuario.models';

export interface ItemCarrinhoDto {
    id: number;
    produto: ProdutoResumoDto;
    quantidade: number;
    precoUnitario: number;
    precoTotal: number;
}

export interface CarrinhoDto {
    id: number;
    status: string;
    total: number;
    
    usuario: DetalhesUsuarioResponse;

    itens: ItemCarrinhoDto[];
}