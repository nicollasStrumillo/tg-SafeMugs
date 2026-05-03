import { TestBed } from '@angular/core/testing';

import { Buscaservice } from './buscaservice';

describe('Buscaservice', () => {
  let service: Buscaservice;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Buscaservice);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
