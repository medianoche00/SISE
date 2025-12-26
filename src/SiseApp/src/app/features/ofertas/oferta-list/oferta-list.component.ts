import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { OfertaDetailComponent } from '../../../shared/oferta-detail/oferta-detail.component';
import { OfertaService } from '../../../core/services/oferta.service';
import { OfertaLaboral } from '../../../core/models/oferta.model';
import { OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

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
    private dialog: MatDialog,
    private authService: AuthService,
    private snackBar: MatSnackBar
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
    const dialogRef = this.dialog.open(OfertaDetailComponent, {
      width: '700px',
      data: { oferta, modo: 'POSTULAR' } 
    });
    
    dialogRef.afterClosed().subscribe(confirmado => {
      // 'confirmado' será true solo si dio clic en el botón POSTULAR del modal
      if (confirmado) {
        this.realizarPostulacion(oferta.idOferta);
      }
    });
  }

  realizarPostulacion(idOferta: number) {
    // 1. Obtener el ID del usuario logueado
    const usuarioId = this.authService.getUserId(); 
    
    if (!usuarioId) {
      this.mostrarMensaje('Debes iniciar sesión para postular', 'error');
      return;
    }

    // 2. Llamar al servicio
    this.ofertaService.postularOferta(idOferta, usuarioId).subscribe({
      next: (response) => {
        // ÉXITO (200 OK)
        this.mostrarMensaje('¡Postulación enviada con éxito!', 'success');
      },
      error: (err) => {
        // ERROR (400 BadRequest o 500)
        // El backend devuelve el mensaje de error en 'err.error'
        const mensajeError = err.error || 'Ocurrió un error al postular.';
        this.mostrarMensaje(mensajeError, 'error');
      }
    });
  }

  // Helper para mostrar mensajes tipo "Toast"
  mostrarMensaje(mensaje: string, tipo: 'success' | 'error') {
    this.snackBar.open(mensaje, 'CERRAR', {
      duration: 4000,
      panelClass: tipo === 'error' ? ['mat-toolbar', 'mat-warn'] : ['mat-toolbar', 'mat-primary'],
      verticalPosition: 'top' // Para que salga arriba
    });
  }
}