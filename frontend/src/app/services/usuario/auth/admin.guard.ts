import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';

import { NotificationService } from '../../../shared/notification/notification.service';
import { AuthSessionService } from './auth-session.service';

export const adminGuard: CanMatchFn = () => {
	const auth = inject(AuthSessionService);
	const router = inject(Router);
	const notification = inject(NotificationService);

	if (auth.isAdmin()) {
		return true;
	}

	if (auth.autenticado()) {
		notification.erro('Acesso restrito a administradores.');
		return router.createUrlTree(['/catalogo']);
	}

	notification.erro('Entre com uma conta administradora para acessar esta página.');
	return router.createUrlTree(['/login']);
};
