import { HttpErrorResponse } from '@angular/common/http';

function extrairMensagemDeObjeto(valor: unknown): string | null {
	if (!valor || typeof valor !== 'object') {
		return null;
	}

	const erro = valor as {
		message?: unknown;
		details?: unknown;
		error?: unknown;
	};

	if (typeof erro.message === 'string' && erro.message.trim()) {
		return erro.message;
	}

	if (typeof erro.details === 'string' && erro.details.trim()) {
		return erro.details;
	}

	if (typeof erro.error === 'string' && erro.error.trim()) {
		return erro.error;
	}

	return null;
}

export function obterMensagemErroAuth(error: unknown, fallback: string): string {
	if (error instanceof HttpErrorResponse) {
		if (typeof error.error === 'string' && error.error.trim()) {
			return error.error;
		}

		const mensagemDoPayload = extrairMensagemDeObjeto(error.error);
		if (mensagemDoPayload) {
			return mensagemDoPayload;
		}

		const mensagemDoErro = extrairMensagemDeObjeto(error);
		if (mensagemDoErro) {
			return mensagemDoErro;
		}
	}

	if (error instanceof Error && error.message.trim()) {
		return error.message;
	}

	return fallback;
}