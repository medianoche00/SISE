import { Component, OnInit } from '@angular/core';

interface EmpresaAliada {
  nombreComercial: string;
  ruc: string;
  sector: string;
  nroOfertas: number;
  estado: 'Activa' | 'Pendiente' | 'Vetada';
}

@Component({
  selector: 'app-reportes',
  templateUrl: './reportes.component.html',
  styleUrls: ['./reportes.component.css']
})
export class ReportesComponent implements OnInit {

  empresas: EmpresaAliada[] = [];
  
  // Variables para los filtros
  filtroSectorEmpresa: string = '';
  filtroEstadoEmpresa: string = '';

  constructor() { }

  ngOnInit(): void {
    this.empresas = [
      { nombreComercial: 'Tech Solutions SAC', ruc: '20100055501', sector: 'Tecnologia', nroOfertas: 15, estado: 'Activa' },
      { nombreComercial: 'Banco del Futuro', ruc: '20100011122', sector: 'Banca', nroOfertas: 8, estado: 'Activa' },
      { nombreComercial: 'Minera San Juan', ruc: '20500099901', sector: 'Mineria', nroOfertas: 3, estado: 'Vetada' },
      { nombreComercial: 'Clínica Vida', ruc: '20600088800', sector: 'Salud', nroOfertas: 0, estado: 'Pendiente' }
    ];
  }
}