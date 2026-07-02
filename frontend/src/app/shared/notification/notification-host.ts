import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

import { NotificationService, NotificationVariant } from './notification.service';

@Component({
	selector: 'sm-notification-host',
	imports: [CommonModule, MatIconModule],
	templateUrl: './notification-host.html',
	styleUrl: './notification-host.scss',
	changeDetection: ChangeDetectionStrategy.OnPush,
	standalone: true,
})
export class NotificationHostComponent {
	private readonly notificationService = inject(NotificationService);

	protected readonly notification = this.notificationService.notification;

	protected fechar(): void {
		this.notificationService.fechar();
	}

	protected iconFor(variant: NotificationVariant): string {
		switch (variant) {
			case 'success':
				return 'check_circle';
			case 'error':
				return 'error';
			default:
				return 'info';
		}
	}
}