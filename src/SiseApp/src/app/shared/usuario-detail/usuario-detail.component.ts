import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RolService } from '../../core/services/rol.service';
import { RolOpcion } from '../../core/models/rol.model';
import { Persona } from '../../core/models/persona.model';

@Component({
  selector: 'app-usuario-detail',
  templateUrl: './usuario-detail.component.html',
  styleUrls: ['./usuario-detail.component.css'],
})
export class UsuarioDetailComponent implements OnInit {
  form: FormGroup;
  titulo: string = 'Nuevo Usuario';
  isEditMode: boolean = false;
  idPersona: number;

  // Listas desplegables (Deberían venir de un MaestroService, aquí simuladas)
  listaRoles: RolOpcion[] = [
    { codigo: 'Egresado', nombre: 'Egresado' },
    { codigo: 'Representante', nombre: 'Representante de Empresa' },
    { codigo: 'Administrativo', nombre: 'Administrativo' },
    { codigo: 'Administrador', nombre: 'Administrador del Sistema' },
  ];

  // Listas simuladas para llenar los combos específicos
  listaCarreras = [
    { id: 1, nombre: 'Ing. Sistemas' },
    { id: 2, nombre: 'Marketing' },
  ];
  listaEmpresas = [
    { id: 1, nombre: 'Microsoft' },
    { id: 2, nombre: 'Google' },
  ];
  listaCargos = [
    { id: 1, nombre: 'Secretaria' },
    { id: 2, nombre: 'Jefe de Area' },
  ];
  listaDepartamentos = [
    { id: 1, nombre: 'Lima' },
    { id: 2, nombre: 'Arequipa' },
  ]; // Para administrativo

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<UsuarioDetailComponent>,
    private rolService: RolService,
    // data puede traer { persona: Persona, usuario: UsuarioAsignado (si es edit) }
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) {
    this.idPersona = data.persona.idPersona;

    // Si viene 'usuario' en la data, es edición
    if (data.usuario) {
      this.isEditMode = true;
      this.titulo = `Editar Usuario (${data.usuario.usuario})`;
    }

    this.form = this.fb.group({
      // Campo común y selector principal
      rol: ['', Validators.required],
      documentoRespaldo: ['', Validators.required],

      // Campos de Identity (Solo requeridos si es NUEVO)
      username: [''],
      email: ['', [Validators.email]],
      password: [''],

      // Campos Egresado
      idCarrera: [null],
      codigoUniversitario: [''],
      anioEgreso: [null],

      // Campos Representante
      idEmpresa: [null],
      cargoRepresentante: [''],

      // Campos Administrativo/Admin
      idCargoAdministrativo: [null],
      idDepartamento: [null], // Agregado por cumplir DTO
    });
  }

  ngOnInit(): void {
    // Escuchar cambios en el Rol para activar/desactivar validaciones
    this.form.get('rol')?.valueChanges.subscribe((rol) => {
      this.actualizarValidaciones(rol);
    });

    // Validaciones iniciales para Identity (Solo si es nuevo)
    if (!this.isEditMode) {
      this.form.get('username')?.setValidators(Validators.required);
      this.form
        .get('email')
        ?.setValidators([Validators.required, Validators.email]);
      this.form.get('password')?.setValidators(Validators.required);
    } else {
      // Si es editar, deshabilitamos el rol porque usualmente no se cambia el rol base al editar,
      // o cargamos los datos existentes.
      // TODO: Aquí cargarías this.form.patchValue(data.usuario)
      // Como tu API actual de edición no fue provista, nos enfocamos en CREAR.
      this.form.get('rol')?.disable();
    }
  }

  actualizarValidaciones(rol: string) {
    // 1. Limpiar validadores específicos primero
    const controles = [
      'idCarrera',
      'codigoUniversitario',
      'anioEgreso',
      'idEmpresa',
      'cargoRepresentante',
      'idCargoAdministrativo',
      'idDepartamento',
    ];
    controles.forEach((c) => {
      this.form.get(c)?.clearValidators();
      this.form.get(c)?.updateValueAndValidity();
    });

    // 2. Asignar validadores según rol
    if (rol === 'Egresado') {
      this.establecerRequerido('idCarrera');
      this.establecerRequerido('codigoUniversitario');
      this.establecerRequerido('anioEgreso');
    } else if (rol === 'Representante') {
      this.establecerRequerido('idEmpresa');
      // Cargo es opcional en la BD (nullable), pero si quieres obligatorio descomenta:
      // this.establecerRequerido('cargoRepresentante');
    } else if (rol === 'Administrativo' || rol === 'Administrador') {
      this.establecerRequerido('idCargoAdministrativo');
      this.establecerRequerido('idDepartamento');
    }
  }

  establecerRequerido(nombreControl: string) {
    this.form.get(nombreControl)?.setValidators(Validators.required);
    this.form.get(nombreControl)?.updateValueAndValidity();
  }

  guardar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const val = this.form.value;
    // Si estamos editando y el rol está disabled, lo sacamos de getRawValue()
    const rolSeleccionado = this.form.getRawValue().rol;

    // Lógica de guardado (Switch API)
    if (!this.isEditMode) {
      this.crearUsuario(rolSeleccionado, val);
    } else {
      // TODO: Lógica de Update cuando tengas el endpoint
      console.log('Edición no implementada aún en API');
      this.dialogRef.close(true);
    }
  }

  crearUsuario(rol: string, val: any) {
    // Objeto base común
    const baseIdentity = {
      idPersona: this.idPersona,
      documentoRespaldo: val.documentoRespaldo,
      username: val.username,
      email: val.email,
      password: val.password,
    };

    switch (rol) {
      case 'Egresado':
        const dtoEgresado = {
          ...baseIdentity,
          idCarrera: val.idCarrera,
          codigoUniversitario: val.codigoUniversitario,
          anioEgreso: val.anioEgreso,
        };
        this.rolService.registrarEgresado(dtoEgresado).subscribe({
          next: () => this.dialogRef.close(true),
          error: (e) => alert(e.message), // Manejar error mejor con SnackBar
        });
        break;

      case 'Representante':
        const dtoRep = {
          ...baseIdentity,
          idEmpresa: val.idEmpresa,
          cargo: val.cargoRepresentante,
        };
        this.rolService.registrarRepresentante(dtoRep).subscribe({
          next: () => this.dialogRef.close(true),
          error: (e) => alert(e.message),
        });
        break;

      case 'Administrativo':
      case 'Administrador':
        const dtoAdmin = {
          ...baseIdentity,
          idCargoAdministrativo: val.idCargoAdministrativo,
          idDepartamento: val.idDepartamento || 0, // Manejo de nulos por si acaso
        };

        if (rol === 'Administrador') {
          this.rolService.registrarAdministrador(dtoAdmin).subscribe({
            next: () => this.dialogRef.close(true),
            error: (e) => alert(e.message),
          });
        } else {
          this.rolService.registrarAdministrativo(dtoAdmin).subscribe({
            next: () => this.dialogRef.close(true),
            error: (e) => alert(e.message),
          });
        }
        break;
    }
  }

  cancelar() {
    this.dialogRef.close();
  }
}
