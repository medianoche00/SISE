import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { OfertaService } from '../../core/services/oferta.service';
import { OfertaLaboral } from '../../core/models/oferta.model';
import { OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { MatDivider } from '@angular/material/divider';
import { OfertaDetailComponent } from '../../shared/oferta-detail/oferta-detail.component';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatOption } from '@angular/material/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { PostularService } from '../../core/services/postular.service';

@Component({
  selector: 'app-ofertas-disponibles',
  templateUrl: './ofertas-disponibles.component.html',
  styleUrls: ['./ofertas-disponibles.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatIconModule,
    MatDivider,
    MatFormField,
    MatLabel,
    MatOption,
    MatSelectModule,
    MatInputModule,
    MatButtonModule
  ],
})
export class OfertasDisponiblesComponent implements OnInit {
  ofertasOriginales: OfertaLaboral[] = [];
  ofertasFiltradas: OfertaLaboral[] = [];

  // Variables para filtros
  textoBusqueda: string = '';
  filtroModalidad: string = '';
  filtroTipoContrato: string = '';
  ordenamiento: string = 'recientes';

  constructor(
    private ofertaService: OfertaService,
    private postularService: PostularService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.cargarOfertas();
  }

  cargarOfertas() {
    this.ofertaService.getOfertasDisponibles().subscribe((data) => {
      this.ofertasOriginales = data;
      this.aplicarFiltros(); // Aplicar filtros iniciales
    });
  }

  aplicarFiltros() {
    let resultado = [...this.ofertasOriginales];

    // 1. Filtro de Texto (Título o Empresa)
    if (this.textoBusqueda) {
      const texto = this.textoBusqueda.toLowerCase();
      resultado = resultado.filter(
        (o) =>
          o.titulo.toLowerCase().includes(texto) ||
          o.empresaRazonSocial?.toLowerCase().includes(texto)
      );
    }

    // Filtro Modalidad
    if (this.filtroModalidad) {
      resultado = resultado.filter(
        (o) =>
          o.modalidad === this.filtroModalidad
      );
    }

    // Filtro Tipo de Contrato
    if (this.filtroTipoContrato) {
      resultado = resultado.filter(
        (o) =>
          o.tipoContrato === this.filtroTipoContrato
      );
    }

    // 3. Ordenamiento
    if (this.ordenamiento === 'recientes') {
      resultado.sort(
        (a, b) =>
          new Date(b.fechaPublicacion).getTime() -
          new Date(a.fechaPublicacion).getTime()
      );
    } else if (this.ordenamiento === 'salario_desc') {
      resultado.sort((a, b) => b.sueldo - a.sueldo);
    } else if (this.ordenamiento === 'salario_asc') {
      resultado.sort((a, b) => a.sueldo - b.sueldo);
    }

    this.ofertasFiltradas = resultado;
  }

  verDetalle(oferta: OfertaLaboral) {
    const dialogRef = this.dialog.open(OfertaDetailComponent, {
      width: '700px',
      data: { oferta, modo: 'POSTULAR' },
    });

    dialogRef.afterClosed().subscribe((confirmado) => {
      // 'confirmado' será true solo si dio clic en el botón POSTULAR del modal
      if (confirmado) {
        this.realizarPostulacion(oferta.idOferta);
      }
    });
  }

  realizarPostulacion(idOferta: number) {
    this.postularService.postularOferta(idOferta, '').subscribe({ //! agregar campo de carta de presentacion
      next: (response) => {
        // ÉXITO (200 OK)
        this.mostrarMensaje('¡Postulación enviada con éxito!', 'success');
      },
      error: (err) => {
        // ERROR (400 BadRequest o 500)
        const mensajeError = err.error || 'Ocurrió un error al postular.';
        this.mostrarMensaje(mensajeError, 'error');
      },
    });
  }

  // Helper para mostrar mensajes tipo "Toast"
  mostrarMensaje(mensaje: string, tipo: 'success' | 'error') {
    this.snackBar.open(mensaje, 'CERRAR', {
      duration: 4000,
      panelClass:
        tipo === 'error'
          ? ['mat-toolbar', 'mat-warn']
          : ['mat-toolbar', 'mat-primary'],
      verticalPosition: 'top', // Para que salga arriba
    });
  }
}
