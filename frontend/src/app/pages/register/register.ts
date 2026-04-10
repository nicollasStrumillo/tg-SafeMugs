import { AbstractControl, NonNullableFormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { RouterLink } from '@angular/router';

function passwordsMatchValidator(control: AbstractControl): ValidationErrors | null {
  const password = control.get('password')?.value;
  const confirmPassword = control.get('confirmPassword')?.value;

  if (!password || !confirmPassword) {
    return null;
  }

  return password === confirmPassword ? null : { passwordMismatch: true };
}

@Component({
  selector: 'sm-register',
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

  protected readonly formSubmitted = signal(false);
  protected readonly passwordHidden = signal(true);
  protected readonly confirmPasswordHidden = signal(true);
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
    },
    { validators: [passwordsMatchValidator] },
  );

  protected submitRegister(): void {
    this.formSubmitted.set(true);
    this.registerForm.markAllAsTouched();
  }

  protected togglePasswordVisibility(): void {
    this.passwordHidden.update((value) => !value);
  }

  protected toggleConfirmPasswordVisibility(): void {
    this.confirmPasswordHidden.update((value) => !value);
  }
}
