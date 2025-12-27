import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistroEgresadoComponent } from './registro-egresado.component';

describe('RegistroEgresadoComponent', () => {
  let component: RegistroEgresadoComponent;
  let fixture: ComponentFixture<RegistroEgresadoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RegistroEgresadoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegistroEgresadoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
