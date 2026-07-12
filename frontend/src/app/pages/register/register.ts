import { AbstractControl, NonNullableFormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { finalize, switchMap, tap } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';

import { AuthApiService } from '../../shared/auth/auth-api.service';
import { AuthSessionService } from '../../shared/auth/auth-session.service';
import { CadastroRequest } from '../../shared/auth/auth.models';
import { HttpErrorResponse } from '@angular/common/http';
import { NotificationService } from '../../shared/notification/notification.service';
import { ApiError } from '../../shared/notification/erro-api.model';

function passwordsMatchValidator(control: AbstractControl): ValidationErrors | null {
  const passwordControl = control.get('password');
  const confirmPasswordControl = control.get('confirmPassword');

  if (!passwordControl || !confirmPasswordControl) {
    return null;
  }

  const password = passwordControl.value;
  const confirmPassword = confirmPasswordControl.value;

  if (!password || !confirmPassword) {
    if (confirmPasswordControl.hasError('passwordMismatch')) {
      const { passwordMismatch, ...remainingErrors } = confirmPasswordControl.errors ?? {};
      confirmPasswordControl.setErrors(Object.keys(remainingErrors).length ? remainingErrors : null);
    }

    return null;
  }

  if (password !== confirmPassword) {
    if (!confirmPasswordControl.hasError('passwordMismatch')) {
      confirmPasswordControl.setErrors({
        ...(confirmPasswordControl.errors ?? {}),
        passwordMismatch: false,
      });
    }

    return { passwordMismatch: false };
  }

  if (confirmPasswordControl.hasError('passwordMismatch')) {
    const { passwordMismatch, ...remainingErrors } = confirmPasswordControl.errors ?? {};
    confirmPasswordControl.setErrors(Object.keys(remainingErrors).length ? remainingErrors : null);
  }

  return null;
}

@Component({
  selector: 'sm-register',
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
  templateUrl: './register.html',
  styleUrl: './register.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterPage {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly authApiService = inject(AuthApiService);
  private readonly authSessionService = inject(AuthSessionService);
  private readonly router = inject(Router);
  private readonly notificationService = inject(NotificationService);

  protected readonly formSubmitted = signal(false);
  protected readonly passwordHidden = signal(true);
  protected readonly confirmPasswordHidden = signal(true);
  protected readonly carregando = signal(false);
  protected readonly mensagemErro = signal<string | null>(null);
  protected readonly passwordInputType = computed(() =>
    this.passwordHidden() ? 'password' : 'text',
  );
  protected readonly confirmPasswordInputType = computed(() =>
    this.confirmPasswordHidden() ? 'password' : 'text',
  );

  protected readonly registerForm = this.formBuilder.group(
    {
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
    }
  );

  protected submitRegister(): void {
    this.formSubmitted.set(true);
    this.registerForm.markAllAsTouched();

    if (this.registerForm.invalid) {
      return;
    }

    this.mensagemErro.set(null);
    this.carregando.set(true);

    const dadosCadastro: CadastroRequest = {
      nomeCompleto: this.registerForm.controls.name.value,
      email: this.registerForm.controls.email.value,
      senha: this.registerForm.controls.password.value,
      confirmarSenha: this.registerForm.controls.confirmPassword.value,
    };

    this.authApiService
      .cadastrar(dadosCadastro)
      .pipe(
        switchMap(() =>
          this.authApiService.login({
            email: dadosCadastro.email,
            senha: dadosCadastro.senha,
          }),
        ),
        tap((usuario) => this.authSessionService.salvarLogin(usuario)),
        finalize(() => this.carregando.set(false)),
      )
      .subscribe({
        next: () => {
          void this.router.navigate(['/catalogo']);
        },
        error: (error: HttpErrorResponse) => {
          const apiError = error.error as ApiError;

          this.mensagemErro.set(apiError?.detail ?? 'Falha ao efetuar cadastro. Verifique seus dados e tente novamente.');
        },
      });
  }

  protected togglePasswordVisibility(): void {
    this.passwordHidden.update((value) => !value);
  }

  protected toggleConfirmPasswordVisibility(): void {
    this.confirmPasswordHidden.update((value) => !value);
  }
}
