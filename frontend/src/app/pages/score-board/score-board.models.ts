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
	resolvido: boolean;
	dicasDesafio: DicaDesafioDto[];

	possuiQuiz: boolean;
	quizResolvido: boolean;

	// para os casos que um desafio foi resolvido através da funcionalidade de restauração da ultima sessão
	isRestored: boolean | null;
}

export interface QuizDesafio{
	nomeDesafio: string;
	linguagem: string;
	resolvido: boolean;

	//Quiz
	linhasQuiz: string[];
	linhasCorretas: number[]; // só chega preenchido quando resolvido == true, se não chega vazio

	//Codigo Seguro
	linhasCodigoSeguro: string[];
	mensagemSeguro: string;
}

export interface DetalhesDesafioModel{
	id: number;

	nome: string;
	descricao: string;
	descricaoDetalhes: string;

	categoria: string;
	descricaoCategoria: string;

	dificuldade: number;
	resolvido: boolean;

	possuiQuiz: boolean;
	quizResolvido: boolean;

	dicasDesafio: DicaDesafioDto[];

	quizDesafio : QuizDesafio | null;
}