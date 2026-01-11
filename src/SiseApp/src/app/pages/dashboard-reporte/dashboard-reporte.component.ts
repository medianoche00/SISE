import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard-reporte',
  templateUrl: './dashboard-reporte.component.html',
  styleUrls: ['./dashboard-reporte.component.css']
})
export class DashboardReporteComponent {
  // Ya no necesitamos nada aquí porque solo es un menú
}

// export class DashboardReporteComponent implements OnInit {

//   kpis: any = {
//     totalEgresados: 0,
//     egresadosTrabajando: 0,
//     ofertasActivas: 0
//   };

//   tablaCarreras: any[] = [];

//   constructor() { }

//   ngOnInit(): void {
//     this.cargarDatosPrueba();
//   }

//   cargarDatosPrueba() {
//     this.kpis = {
//       totalEgresados: 1540,
//       egresadosTrabajando: 1200,
//       ofertasActivas: 45
//     };

//     this.tablaCarreras = [
//       { carrera: 'Ingeniería de Sistemas', total: 450, trabajando: 400, porcentaje: 88 },
//       { carrera: 'Administración', total: 300, trabajando: 210, porcentaje: 70 },
//       { carrera: 'Diseño Gráfico', total: 200, trabajando: 150, porcentaje: 75 },
//       { carrera: 'Contabilidad', total: 300, trabajando: 130, porcentaje: 43 },
//       { carrera: 'Marketing', total: 290, trabajando: 200, porcentaje: 69 }
//     ];
//   }

// }
