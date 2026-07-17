import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';

import { AuthSessionService } from './auth-session.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
	const authSession = inject(AuthSessionService);
	const token = authSession.token();

	if (token) {
		req = req.clone({
			setHeaders: { Authorization: `Bearer ${token}` },
		});
	}

	return next(req);
};
