import {
  ApplicationConfig,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection,
  provideEnvironmentInitializer,
  inject,
} from '@angular/core';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { provideRouter } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { SignalRService } from './services/signalR/signalr.service';
import { authInterceptor } from './services/usuario/auth/auth-interceptor';
import { AuthInitService } from './services/usuario/auth/auth-init.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([authInterceptor])),
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    provideNativeDateAdapter(),
    {
      provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue: { appearance: 'outline' },
    },
    provideAppInitializer(() => inject(AuthInitService).verificarSessao()),
    provideEnvironmentInitializer(() => {
      const signalRService = inject(SignalRService);
      signalRService.startSignalRConnection();
    })
  ],
};
