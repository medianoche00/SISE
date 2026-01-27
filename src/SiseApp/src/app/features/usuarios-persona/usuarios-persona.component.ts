import { Component, Inject, OnInit } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Persona } from '../../core/models/persona.model';
import { UsuarioAsignado } from '../../core/models/usuario-asignado.model';
import { ExpedienteService } from '../../core/services/expediente.service';
import { UsuarioDetailComponent } from '../../shared/usuario-detail/usuario-detail.component';
import { CredencialesDetailComponent } from '../../shared/credenciales-detail/credenciales-detail.component';
import { EliminarModalComponent } from '../../shared/eliminar-modal/eliminar-modal.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AdministrativoService } from '../../core/services/administrativo.service';
import { EgresadoService } from '../../core/services/egresado.service';
import { RepresentanteService } from '../../core/services/representante.service';

@Component({
  selector: 'app-usuarios-persona',
  templateUrl: './usuarios-persona.component.html',
  styleUrls: ['./usuarios-persona.component.scss'],
})
export class UsuariosPersonaComponent implements OnInit {
  // Datos de la persona (Panel Izquierdo)
  persona: Persona;

  // Tabla (Panel Derecho)
  displayedColumns: string[] = [
    'usuario',
    'contexto',
    'rol',
    'estado',
    'acciones',
    //correoElectronico
  ];
  dataSource!: MatTableDataSource<UsuarioAsignado>;
  isLoading = true;

  constructor(
    public dialogRef: MatDialogRef<UsuariosPersonaComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Persona, // Recibimos la persona seleccionada
    private expedienteService: ExpedienteService,
    private administrativoService: AdministrativoService,
    private egresadoService: EgresadoService,
    private respresentanteService: RepresentanteService,
    private dialog: MatDialog, // Para abrir los futuros modales
    private snackBar: MatSnackBar
  ) {
    this.persona = data;
  }

  ngOnInit(): void {
    this.cargarUsuarios();
  }

  cargarUsuarios() {
    this.isLoading = true;
    this.expedienteService
      .getUsuariosPorPersona(this.persona.idPersona)
      .subscribe({
        next: (data) => {
          this.dataSource = new MatTableDataSource(data);
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error al cargar usuarios asignados:', err);
          this.isLoading = false;
        },
      });
  }

  cerrarModal() {
    this.dialogRef.close();
  }

  editarUsuario(usuarioRow: UsuarioAsignado) {
    const dialogRef = this.dialog.open(UsuarioDetailComponent, {
      width: '650px',
      disableClose: true,
      data: {
        persona: this.persona,
        usuario: {
          idUsuario: usuarioRow.idUsuario,
          rol: usuarioRow.rol,
          idEntidad: usuarioRow.idEntidad,
        },
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      // Si devolvió true, significa que guardó cambios
      if (result) {
        this.cargarUsuarios();
      }
    });
  }

  gestionarCredenciales(usuarioRow: UsuarioAsignado) {
    const dialogRef = this.dialog.open(CredencialesDetailComponent, {
      width: '400px',
      disableClose: true,
      data: {
        idUsuario: usuarioRow.idUsuario,
        username: usuarioRow.usuario,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.cargarUsuarios();
      }
    });
  }

  agregarUsuario() {
    const dialogRef = this.dialog.open(UsuarioDetailComponent, {
      width: '650px',
      disableClose: true,
      data: {
        persona: this.persona,
        usuario: null,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.cargarUsuarios();
      }
    });
  }

  eliminarUsuario(usuarioRow: UsuarioAsignado) {
    this.dialog
      .open(EliminarModalComponent, {
        width: '400px',
        data: {
          mensaje:
            '¿Está seguro de eliminar al usuario: ' +
            usuarioRow.usuario +
            ' de ' +
            this.persona.nombres +
            ' ' +
            this.persona.apellidoPaterno +
            '?',
        },
      })
      .afterClosed()
      .subscribe((documento) => {
        if (documento) {
          switch (usuarioRow.rol) {
            case 'Representante':
              {
                this.respresentanteService
                  .eliminar(usuarioRow.idEntidad, documento)
                  .subscribe({
                    next: () => {
                      this.mostrarMensaje(
                        'Usuario representante eliminado correctamente',
                      );
                      this.cargarUsuarios();
                    },
                    error: (err) =>
                      this.mostrarMensaje(
                        'Error al eliminar: ' + err.message,
                        'error',
                      ),
                  });
              }
              break;
            case 'Egresado':
              {
                this.egresadoService
                  .eliminar(usuarioRow.idEntidad, documento)
                  .subscribe({
                    next: () => {
                      this.mostrarMensaje(
                        'Usuario egresado eliminado correctamente',
                      );
                      this.cargarUsuarios();
                    },
                    error: (err) =>
                      this.mostrarMensaje(
                        'Error al eliminar: ' + err.message,
                        'error',
                      ),
                  });
              }
              break;
            case 'Administrativo':
              this.administrativoService
                .eliminar(usuarioRow.idEntidad, documento)
                .subscribe({
                  next: () => {
                    this.mostrarMensaje(
                      'Usuario administrativo eliminado correctamente',
                    );
                    this.cargarUsuarios();
                  },
                  error: (err) =>
                    this.mostrarMensaje(
                      'Error al eliminar: ' + err.message,
                      'error',
                    ),
                });
              break;
          }
        }
      });
  }

  restaurarUsuario(usuarioRow: UsuarioAsignado) {
    // Lógica para restaurar el usuario
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
