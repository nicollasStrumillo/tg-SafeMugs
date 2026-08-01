import { Injectable } from '@angular/core';

const BACKUP_DESAFIOS_COOKIE_NAME = 'backupCookie';
const BACKUP_QUIZZES_COOKIE_NAME = 'backupQuizzesCookie';
const COOKIE_STATUS_NAME = 'cookieStatus';

@Injectable({
  providedIn: 'root'
})
export class BrowserCookieService {

    // Backup desafios 
    public setBackupDesafiosCookie(value: string): void {
        this.set(BACKUP_DESAFIOS_COOKIE_NAME, value);
    }

    public getBackupDesafiosCookie(): string | null {
        return this.get(BACKUP_DESAFIOS_COOKIE_NAME);
    }

    public removeBackupDesafiosCookie(): void {
        this.remove(BACKUP_DESAFIOS_COOKIE_NAME);
    }

    // Backup quizzes
    public setBackupQuizzesCookie(value: string): void{
        this.set(BACKUP_QUIZZES_COOKIE_NAME, value);
    }

    public getBackupQuizzesCookie(): string | null {
        return this.get(BACKUP_QUIZZES_COOKIE_NAME);
    }

    public removeBackupQuizzesCookie(): void {
        this.remove(BACKUP_QUIZZES_COOKIE_NAME);
    }

    // Cookies status
    public setCookieStatus(value: string): void {
        this.set(COOKIE_STATUS_NAME, value);
    }

    public getCookieStatus(): string | null {
        return this.get(COOKIE_STATUS_NAME);
    }

    public existsCookieStatus(): boolean {
        return this.exists(COOKIE_STATUS_NAME);
    }

    public removeCookieStatus(): void {
        this.remove(COOKIE_STATUS_NAME);
    }

    // Utils
    public set(name: string, value: string, expires?: Date): void {
        let cookie = `${encodeURIComponent(name)}=${encodeURIComponent(value)}; path=/`;

        if (expires == null) {
            expires = new Date();
            expires.setFullYear(expires.getFullYear() + 1); // Define a expiração para 1 ano a partir de agora
        }
        cookie += `; expires=${expires.toUTCString()}`;

        document.cookie = cookie;
    }

    public get(name: string): string | null {
        const nameEncoded = encodeURIComponent(name) + '=';

        const cookies = document.cookie.split(';');

        for (const cookie of cookies) {
            const trimmed = cookie.trim();

            if (trimmed.startsWith(nameEncoded)) {
                return decodeURIComponent(trimmed.substring(nameEncoded.length));
            }
        }

        return null;
    }

    public exists(name: string): boolean {
        return this.get(name) !== null;
    }

    public remove(name: string): void {
        document.cookie = `${encodeURIComponent(name)}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/`;
    }

    public clear(): void {
        const cookies = document.cookie.split(';');

        for (const cookie of cookies) {
            const cookieName = cookie.split('=')[0].trim();

            document.cookie =
            `${cookieName}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/`;
        }
    }
}