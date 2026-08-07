import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, ElementRef, OnInit, ViewChild, inject, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

import { AuthSessionService } from '../../services/usuario/auth/auth-session.service';
import { UsuarioApiService } from '../../services/usuario/usuario-api.service';
import { AuthTokenResponse, DetalhesUsuarioResponse, EditarUsuarioRequest, UsuarioLogado } from '../../services/usuario/usuario.models';
import { NotificationService } from '../../shared/notification/notification.service';
import { PerfilConfirmarExclusao } from './perfil-confirmar-exclusao';

const FOTO_GENERICA = '/imagens/perfil/generic_profile.jpg';

function campoVazio(value: unknown): boolean {
	if (value === null || value === undefined) {
		return true;
	}
	if (typeof value === 'number') {
		return Number.isNaN(value);
	}
	return String(value).trim() === '';
}

function enderecoCompletoOuVazio(group: AbstractControl): ValidationErrors | null {
	const logradouro = group.get('logradouro')?.value;
	const numero = group.get('numero')?.value;
	const complemento = group.get('complemento')?.value;
	const bairro = group.get('bairro')?.value;
	const cidade = group.get('cidade')?.value;
	const estado = group.get('estado')?.value;
	const cep = group.get('cep')?.value;

	const algumPreenchido =
		!campoVazio(logradouro) ||
		!campoVazio(numero) ||
		!campoVazio(complemento) ||
		!campoVazio(bairro) ||
		!campoVazio(cidade) ||
		!campoVazio(estado) ||
		!campoVazio(cep);

	if (!algumPreenchido) {
		return null;
	}

	const obrigatoriosOk =
		!campoVazio(logradouro) &&
		!campoVazio(numero) &&
		!campoVazio(bairro) &&
		!campoVazio(cidade) &&
		!campoVazio(estado) &&
		!campoVazio(cep);

	return obrigatoriosOk ? null : { enderecoIncompleto: true };
}

function emptyToNull(value: string | null | undefined): string | null {
	const trimmed = (value ?? '').trim();
	return trimmed === '' ? null : trimmed;
}

