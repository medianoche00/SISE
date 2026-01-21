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
    private dialog: MatDialog, // Para abrir los futuros modales
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

  // --- Acciones de la Tabla ---

  // 1. Icono Lápiz -> Editar Usuario
  editarUsuario(usuario: UsuarioAsignado) {
    console.log('Abriendo modal UsuarioDetail (Editar) para:', usuario.usuario);

    /* TODO: Descomentar cuando crees el componente
    const dialogRef = this.dialog.open(UsuarioDetailComponent, {
      width: '600px',
      data: usuario // Pasamos el usuario a editar
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) this.cargarUsuarios(); // Recargar si hubo cambios
    });
    */
  }

  // 2. Icono Llave -> Gestionar Credenciales
  gestionarCredenciales(usuario: UsuarioAsignado) {
    console.log('Abriendo modal CredencialesDetail para:', usuario.usuario);

    /* TODO: Descomentar cuando crees el componente
    const dialogRef = this.dialog.open(CredencialesDetailComponent, {
      width: '500px',
      data: usuario
    });
    */
  }

  agregarUsuario() {
    const dialogRef = this.dialog.open(UsuarioDetailComponent, {
      width: '650px',
      disableClose: true,
      data: {
        persona: this.persona, // IMPORTANTE: pasar la persona completa
        usuario: null,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.cargarUsuarios();
      }
    });
  }
}
