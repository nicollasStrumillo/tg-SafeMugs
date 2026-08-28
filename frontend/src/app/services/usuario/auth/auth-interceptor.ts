import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';

import { AuthSessionService } from './auth-session.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
	const authSession = inject(AuthSessionService);
	const token = authSession.token();

	if (token) {
		req = req.clone({
			setHeaders: { Authorization: `Bearer ${token}` },
		});
	}

	return next(req).pipe(
		catchError((error: HttpErrorResponse) => {
			if (authSession.autenticado()) {
				const isDetalhes = req.url.includes('/api/usuario/detalhes');
				if (error.status === 401 || (error.status === 404 && isDetalhes)) {
					authSession.limparSessao();
				}
			}
			return throwError(() => error);
		}),
	);
};
