import { Injectable, computed, signal } from '@angular/core';

export type NotificationVariant = 'info' | 'success' | 'error';

export interface NotificationState {
	message: string;
	icon?: string;
	variant: NotificationVariant;
}

export interface NotificationOptions {
	icon?: string;
	durationMs?: number;
	variant?: NotificationVariant;
}

const DEFAULT_DURATION_MS = 3200;

@Injectable({
	providedIn: 'root',
})
export class NotificationService {
	private readonly notificationSignal = signal<NotificationState | null>(null);
	private timeoutId: ReturnType<typeof setTimeout> | null = null;

	public readonly notification = this.notificationSignal.asReadonly();
	public readonly hasNotification = computed(() => this.notificationSignal() !== null);

	public mostrar(message: string, options: NotificationOptions = {}): void {
		const texto = message.trim();

		if (!texto) {
			return;
		}

		this.limparTimeout();
		this.notificationSignal.set({
			message: texto,
			icon: options.icon,
			variant: options.variant ?? 'info',
		});

		const durationMs = options.durationMs ?? DEFAULT_DURATION_MS;
		if (durationMs > 0) {
			this.timeoutId = setTimeout(() => this.fechar(), durationMs);
		}
	}

	public sucesso(message: string, options: Omit<NotificationOptions, 'variant'> = {}): void {
		this.mostrar(message, {
			...options,
			variant: 'success',
		});
	}

	public erro(message: string, options: Omit<NotificationOptions, 'variant'> = {}): void {
		this.mostrar(message, {
			...options,
			variant: 'error',
			durationMs: options.durationMs ?? 4500,
		});
	}

	public info(message: string, options: Omit<NotificationOptions, 'variant'> = {}): void {
		this.mostrar(message, {
			...options,
			variant: 'info',
		});
	}

	public fechar(): void {
		this.limparTimeout();
		this.notificationSignal.set(null);
	}

	private limparTimeout(): void {
		if (this.timeoutId !== null) {
			clearTimeout(this.timeoutId);
			this.timeoutId = null;
		}
	}
}