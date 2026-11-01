import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import {
  AbstractControl,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { finalize, map } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

import { UsuarioApiService } from '../../services/usuario/usuario-api.service';

function passwordsMatchValidator(control: AbstractControl): ValidationErrors | null {
  const passwordControl = control.get('novaSenha');
  const confirmPasswordControl = control.get('confirmarSenha');

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
        passwordMismatch: true,
      });
    }

    return { passwordMismatch: true };
  }

  if (confirmPasswordControl.hasError('passwordMismatch')) {
    const { passwordMismatch, ...remainingErrors } = confirmPasswordControl.errors ?? {};
    confirmPasswordControl.setErrors(Object.keys(remainingErrors).length ? remainingErrors : null);
  }

  return null;
}

@Component({
  selector: 'sm-esqueceu-senha',
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
  templateUrl: './esqueceu-senha.html',
  styleUrl: './esqueceu-senha.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SenhaPage {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly usuarioService = inject(UsuarioApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly tokenParam = toSignal(
    this.route.queryParamMap.pipe(map((params) => params.get('token'))),
    { initialValue: this.route.snapshot.queryParamMap.get('token') },
  );

  protected readonly token = computed(() => this.tokenParam());
  protected readonly redefinindoSenha = computed(() => !!this.token());
  protected readonly carregando = signal(false);
  protected readonly solicitacaoConcluida = signal(false);
  protected readonly senhaAlterada = signal(false);
  protected readonly formSubmitted = signal(false);
  protected readonly mensagemErro = signal<string | null>(null);
  protected readonly passwordHidden = signal(true);
  protected readonly confirmPasswordHidden = signal(true);
  protected readonly passwordInputType = computed(() =>
    this.passwordHidden() ? 'password' : 'text',
  );
  protected readonly confirmPasswordInputType = computed(() =>
    this.confirmPasswordHidden() ? 'password' : 'text',
  );

  protected readonly requestForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
  });

  protected readonly resetForm = this.formBuilder.group(
    {
      novaSenha: ['', [Validators.required, Validators.minLength(8)]],
      confirmarSenha: ['', Validators.required],
    },
    { validators: [passwordsMatchValidator] },
  );

  protected solicitarLink(): void {
    this.formSubmitted.set(true);
    this.requestForm.markAllAsTouched();

    if (this.requestForm.invalid) return;

    this.mensagemErro.set(null);
    this.carregando.set(true);

    this.usuarioService
      .solicitarResetSenha({ email: this.requestForm.getRawValue().email })
      .pipe(finalize(() => this.carregando.set(false)))
      .subscribe({
        next: () => this.solicitacaoConcluida.set(true),
        error: () =>
          this.mensagemErro.set('Não foi possível solicitar o link agora. Tente novamente.'),
      });
  }

  protected redefinir(): void {
    this.formSubmitted.set(true);
    this.resetForm.markAllAsTouched();

    const token = this.token();
    if (this.resetForm.invalid || !token) return;

    this.mensagemErro.set(null);
    this.carregando.set(true);

    this.usuarioService
      .resetSenha(token, this.resetForm.getRawValue())
      .pipe(finalize(() => this.carregando.set(false)))
      .subscribe({
        next: () => this.senhaAlterada.set(true),
        error: (error: HttpErrorResponse) => {
          const detail = (error.error as { detail?: unknown } | null)?.detail;
          this.mensagemErro.set(
            typeof detail === 'string' ? detail : 'O link é inválido ou expirou. Solicite outro.',
          );
        },
      });
  }

  protected alternarVisibilidadeSenha(): void {
    this.passwordHidden.update((value) => !value);
  }

  protected alternarVisibilidadeConfirmacao(): void {
    this.confirmPasswordHidden.update((value) => !value);
  }

  protected solicitarOutroLink(): void {
    this.mensagemErro.set(null);
    this.formSubmitted.set(false);
    this.resetForm.reset();
    void this.router.navigate(['/senha']);
  }

}
