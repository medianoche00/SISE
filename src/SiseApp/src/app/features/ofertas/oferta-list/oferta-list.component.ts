import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { OfertaDetailComponent } from '../../../shared/oferta-detail/oferta-detail.component';
import { OfertaService } from '../../../core/services/oferta.service';
import { OfertaLaboral } from '../../../core/models/oferta.model';
import { OnInit } from '@angular/core';

@Component({
  selector: 'app-oferta-list',
  templateUrl: './oferta-list.component.html',
  styleUrls: ['./oferta-list.component.css']
})
export class OfertaListComponent implements OnInit {
  ofertasOriginales: OfertaLaboral[] = [];
  ofertasFiltradas: OfertaLaboral[] = [];

  // Variables para filtros
  textoBusqueda: string = '';
  filtroModalidad: string = '';
  ordenamiento: string = 'recientes';

  constructor(
    private ofertaService: OfertaService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.cargarOfertas();
  }

  cargarOfertas() {
    this.ofertaService.getOfertasActivas().subscribe(data => {
      this.ofertasOriginales = data;
      this.aplicarFiltros(); // Aplicar filtros iniciales
    });
  }

  aplicarFiltros() {
    let resultado = [...this.ofertasOriginales];

    // 1. Filtro de Texto (Título o Empresa)
    if (this.textoBusqueda) {
      const texto = this.textoBusqueda.toLowerCase();
      resultado = resultado.filter(o => 
        o.titulo.toLowerCase().includes(texto) || 
        o.idEmpresaNavigation?.razonSocial.toLowerCase().includes(texto)
      );
    }

    // 2. Filtro Modalidad (Ejemplo de select)
    if (this.filtroModalidad) {
      resultado = resultado.filter(o => 
        o.idModalidadTrabajoNavigation?.nombreModalidad === this.filtroModalidad
      );
    }

    // 3. Ordenamiento
    if (this.ordenamiento === 'recientes') {
      resultado.sort((a, b) => new Date(b.fechaPublicacion).getTime() - new Date(a.fechaPublicacion).getTime());
    } else if (this.ordenamiento === 'salario_desc') {
      resultado.sort((a, b) => b.sueldo - a.sueldo);
    } else if (this.ordenamiento === 'salario_asc') {
      resultado.sort((a, b) => a.sueldo - b.sueldo);
    }

    this.ofertasFiltradas = resultado;
  }

  verDetalle(oferta: OfertaLaboral) {
    this.dialog.open(OfertaDetailComponent, {
      width: '700px',
      data: { oferta, modo: 'POSTULAR' } 
    });
  }
}