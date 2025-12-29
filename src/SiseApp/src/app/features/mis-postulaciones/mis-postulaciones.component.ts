import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { OfertaService } from '../../core/services/oferta.service';
import { OfertaLaboral } from '../../core/models/oferta.model';
import { OfertaDetailComponent } from '../../shared/oferta-detail/oferta-detail.component';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-mis-postulaciones',
  templateUrl: './mis-postulaciones.component.html',
  styleUrls: ['./mis-postulaciones.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatSnackBarModule,
  ],
})
export class MisPostulacionesComponent implements OnInit {
  misOfertas: OfertaLaboral[] = [];
  cargando: boolean = true;

  constructor(
    private ofertaService: OfertaService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.cargarPostulaciones();
  }

  cargarPostulaciones() {
    this.cargando = true;
    this.ofertaService.getMisPostulaciones().subscribe({
      next: (data) => {
        this.misOfertas = data;
        this.cargando = false;
      },
      error: (err) => {
        this.cargando = false;
        const mensaje = err.error || 'Error al cargar tus postulaciones.';
        this.mostrarMensaje(mensaje, 'error');
      },
    });
  }

  // Abre el modal para ver detalles (Reutiliza tu OfertaDetailComponent)
  verDetalle(oferta: OfertaLaboral) {
    const dialogRef = this.dialog.open(OfertaDetailComponent, {
      width: '700px',
      data: {
        oferta: oferta,
        modo: 'CANCELAR_POSTULACION', // Esto habilita el botón rojo en el modal
      },
    });

    dialogRef.afterClosed().subscribe((confirmado) => {
      // Si el modal devuelve true, es que dio clic en "Retirar Postulación" desde dentro
      if (confirmado) {
        this.ejecutarRetiro(oferta);
      }
    });
  }

  // Acción directa desde el botón "Retirar" de la lista
  retirarPostulacion(oferta: OfertaLaboral) {
    if (
      confirm(`¿Estás seguro de retirar tu postulación a ${oferta.titulo}?`)
    ) {
      this.ejecutarRetiro(oferta);
    }
  }

  // Lógica centralizada para llamar al servicio de eliminar
  private ejecutarRetiro(oferta: OfertaLaboral) {
    this.ofertaService.cancelarPostulacion(oferta.idOferta).subscribe({
      next: () => {
        this.mostrarMensaje('Postulación retirada correctamente.', 'success');
        this.cargarPostulaciones(); // Recargar la lista
      },
      error: () => {
        this.mostrarMensaje('No se pudo retirar la postulación.', 'error');
      },
    });
  }

  mostrarMensaje(mensaje: string, tipo: 'success' | 'error') {
    this.snackBar.open(mensaje, 'CERRAR', {
      duration: 4000,
      panelClass:
        tipo === 'error'
          ? ['mat-toolbar', 'mat-warn']
          : ['mat-toolbar', 'mat-primary'],
      verticalPosition: 'top',
    });
  }
}
