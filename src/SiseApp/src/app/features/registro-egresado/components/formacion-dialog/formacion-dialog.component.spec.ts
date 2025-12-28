import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormacionDialogComponent } from './formacion-dialog.component';

describe('FormacionDialogComponent', () => {
  let component: FormacionDialogComponent;
  let fixture: ComponentFixture<FormacionDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FormacionDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormacionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
