import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { finalize } from 'rxjs';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';

import { AuthApiService } from '../../shared/auth/auth-api.service';
import { AuthSessionService } from '../../shared/auth/auth-session.service';
import { ApiError } from '../../shared/notification/erro-api.model';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'sm-login',
  standalone: true,
  imports: [
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    ReactiveFormsModule,
    RouterLink,
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginPage {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly authApiService = inject(AuthApiService);
  private readonly authSessionService = inject(AuthSessionService);
  private readonly router = inject(Router);

  protected readonly formSubmitted = signal(false);
  protected readonly passwordHidden = signal(true);
  protected readonly carregando = signal(false);
  protected readonly mensagemErro = signal<string | null>(null);
  protected readonly passwordInputType = computed(() =>
    this.passwordHidden() ? 'password' : 'text',
  );

  protected readonly loginForm = this.formBuilder.group({
    email: ['', [Validators.required]],
    password: ['', [Validators.required]],
  });

  protected submitLogin(): void {
    this.formSubmitted.set(true);
    this.loginForm.markAllAsTouched();

    if (this.loginForm.invalid) {
      return;
    }

    this.mensagemErro.set(null);
    this.carregando.set(true);

    const { email, password } = this.loginForm.getRawValue();

    this.authApiService
      .login({ email, senha: password })
      .pipe(finalize(() => this.carregando.set(false)))
      .subscribe({
        next: (usuario) => {
          this.authSessionService.salvarLogin(usuario);
          void this.router.navigate(['/catalogo']);
        },
        error: (erro: HttpErrorResponse) => {
          const apiError = erro.error as ApiError;
          this.mensagemErro.set(apiError?.detail ?? 'Falha ao efetuar login. Verifique suas credenciais e tente novamente.');
        },
      });
  }

  protected togglePasswordVisibility(): void {
    this.passwordHidden.update((value) => !value);
  }
}
