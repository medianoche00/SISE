import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { UsuarioService } from '../../../core/services/usuario.service';
import { RoleAssignmentComponent } from '../../../shared/role-assignment/role-assignment.component';
import { Persona } from '../../../core/models/persona.model';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-usuarios-list',
  templateUrl: './usuarios-list.component.html',
  styleUrls: ['./usuarios-list.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatSelectModule
  ]
})
export class UsuariosListComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['username', 'email', 'rol', 'estado', 'acciones'];

  dataSource = new MatTableDataSource<any>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  cargando: boolean = true;

  datosOriginales: any[] = [
    {
      id: 1,
      username: 'admin',
      email: 'admin@sise.com',
      rol: 'Administrador',
      activo: true,
    },
    {
      id: 2,
      username: 'juan.perez',
      email: 'juan@mail.com',
      rol: 'Egresado',
      activo: true,
    },
    {
      id: 3,
      username: 'maria.gerente',
      email: 'maria@empresa.com',
      rol: 'Representante',
      activo: true,
    },
    {
      id: 4,
      username: 'rector',
      email: 'rector@sise.com',
      rol: 'Administrativo',
      activo: true,
    },
  ];

  constructor(
    private usuarioService: UsuarioService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.cargarUsuarios();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  cargarUsuarios() {
    this.cargando = true;
    this.dataSource.data = this.datosOriginales;
    this.cargando = false;
  }

  aplicarFiltro(event: Event) {
    const valor = (event.target as HTMLInputElement).value;
    this.dataSource.filter = valor.trim().toLowerCase();
  }

  filtrarPorRol(rol: string) {
    if (!rol) {
      this.dataSource.data = this.datosOriginales;
    } else {
      this.dataSource.data = this.datosOriginales.filter(u => u.rol === rol);
    }

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  abrirModalCrear() {
    const nuevaPersona: Persona = {
      idPersona: 0,
      nombres: '',
      apellidoPaterno: '',
      apellidoMaterno: '',
      nombreTipoDocumento: '',
      numeroDocumento: '',
      estado: 'Activo',
      idTipoDocumento: 0,
      telefono: null,
      correoPersonal: '',
      idDireccion: 0,
      idDistrito: 0,
      calle: '',
      numero: '',
      pisoDepartamento: null,
      referencia: null
    };

    const dialogRef = this.dialog.open(RoleAssignmentComponent, {
      width: '600px',
      disableClose: true,
      data: { persona: nuevaPersona }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Usuario creado, recargando lista...');
        alert('Usuario creado correctamente (Simulación)');
      }
    });
  }

  cambiarEstado(usuario: any) {
    usuario.activo = !usuario.activo;
    console.log('Nuevo estado:', usuario.activo);
  }
}
