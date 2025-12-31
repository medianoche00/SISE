import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormacionDialogComponent } from '../formacion-dialog/formacion-dialog.component'; // Importa tu modal
import {
  FormacionComplementaria,
  FormacionComplementariasService,
} from '../../../../core/services/formaciones.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-formacion-complementaria',
  templateUrl: './formacion-complementaria.component.html',
  styleUrls: ['./formacion-complementaria.component.css'],
})
export class FormacionComplementariaComponent implements OnInit {
  listaFormacion: FormacionComplementaria[] = [];
  cargando = false;

  constructor(
    private dialog: MatDialog,
    private formacionService: FormacionComplementariasService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.cargarFormaciones()
  }

  cargarFormaciones() {
    this.cargando = true;
    this.formacionService.obtenerFormaciones().subscribe({
      next: (data) => {
        this.listaFormacion = data;
        this.cargando = false;
      },
      error: (err) => {
        this.cargando = false;
        const mensajeError =
          err.error || 'Ocurrió un error al cargar las formaciones complementarias.';
        this.mostrarMensaje(mensajeError, 'error');
      },
    });
  }

  abrirModal(formacion?: FormacionComplementaria) {
    const dialogRef = this.dialog.open(FormacionDialogComponent, {
      width: '600px',
      data: formacion || null, // Si es null, el modal sabe que es "Crear"
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        if (formacion) {
          // MODO EDICIÓN
          this.formacionService
            .editarFormacion(formacion.idFormacion!, result)
            .subscribe({
              next: (response) => {
                // ÉXITO (200 OK)
                this.mostrarMensaje(
                  'Formación complementaria actualizada con éxito!',
                  'success'
                );
              },
              error: (err) => {
                // ERROR (400 BadRequest o 500)
                const mensajeError =
                  err.error ||
                  'Ocurrió un error al agregar la formación complementaria.';
                this.mostrarMensaje(mensajeError, 'error');
              },
            });
        } else {
          // MODO CREACIÓN
          this.formacionService.crearFormacion(result).subscribe({
            next: (response) => {
              // ÉXITO (200 OK)
              this.mostrarMensaje(
                'Formación complementaria agregada con éxito!',
                'success'
              );
            },
            error: (err) => {
              // ERROR (400 BadRequest o 500)
              const mensajeError =
                err.error ||
                'Ocurrió un error al agregar la formación complementaria.';
              this.mostrarMensaje(mensajeError, 'error');
            },
          });
        }
        this.cargarFormaciones();
      }
    });
  }

  eliminar(formacion: FormacionComplementaria) {
      if (confirm(`¿Estás seguro de eliminar tu formación complementaria en ${formacion.institucion}?`)) {
        this.formacionService
          .eliminarFormacion(formacion.idFormacion!)
          .subscribe(() => {
            this.cargarFormaciones();
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
