import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { UsuarioService } from '../../../core/services/usuario.service';
import { Usuario } from '../../../core/models/usuario.interface';

@Component({
  selector: 'app-usuarios-list',
  templateUrl: './usuarios-list.component.html',
  styleUrls: ['./usuarios-list.component.css']
})
export class UsuariosListComponent implements OnInit {

  // Fuente de datos para la tabla y filtro
  dataSource = new MatTableDataSource<Usuario>([]);
  cargando: boolean = true;

  // Variables del Modal
  mostrarModal: boolean = false;
  mostrarPassword: boolean = false;
  esEdicion: boolean = false; // <--- Bandera para saber si editamos

  // Objeto del formulario
  nuevoUsuario: any = {
    id: 0,
    email: '',
    password: '',
    rol: 'Estudiante',
    dni: '',
    nombres: '',      // Asegúrate de usar nombres que coincidan con tu HTML
    apellidoPaterno: '',
    apellidoMaterno: '',
    idCarrera: ''
  };

  constructor(private usuarioService: UsuarioService) { }

  ngOnInit(): void {
    this.cargarUsuarios();
  }

  cargarUsuarios() {
    this.cargando = true;
    this.usuarioService.getAll().subscribe({
      next: (data) => {
        this.dataSource.data = data;
        this.cargando = false;
      },
      error: (e) => {
        console.error(e);
        this.cargando = false;
      }
    });
  }

  filtrar(event: Event) {
    const valor = (event.target as HTMLInputElement).value;
    this.dataSource.filter = valor.trim().toLowerCase();
  }

  abrirModalCrear() {
    this.esEdicion = false;
    this.reiniciarFormulario();
    this.mostrarModal = true;
  }

  abrirModalEditar(usuario: Usuario) {
    this.esEdicion = true;
    this.nuevoUsuario = {
      id: usuario.id,
      email: usuario.nombreUsuario,
      password: '',
      rol: usuario.rol,
      dni: usuario.dni || '',
      nombres: usuario.nombres || '',
      apellidoPaterno: usuario.apellidoPaterno || '',
      apellidoMaterno: usuario.apellidoMaterno || '',
    };
    this.mostrarModal = true;
  }

  cerrarModal() {
    this.mostrarModal = false;
  }

  reiniciarFormulario() {
    this.nuevoUsuario = {
      id: 0,
      email: '',
      password: '',
      rol: 'Estudiante',
      dni: '',
      nombres: '',
      apellidoPaterno: '',
      apellidoMaterno: '',
      idCarrera: ''
    };
  }

  guardarUsuario() {
    if (this.esEdicion) {
      console.log('Editando usuario:', this.nuevoUsuario);
      // Lógica de Update: this.usuarioService.update(this.nuevoUsuario)...
    } else {
      console.log('Creando usuario:', this.nuevoUsuario);
      // Lógica de Create: this.usuarioService.create(this.nuevoUsuario)...
    }
    this.cerrarModal();
    // this.cargarUsuarios(); // Recargar tabla
  }

  cambiarEstado(usuario: Usuario) {
    usuario.activo = !usuario.activo;
    // Llamar al servicio...
  }
}
