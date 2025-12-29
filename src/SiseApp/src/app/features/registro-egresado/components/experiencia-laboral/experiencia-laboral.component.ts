import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ExperienciaDialogComponent } from '../experiencia-dialog/experiencia-dialog.component';
import {
  ExperienciaLaboral,
  ExperienciasService,
} from '../../../../core/services/experiencias.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-experiencia-laboral',
  templateUrl: './experiencia-laboral.component.html',
  styleUrls: ['./experiencia-laboral.component.css'],
})
export class ExperienciaLaboralComponent implements OnInit {
  experiencias: ExperienciaLaboral[] = [];
  cargando: boolean = true;

  constructor(
    private experienciaService: ExperienciasService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.cargarExperiencias();
  }

  cargarExperiencias() {
    this.cargando = true;
    this.experienciaService.obtenerExperiencias().subscribe({
      next: (data) => {
        //console.log(data);
        this.experiencias = data;
        this.cargando = false;
      },
      error: (err) => {
        this.cargando = false;
        const mensajeError = err.error || 'Ocurrió un error al cargar las experiencias laborales.';
        this.mostrarMensaje(mensajeError, 'error');
      }
    });
  }

  abrirModal(experiencia?: ExperienciaLaboral) {
    const dialogRef = this.dialog.open(ExperienciaDialogComponent, {
      width: '600px',
      data: experiencia || null, // Si es null, el modal sabe que es "Crear"
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        if (experiencia) { // MODO EDICIÓN
          this.experienciaService.editarExperiencia(experiencia.idExperiencia!, result).subscribe({
            next: (response) => { // ÉXITO (200 OK)
              this.mostrarMensaje('Experiencia laboral actualizada con éxito!', 'success');
            },
            error: (err) => { // ERROR (400 BadRequest o 500)
              const mensajeError = err.error || 'Ocurrió un error al agregar la experiencia laboral.';
              this.mostrarMensaje(mensajeError, 'error');
            },
            });
        } else { // MODO CREACIÓN
          this.experienciaService.crearExperiencia(result).subscribe({
            next: (response) => { // ÉXITO (200 OK)
              this.mostrarMensaje('Experiencia laboral agregada con éxito!', 'success');
            },
            error: (err) => { // ERROR (400 BadRequest o 500)
              const mensajeError = err.error || 'Ocurrió un error al agregar la experiencia laboral.';
              this.mostrarMensaje(mensajeError, 'error');
            },
            });
        }
        this.cargarExperiencias();
      }
    });
  }

  eliminar(experiencia: ExperienciaLaboral) {
    if (
      confirm(
        `¿Estás seguro de eliminar tu experiencia en ${experiencia.empresa}?`
      )
    ) {
      this.experienciaService
        .eliminarExperiencia(experiencia.idExperiencia!)
        .subscribe(() => {
          this.cargarExperiencias();
        });
    }
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
