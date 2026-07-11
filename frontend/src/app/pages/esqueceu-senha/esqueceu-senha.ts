import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors, ReactiveFormsModule} from '@angular/forms';
import { MatError, MatFormField, MatLabel } from "@angular/material/form-field";
import { MatIcon } from "@angular/material/icon";
import { MatCardActions, MatCardContent, MatCardSubtitle, MatCardHeader, MatCardTitle, MatCard } from "@angular/material/card";
import { CommonModule } from '@angular/common';
import { SenhaService } from '../../shared/senha/senha-service'; 
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
@Component({
  selector: 'sm-esqueceu-senha',
  imports: [CommonModule, MatError, MatIcon, MatCardActions, MatFormField, MatLabel, MatCardContent, MatCardSubtitle, MatCardHeader, MatCardTitle, MatCard, ReactiveFormsModule,
    MatInputModule, MatButtonModule
  ],
  templateUrl: './esqueceu-senha.html',
  styleUrl: './esqueceu-senha.scss',
})
export class SenhaPage implements OnInit {
resetForm!: FormGroup;

  hidePassword = true;
  hideConfirmPassword = true;

  constructor(
    private fb: FormBuilder,
    private senhaService: SenhaService
  ) {}

  ngOnInit(): void {

    this.resetForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, {
      validators: this.matchPasswordsValidator
    });

  }

  matchPasswordsValidator(control: AbstractControl): ValidationErrors | null {

    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    if (password !== confirmPassword) {
      control.get('confirmPassword')?.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }

    return null;
  }

  onSubmit(): void {

    if (this.resetForm.invalid) {
      return;
    }

    this.senhaService.resetSenha({
      email: this.resetForm.value.email,
      novaSenha: this.resetForm.value.password
    }).subscribe({
      next: () => {
        console.log('Senha alterada com sucesso.');
      },
      error: (erro) => {
        console.error(erro);
      }
    });

  }
}
