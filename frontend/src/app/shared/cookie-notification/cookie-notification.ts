import { ChangeDetectionStrategy, Component, inject, signal, type OnInit } from '@angular/core';

import { BrowserCookieService } from '../../services/cookies/browser-cookies.service';

@Component({
  selector: 'sm-cookie-notification',
  imports: [],
  templateUrl: './cookie-notification.html',
  styleUrl: './cookie-notification.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true
})
export class CookieNotification implements OnInit {
    private readonly cookieService = inject(BrowserCookieService);
    
    protected readonly showNotification = signal(false);
  
    ngOnInit(): void {
        if (!this.cookieService.existsCookieStatus()) 
            this.showNotification.set(true);
    }

    protected fecharNotificacao(): void {
        this.cookieService.setCookieStatus('dissmissed');
        this.showNotification.set(false);
    }
}
