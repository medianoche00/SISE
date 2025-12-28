import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormacionComplementariaComponent } from './formacion-complementaria.component';

describe('FormacionComplementariaComponent', () => {
  let component: FormacionComplementariaComponent;
  let fixture: ComponentFixture<FormacionComplementariaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FormacionComplementariaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormacionComplementariaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
