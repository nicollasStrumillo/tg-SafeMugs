import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SolicitarToken } from './solicitar-token';

describe('SolicitarToken', () => {
  let component: SolicitarToken;
  let fixture: ComponentFixture<SolicitarToken>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SolicitarToken]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SolicitarToken);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
