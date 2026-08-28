import { ChangeDetectionStrategy, Component, HostListener, inject, signal, type OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { BrowserCookieService } from '../../services/cookies/browser-cookies.service';

@Component({
  selector: 'sm-boas-vindas-notification',
  imports: [MatButtonModule, MatIconModule],
  templateUrl: './boas-vindas-notification.html',
  styleUrl: './boas-vindas-notification.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true
})
export class BoasVindasNotification implements OnInit {
    private readonly cookieService = inject(BrowserCookieService);
    
    protected readonly showNotification = signal(false);
  
    ngOnInit(): void {
        if (!this.cookieService.existsBoasVindasStatus()) 
            this.showNotification.set(true);
    }

    @HostListener('document:keydown.escape')
    protected onEsc(): void {
        if (this.showNotification()) {
            this.fecharNotificacao();
        }
    }

    protected fecharNotificacao(): void {
        this.cookieService.setBoasVindasStatus('dissmissed');
        this.showNotification.set(false);
    }
}