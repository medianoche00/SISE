import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar'; // Opcional para notificaciones

// Importaciones de tus archivos
import { Persona } from '../../../core/models/persona.model';
import { PersonaService } from '../../../core/services/persona.service';
import { PersonaDetailComponent } from '../../../shared/persona-detail/persona-detail.component';
import { RouterLink } from '@angular/router';
import { UsuariosPersonaComponent } from '../../usuarios-persona/usuarios-persona.component';

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
    private snackBar: MatSnackBar, // Para mostrar mensajes de éxito
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

  abrirModal(persona?: Persona) {
    const dialogRef = this.dialog.open(PersonaDetailComponent, {
      width: '900px',
      disableClose: true,
      data: persona || null, // Si hay persona es Edición, si no es Creación
    });

    dialogRef.afterClosed().subscribe((formularioResult) => {
      if (formularioResult) {
        this.procesarGuardado(formularioResult);
      }
    });
  }

  procesarGuardado(datosPersona: Persona) {
    // Definimos el documento de respaldo
    const docRespaldo = 'DOC-SISTEMA-' + new Date().getTime(); //! cambiar por un dialogo para ingresar el doc

    if (datosPersona.idPersona && datosPersona.idPersona > 0) {
      // EDITAR
      this.personaService.update(datosPersona, docRespaldo).subscribe({
        next: (resp) => {
          this.mostrarMensaje('Persona actualizada correctamente');
          this.cargarPersonas(); // Recargar tabla
        },
        error: (err) => this.mostrarMensaje('Error al actualizar:' + err, 'error'),
      });
    } else {
      // CREAR
      this.personaService.create(datosPersona, docRespaldo).subscribe({
        next: (resp) => {
          this.mostrarMensaje('Persona creada correctamente');
          this.cargarPersonas(); // Recargar tabla
        },
        error: (err) => this.mostrarMensaje('Error al crear' + err, 'error'),
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
