import { Component, OnInit } from '@angular/core';

interface DashboardKPI {
  totalEgresados: number;
  egresadosTrabajando: number;
  ofertasActivas: number;
}

interface DatosCarrera {
  carrera: string;
  total: number;
  trabajando: number;
  porcentaje: number;
}

@Component({
  selector: 'app-estadisticas',
  templateUrl: './estadisticas.component.html',
  styleUrls: ['./estadisticas.component.css']
})
export class EstadisticasComponent implements OnInit {

  kpis: DashboardKPI = { totalEgresados: 0, egresadosTrabajando: 0, ofertasActivas: 0 };
  tablaCarreras: DatosCarrera[] = [];
  
  // Filtros visuales (simulados)
  filtroFacultad: string = '';
  filtroAnio: string = '';

  constructor() { }

  ngOnInit(): void {
    // Datos simulados (MOCK)
    this.kpis = { totalEgresados: 1250, egresadosTrabajando: 890, ofertasActivas: 45 };
    this.tablaCarreras = [
      { carrera: 'Ingeniería de Sistemas', total: 150, trabajando: 140, porcentaje: 93 },
      { carrera: 'Administración', total: 200, trabajando: 160, porcentaje: 80 },
      { carrera: 'Marketing Digital', total: 100, trabajando: 75, porcentaje: 75 },
      { carrera: 'Contabilidad', total: 180, trabajando: 150, porcentaje: 83 }
    ];
  }
}