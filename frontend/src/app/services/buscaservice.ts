import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class Buscaservice {
  termoBusca = signal<string>('');
}
