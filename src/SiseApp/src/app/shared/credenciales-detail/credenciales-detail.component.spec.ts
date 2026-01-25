import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CredencialesDetailComponent } from './credenciales-detail.component';

describe('CredencialesDetailComponent', () => {
  let component: CredencialesDetailComponent;
  let fixture: ComponentFixture<CredencialesDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CredencialesDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CredencialesDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
