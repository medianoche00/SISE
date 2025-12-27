import { ComponentFixture, TestBed } from '@angular/core/testing';
import { OfertasDisponiblesComponent } from './ofertas-disponibles.component';

describe('OfertasDisponiblesComponent', () => {
  let component: OfertasDisponiblesComponent;
  let fixture: ComponentFixture<OfertasDisponiblesComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [OfertasDisponiblesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OfertasDisponiblesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
