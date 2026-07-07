export interface DicaDesafioDto {
	id: number;
	nrDica: number;
	texto: string;
}

export interface DesafioResponse {
	id: number;
	nome: string;
	descricao: string;
	categoria: string;
	dificuldade: number;
	urlMitigacao: string;
	dicasDesafio: DicaDesafioDto[];
}