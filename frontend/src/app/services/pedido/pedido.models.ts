import { DetalhesUsuarioResponse, EnderecoDto } from '../usuario/usuario.models';
import { CarrinhoDto } from '../carrinho/carrinho.models';

export interface PedidoResumoDto {
    id: number;
    numeroPedido: string;
    valorTotal: number;
    quantidadeItens: number;
    dataPedido: Date;
    usuarioId: number;
}

export interface PedidoDto {
    id: number;
    numeroPedido: string;
    valorTotal: number;
    quantidadeItens: number;
    dataPedido: Date;

    usuario: DetalhesUsuarioResponse;
    endereco: EnderecoDto;
    carrinho: CarrinhoDto;
}