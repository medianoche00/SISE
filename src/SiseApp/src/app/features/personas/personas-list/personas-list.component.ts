import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

// Importaciones de tus modelos actualizados
import {
  Persona,
  PersonaCrearDto,
  PersonaActualizarDto
} from '../../../core/models/persona.model';

import { PersonaService } from '../../../core/services/persona.service';
import { PersonaDetailComponent } from '../../../shared/persona-detail/persona-detail.component';
import { UsuariosPersonaComponent } from '../../usuarios-persona/usuarios-persona.component';
import { EliminarModalComponent } from '../../../shared/eliminar-modal/eliminar-modal.component';

@Component({
  selector: 'app-personas-list',
  templateUrl: './personas-list.component.html',
  styleUrls: ['./personas-list.component.scss'],
})
export class PersonasListComponent implements OnInit {
  displayedColumns: string[] = [
    'nombres',
    'apellidoPaterno',
    'apellidoMaterno',
    'tipoDoc',
    'numeroDocumento',
    'estado',
    'acciones',
  ];

  dataSource!: MatTableDataSource<Persona>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private personaService: PersonaService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
  ) {}

  ngOnInit(): void {
    this.cargarPersonas();
  }

  // --- Carga de Datos ---
  cargarPersonas() {
    this.personaService.getAll().subscribe({
      next: (data) => {
        this.dataSource = new MatTableDataSource(data);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: (err) => {
        console.error('Error al cargar personas:', err);
        this.mostrarMensaje('Error al conectar con el servidor', 'error');
      },
    });
  }

  // --- Filtro ---
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    if (this.dataSource) {
      this.dataSource.filter = filterValue.trim().toLowerCase();
      if (this.dataSource.paginator) {
        this.dataSource.paginator.firstPage();
      }
    }
  }

  // --- Modal ---
  abrirModal(persona?: Persona) {
    const dialogRef = this.dialog.open(PersonaDetailComponent, {
      width: '900px',
      disableClose: true,
      data: persona || null, // Si hay persona es Edición, si no es Creación
    });

    dialogRef.afterClosed().subscribe((formularioResult) => {
      // formularioResult ahora contiene todos los datos + documentoRespaldo
      if (formularioResult) {
        this.procesarGuardado(formularioResult);
      }
    });
  }

  /**
   * Procesa la respuesta del modal y decide si llamar a Create o Update
   * usando los nuevos DTOs.
   */
  procesarGuardado(datosFormulario: any) {

    // CASO: EDITAR
    if (datosFormulario.idPersona && datosFormulario.idPersona > 0) {

      // Mapeamos al DTO de actualización
      // Como el formulario tiene los mismos nombres de campos que el DTO, usamos spread operator (...)
      const dto: PersonaActualizarDto = {
        ...datosFormulario
        // Aquí ya viaja 'documentoRespaldo' que viene del input del usuario en el dialog
      };

      this.personaService.update(dto).subscribe({
        next: () => {
          this.mostrarMensaje('Persona actualizada correctamente');
          this.cargarPersonas();
        },
        error: (err) => this.mostrarMensaje('Error al actualizar: ' + err.message, 'error'),
      });

    }
    // CASO: CREAR
    else {

      // Mapeamos al DTO de creación
      const dto: PersonaCrearDto = {
        ...datosFormulario
        // Aquí ya viaja 'documentoRespaldo' que viene del input del usuario en el dialog
      };

      this.personaService.create(dto).subscribe({
        next: () => {
          this.mostrarMensaje('Persona creada correctamente');
          this.cargarPersonas();
        },
        error: (err) => this.mostrarMensaje('Error al crear: ' + err.message, 'error'),
      });
    }
  }

  // --- Acciones de Botones ---

  // Botón Nuevo (+ PERSONA)
  nuevaPersona() {
    this.abrirModal();
  }

  // Botón Lápiz
  editarPersona(persona: Persona) {
    this.abrirModal(persona);
  }

  eliminarPersona(persona: Persona) {
    this.dialog.open(EliminarModalComponent, {
      width: '400px',
      data: { mensaje: "¿Está seguro de eliminar la persona con documento: " + persona.numeroDocumento + "?" }
    }).afterClosed().subscribe(documento => {
      if (documento) {
        this.personaService.delete(persona.idPersona, documento).subscribe({
          next: () => {
            this.mostrarMensaje('Persona eliminada correctamente');
            this.cargarPersonas();
          },
          error: (err) => this.mostrarMensaje('Error al eliminar: ' + err.message, 'error'),
        });
      }
    });
  }

  // Botón User (Gestionar)
  gestionarUsuario(persona: Persona) {
    this.dialog.open(UsuariosPersonaComponent, {
      width: '95%',
      maxWidth: '1200px',
      height: '85vh',
      panelClass: 'modal-usuarios-wrapper',
      data: persona
    });
  }

  // --- Utilidad ---
  mostrarMensaje(mensaje: string, tipo: 'success' | 'error' = 'success') {
    this.snackBar.open(mensaje, 'Cerrar', {
      duration: 3000,
      panelClass:
        tipo === 'error'
          ? ['bg-danger', 'text-white']
          : ['bg-success', 'text-white'],
    });
  }
}