@Component({
	selector: 'sm-perfil',
	standalone: true,
	imports: [
		MatButtonModule,
		MatCardModule,
		MatDialogModule,
		MatFormFieldModule,
		MatIconModule,
		MatInputModule,
		ReactiveFormsModule,
	],
	templateUrl: './perfil.html',
	styleUrl: './perfil.scss',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Perfil implements OnInit {
	@ViewChild('fileInput') private readonly fileInput?: ElementRef<HTMLInputElement>;

	private readonly fb = inject(FormBuilder);
	private readonly usuarioApi = inject(UsuarioApiService);
	private readonly auth = inject(AuthSessionService);
	private readonly notification = inject(NotificationService);
	private readonly router = inject(Router);
	private readonly dialog = inject(MatDialog);

	protected readonly carregando = signal(true);
	protected readonly salvando = signal(false);
	protected readonly salvandoSenha = signal(false);
	protected readonly enviandoUrlFoto = signal(false);
	protected readonly enviandoUpload = signal(false);
	protected readonly desativando = signal(false);
	protected readonly formSubmitted = signal(false);

	protected readonly email = signal('');
	protected readonly urlImagemPerfil = signal(FOTO_GENERICA);
	protected readonly perfilNome = signal('');

	protected readonly usuarioLogado = signal<UsuarioLogado | null>(null);

	protected readonly editarForm = this.fb.group(
		{
			nomeCompleto: ['', [Validators.required, Validators.minLength(3)]],
			telefone: ['', [Validators.minLength(11), Validators.maxLength(11), Validators.pattern(/^\d+$/)]],
			logradouro: [''],
			numero: [this.fb.control<number | null>(null), [Validators.min(1), Validators.pattern(/^\d+$/)]],
			complemento: [''],
			bairro: [''],
			cidade: [''],
			estado: [''],
			cep: ['', [Validators.pattern(/^\d+$/)]],
		},
		{ validators: [enderecoCompletoOuVazio] },
	);

	protected readonly senhaForm = this.fb.group({
		novaSenha: ['', [Validators.minLength(8)]],
	});

	protected readonly urlFotoForm = this.fb.group({
		url: [''],}
	);

	ngOnInit(): void {
		const usuario = this.auth.usuarioLogado();
		if (!usuario) {
			void this.router.navigate(['/login']);
			return;
		}
		this.usuarioLogado.set(usuario);

		this.carregarDetalhes(usuario.usuarioId);
	}

	protected salvarPerfil(): void {
		this.formSubmitted.set(true);
		this.editarForm.markAllAsTouched();

		if (this.editarForm.invalid || this.salvando()) {
			return;
		}

		const request = this.montarEditarRequest();
		this.salvando.set(true);

		this.usuarioApi
			.editarUsuario(request)
			.pipe(finalize(() => this.salvando.set(false)))
			.subscribe({
				next: (token) => {
					this.aplicarToken(token);
					this.notification.sucesso('Perfil atualizado com sucesso.');
				},
				error: (erro: HttpErrorResponse) => this.notification.notificarErroApi(erro),
			});
	}

	protected limparEndereco(): void {
		this.editarForm.patchValue({
			logradouro: '',
			numero: null,
			complemento: '',
			bairro: '',
			cidade: '',
			estado: '',
			cep: '',
		});
		this.editarForm.updateValueAndValidity();
	}

	protected enviarNovaSenha(): void {
		this.senhaForm.markAllAsTouched();

		if (this.senhaForm.invalid || this.salvandoSenha() || this.senhaForm.controls.novaSenha.value === null || this.senhaForm.controls.novaSenha.value.trim() === '') {
			return;
		}

		const novaSenha = (this.senhaForm.controls.novaSenha.value ?? '').trim();
		this.salvandoSenha.set(true);

		this.usuarioApi
			.mudarSenha({ novaSenha })
			.pipe(finalize(() => this.salvandoSenha.set(false)))
			.subscribe({
				next: () => {
					this.senhaForm.reset({ novaSenha: '' });
					this.notification.sucesso('Senha alterada com sucesso.');
				},
				error: (erro: HttpErrorResponse) => this.notification.notificarErroApi(erro),
			});
	}

	protected abrirConfirmacaoExclusao(): void {
		if (this.desativando()) {
			return;
		}

		this.dialog
			.open(PerfilConfirmarExclusao, { width: '520px' })
			.afterClosed()
			.subscribe((confirmado: boolean | undefined) => {
				if (confirmado) {
					this.desativarConta(this.usuarioLogado()!.usuarioId);
				}
			});
	}

	protected enviarFotoUrl(): void {
		this.urlFotoForm.markAllAsTouched();

		if (this.enviandoUrlFoto() || this.urlFotoForm.controls.url.value === null || this.urlFotoForm.controls.url.value.trim() === '')
			return;
			
		const url = (this.urlFotoForm.controls.url.value ?? '').trim();
		this.enviandoUrlFoto.set(true);

		this.usuarioApi
			.uploadFotoPerfilUrl({ url })
			.pipe(finalize(() => this.enviandoUrlFoto.set(false)))
			.subscribe({
				next: (token) => {
					this.aplicarToken(token);
					this.urlFotoForm.reset({ url: '' });
					this.notification.sucesso('Foto de perfil atualizada.');
				},
				error: (erro: HttpErrorResponse) => this.notification.notificarErroApi(erro),
			});
	}

	protected abrirSeletorArquivo(): void {
		this.fileInput?.nativeElement.click();
	}

	protected onArquivoSelecionado(event: Event): void {
		const input = event.target as HTMLInputElement;
		const arquivo = input.files?.[0];
		input.value = '';

		if (!arquivo || this.enviandoUpload()) {
			return;
		}

		const mimeOk = arquivo.type === 'image/jpeg';
		const nomeOk = /\.jpe?g$/i.test(arquivo.name);
		if (!mimeOk && !nomeOk) {
			this.notification.erro('Selecione um arquivo JPEG (.jpg).');
			return;
		}

		if (arquivo.size === 0) {
			this.notification.erro('O arquivo selecionado está vazio.');
			return;
		}

		this.enviandoUpload.set(true);
		this.usuarioApi
			.uploadFotoPerfil(arquivo)
			.pipe(finalize(() => this.enviandoUpload.set(false)))
			.subscribe({
				next: (token) => {
					this.aplicarToken(token);
					this.notification.sucesso('Foto de perfil enviada com sucesso.');
				},
				error: (erro: HttpErrorResponse) => this.notification.notificarErroApi(erro),
			});
	}

	protected onFotoError(event: Event): void {
		const img = event.target as HTMLImageElement;
		img.src = FOTO_GENERICA;
	}

	private carregarDetalhes(usuarioId: number): void {
		this.carregando.set(true);
		this.usuarioApi
			.detalhes(usuarioId)
			.pipe(finalize(() => this.carregando.set(false)))
			.subscribe({
				next: (detalhes) => this.preencherFormulario(detalhes),
				error: (erro: HttpErrorResponse) => {
					this.notification.notificarErroApi(erro);
					void this.router.navigate(['/catalogo']);
				},
			});
	}

	private preencherFormulario(detalhes: DetalhesUsuarioResponse): void {
		this.email.set(detalhes.email);
		this.perfilNome.set(detalhes.perfil);
		this.urlImagemPerfil.set(this.comCacheBust(detalhes.urlImagemPerfil || FOTO_GENERICA));

		const endereco = detalhes.endereco;
		this.editarForm.patchValue({
			nomeCompleto: detalhes.nomeCompleto,
			telefone: detalhes.telefone ?? '',
			logradouro: endereco?.logradouro ?? '',
			numero: endereco?.numero ?? null,
			complemento: endereco?.complemento ?? '',
			bairro: endereco?.bairro ?? '',
			cidade: endereco?.cidade ?? '',
			estado: endereco?.estado ?? '',
			cep: endereco?.cep ?? '',
		});
		this.editarForm.markAsPristine();
		this.formSubmitted.set(false);
	}

	private montarEditarRequest(): EditarUsuarioRequest {
		const v = this.editarForm.getRawValue();
		const numero =
			v.numero === null || v.numero === undefined || Number.isNaN(Number(v.numero))
				? null
				: Number(v.numero);

		return {
			nomeCompleto: (v.nomeCompleto ?? '').trim(),
			telefone: emptyToNull(v.telefone),
			logradouro: emptyToNull(v.logradouro),
			numero,
			complemento: emptyToNull(v.complemento),
			bairro: emptyToNull(v.bairro),
			cidade: emptyToNull(v.cidade),
			estado: emptyToNull(v.estado),
			cep: emptyToNull(v.cep),
		};
	}

	private desativarConta(usuarioId: number): void {
		this.desativando.set(true);
		this.usuarioApi
			.desativarUsuario(usuarioId)
			.pipe(finalize(() => this.desativando.set(false)))
			.subscribe({
				next: () => {
					this.auth.limparSessao();
					this.notification.sucesso('Conta deletada com sucesso.');
					void this.router.navigate(['/catalogo']);
				},
				error: (erro: HttpErrorResponse) => this.notification.notificarErroApi(erro),
			});
	}

	private aplicarToken(token: AuthTokenResponse): void {
		const imagemComCacheBust = this.comCacheBust(token.urlImagemPerfil || FOTO_GENERICA);

		token.urlImagemPerfil = imagemComCacheBust;
		this.auth.salvarToken(token);
		
		this.urlImagemPerfil.set(imagemComCacheBust);
	}

	private comCacheBust(url: string): string {
		const base = url.split('?')[0] || FOTO_GENERICA;
		return `${base}?t=${Date.now()}`;
	}

	protected somenteNumeros(event: KeyboardEvent): void {
		const charCode = event.key.charCodeAt(0);

		// Permite apenas 0-9
		if (charCode < 48 || charCode > 57) {
			event.preventDefault();
		}
	}
}
