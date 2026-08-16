import { DetalhesUsuarioResponse, EnderecoDto } from '../usuario/usuario.models';
import { CarrinhoDto } from '../carrinho/carrinho.models';

export interface PedidoDto {
    id: number;
    numeroPedido: string;
    valorTotal: number;
    quantidadeItens: number;

    usuario: DetalhesUsuarioResponse;
    endereco: EnderecoDto;
    carrinho: CarrinhoDto;
}