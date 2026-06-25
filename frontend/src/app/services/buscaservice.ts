import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BuscaService {
  termoBusca = signal<string>('');
}
