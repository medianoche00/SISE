import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardReporteComponent } from './dashboard-reporte.component';

describe('DashboardReporteComponent', () => {
  let component: DashboardReporteComponent;
  let fixture: ComponentFixture<DashboardReporteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DashboardReporteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardReporteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
