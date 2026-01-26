import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { forkJoin, Observable } from 'rxjs';

// Services
import { CatalogosService } from '../../core/services/catalogos.service';
import { EgresadoService } from '../../core/services/egresado.service';
import { RepresentanteService } from '../../core/services/representante.service';
import { AdministrativoService } from '../../core/services/administrativo.service';

// Models
import { Persona } from '../../core/models/persona.model';
import {
  Carrera,
  CargoAdministrativo,
  Empresa,
  Rol,
} from '../../core/models/catalogos.model';
import {
  EgresadoCrearDto,
  EgresadoActualizarDto,
} from '../../core/models/egresado.model';
import {
  RepresentanteCrearDto,
  RepresentanteActualizarDto,
} from '../../core/models/representante.model';
import {
  AdministrativoCrearDto,
  AdministrativoActualizarDto,
} from '../../core/models/administrativo.model';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';

// Interfaz auxiliar para lo que recibe el modal
export interface UsuarioModalData {
  persona: Persona;
  usuario?: {
    idUsuario: number;
    rol: string;
    idEntidad: number; // idEgresado, idAdministrativo, etc.
  };
}

@Component({
  selector: 'app-usuario-detail',
  templateUrl: './usuario-detail.component.html',
  styleUrls: ['./usuario-detail.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
  ],
})
export class UsuarioDetailComponent implements OnInit {
  form: FormGroup;
  titulo: string = 'Nuevo Usuario';
  isEditMode: boolean = false;
  isLoading: boolean = true;
  rolNoSoportado: boolean = false;

  // Datos recibidos
  persona: Persona;
  usuarioEdicion: any | null = null;

  // Catalogos
  listaRoles: Rol[] = [];
  listaCarreras: Carrera[] = [];
  listaCargos: CargoAdministrativo[] = [];
  listaEmpresas: Empresa[] = [];

  // Constantes de Roles
  readonly ROL_EGRESADO = 'Egresado';
  readonly ROL_REPRESENTANTE = 'Representante';
  readonly ROL_ADMINISTRATIVO = 'Administrativo';
  readonly ROL_ADMINISTRADOR = 'Administrador';

  readonly ESTADOS_EGRESADO = [
    'Buscando Trabajo',
    'Trabajando',
    'Estudiando',
    'Inactivo',
    'Eliminado',
  ];
  readonly ESTADOS_ADMINISTRATIVO = ['Activo', 'Eliminado'];
  readonly ESTADOS_REPRESENTANTE = ['Activo', 'Eliminado'];

  estadosDisponibles: string[] = [];

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<UsuarioDetailComponent>,
    private catalogosService: CatalogosService,
    private egresadoService: EgresadoService,
    private representanteService: RepresentanteService,
    private administrativoService: AdministrativoService,
    @Inject(MAT_DIALOG_DATA) public data: UsuarioModalData,
  ) {
    this.persona = data.persona;
    this.usuarioEdicion = data.usuario;
    this.isEditMode = !!data.usuario;

    if (this.isEditMode) {
      this.titulo = `Editar ${this.usuarioEdicion.rol}`;
    }

    this.form = this.fb.group({
      idRol: ['', Validators.required],
      documentoRespaldo: ['', Validators.required],

      // Credenciales (Solo CREATE)
      username: [''],
      email: ['', [Validators.email]],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.pattern(/^(?=.*[a-z])(?=.*\d).+$/),
        ],
      ],

      // Campos Egresado
      idCarrera: [null],
      codigoUniversitario: [''],
      anioEgreso: [null],

      // Campos Representante
      idEmpresa: [null],
      cargoRepresentante: [''], // Mapeado a 'cargo' o 'cargoRepresentante'

      // Campos Administrativo
      idCargoAdministrativo: [null],

      estado: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.cargarCatalogos();
    this.setupValidations();
  }

  // 1. CARGA DE CATALOGOS
  cargarCatalogos() {
    this.isLoading = true;
    forkJoin({
      roles: this.catalogosService.getRoles(),
      carreras: this.catalogosService.getCarreras(),
      cargos: this.catalogosService.getCargosAdministrativos(),
      empresas: this.catalogosService.getEmpresas(),
    }).subscribe({
      next: (res) => {
        this.listaRoles = res.roles;
        this.listaCarreras = res.carreras;
        this.listaCargos = res.cargos;
        this.listaEmpresas = res.empresas;

        this.isLoading = false;

        if (this.isEditMode) {
          this.cargarDatosEdicion();
        } else {
          // Validadores Identity para CREAR
          this.form.get('username')?.setValidators(Validators.required);
          this.form
            .get('email')
            ?.setValidators([Validators.required, Validators.email]);
          this.form.get('password')?.setValidators(Validators.required);
        }
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      },
    });
  }

  // 2. VALIDACIONES DINAMICAS
  setupValidations() {
    this.form.get('idRol')?.valueChanges.subscribe((idRol) => {
      this.actualizarCamposSegunRol(idRol);
    });
  }

  obtenerNombreRol(idRol: number): string {
    const rol = this.listaRoles.find((r) => r.idRol === idRol);
    return rol ? rol.nombreRol : '';
  }

  actualizarCamposSegunRol(idRol: number) {
    const nombreRol = this.obtenerNombreRol(idRol);
    this.rolNoSoportado = false;

    // Limpiar validadores previos
    const controles = [
      'idCarrera',
      'codigoUniversitario',
      'anioEgreso',
      'idEmpresa',
      'cargoRepresentante',
      'idCargoAdministrativo',
    ];
    controles.forEach((c) => {
      this.form.get(c)?.clearValidators();
      this.form.get(c)?.updateValueAndValidity();
    });

    // Asignar nuevos
    switch (nombreRol) {
      case this.ROL_EGRESADO:
        this.establecerRequerido([
          'idCarrera',
          'codigoUniversitario',
          'anioEgreso',
        ]);
        break;
      case this.ROL_REPRESENTANTE:
        this.establecerRequerido(['idEmpresa']);
        // Cargo es opcional
        break;
      case this.ROL_ADMINISTRATIVO:
      case this.ROL_ADMINISTRADOR:
        this.establecerRequerido(['idCargoAdministrativo']);
        break;
      default:
        this.rolNoSoportado = true;
    }

    this.estadosDisponibles = []; // Reiniciar

    switch (nombreRol) {
      case this.ROL_EGRESADO:
        this.establecerRequerido([
          'idCarrera',
          'codigoUniversitario',
          'anioEgreso',
        ]);

        // logica de estados por rol
        this.estadosDisponibles = this.ESTADOS_EGRESADO;

        if (!this.isEditMode) {
          this.form.get('estado')?.setValue('Buscando Trabajo');
        }
        break;

      case this.ROL_REPRESENTANTE:
        this.establecerRequerido(['idEmpresa']);

        this.estadosDisponibles = this.ESTADOS_REPRESENTANTE;
        if (!this.isEditMode) {
          this.form.get('estado')?.setValue('Activo');
        }
        break;

      case this.ROL_ADMINISTRATIVO:
      case this.ROL_ADMINISTRADOR:
        this.establecerRequerido(['idCargoAdministrativo']);

        this.estadosDisponibles = this.ESTADOS_ADMINISTRATIVO;
        if (!this.isEditMode) {
          this.form.get('estado')?.setValue('Activo');
        }
        break;

      default:
        this.rolNoSoportado = true;
        this.form.get('estado')?.setValue('');
    }
  }

  establecerRequerido(campos: string[]) {
    campos.forEach((c) => {
      this.form.get(c)?.setValidators(Validators.required);
      this.form.get(c)?.updateValueAndValidity();
    });
  }

  // LOGICA DE EDICIÓN (GET DATA)
  cargarDatosEdicion() {
    if (!this.usuarioEdicion) return;

    // Buscamos el ID del rol en base al nombre que viene en la tabla
    const rolObj = this.listaRoles.find(
      (r) => r.nombreRol === this.usuarioEdicion.rol,
    );
    if (rolObj) {
      this.form.get('idRol')?.setValue(rolObj.idRol);
    }

    this.form.get('idRol')?.disable();
    this.form.get('username')?.disable();
    this.form.get('email')?.disable();
    this.form.get('password')?.disable();

    const nombreRol = this.usuarioEdicion.rol;
    const idEntidad = this.usuarioEdicion.idEntidad; // ID Específico (EgresadoID, etc.)

    this.isLoading = true;
    let request$: import('rxjs').Observable<any> | undefined;

    switch (nombreRol) {
      case this.ROL_EGRESADO:
        request$ = this.egresadoService.getPorId(idEntidad);
        break;
      case this.ROL_REPRESENTANTE:
        request$ = this.representanteService.getPorId(idEntidad);
        break;
      case this.ROL_ADMINISTRATIVO:
      case this.ROL_ADMINISTRADOR:
        request$ = this.administrativoService.getPorId(idEntidad);
        break;
    }

    if (request$) {
      request$.subscribe({
        next: (data: any) => {
          this.form.patchValue(data);
          // Mapeos manuales si los nombres difieren
          if (nombreRol === this.ROL_REPRESENTANTE && data.cargo) {
            this.form.get('cargoRepresentante')?.setValue(data.cargo);
          }
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
          this.cancelar();
        },
      });
    } else {
      this.isLoading = false;
    }
  }

  // GUARDAR (CREATE & UPDATE)
  guardar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const val = this.form.getRawValue();
    const nombreRol = this.obtenerNombreRol(val.idRol);
    let request$;

    // === MODO CREACIÓN ===
    if (!this.isEditMode) {
      // Datos base de identidad
      const baseIdentity = {
        username: val.username,
        email: val.email,
        password: val.password,
        documentoRespaldo: val.documentoRespaldo,
        idPersona: this.persona.idPersona,
      };

      switch (nombreRol) {
        case this.ROL_EGRESADO:
          const dtoEg: EgresadoCrearDto = {
            ...baseIdentity,
            idCarrera: val.idCarrera,
            codigoUniversitario: val.codigoUniversitario,
            anioEgreso: val.anioEgreso,
          };
          request$ = this.egresadoService.registrar(dtoEg);
          break;

        case this.ROL_REPRESENTANTE:
          const dtoRep: RepresentanteCrearDto = {
            ...baseIdentity,
            idEmpresa: val.idEmpresa,
            cargo: val.cargoRepresentante,
          };
          request$ = this.representanteService.registrar(dtoRep);
          break;

        case this.ROL_ADMINISTRATIVO:
          const dtoAdmin: AdministrativoCrearDto = {
            ...baseIdentity,
            idCargoAdministrativo: val.idCargoAdministrativo,
          };
          request$ =
            this.administrativoService.registrarAdministrativo(dtoAdmin);
          break;

        case this.ROL_ADMINISTRADOR:
          const dtoAdminSys: AdministrativoCrearDto = {
            ...baseIdentity,
            idCargoAdministrativo: val.idCargoAdministrativo,
          };
          request$ =
            this.administrativoService.registrarAdministrador(dtoAdminSys);
          break;
      }
    }

    // === MODO EDICIÓN ===
    else {
      const idEntidad = this.usuarioEdicion.idEntidad;
      const docRespaldo = val.documentoRespaldo;
      const estadoActual = val.estado;;

      switch (nombreRol) {
        case this.ROL_EGRESADO:
          const updateEg: EgresadoActualizarDto = {
            idEgresado: idEntidad,
            idCarrera: val.idCarrera,
            anioEgreso: val.anioEgreso,
            codigoUniversitario: val.codigoUniversitario,
            documentoRespaldo: docRespaldo,
            estado: estadoActual,
          };
          request$ = this.egresadoService.actualizar(updateEg);
          break;

        case this.ROL_REPRESENTANTE:
          const updateRep: RepresentanteActualizarDto = {
            idRepresentante: idEntidad,
            idEmpresa: val.idEmpresa,
            cargo: val.cargoRepresentante,
            documentoRespaldo: docRespaldo,
            estado: estadoActual,
          };
          request$ = this.representanteService.actualizar(updateRep);
          break;

        case this.ROL_ADMINISTRATIVO:
        case this.ROL_ADMINISTRADOR:
          const updateAdmin: AdministrativoActualizarDto = {
            idAdministrativo: idEntidad,
            idCargoAdministrativo: val.idCargoAdministrativo,
            documentoRespaldo: docRespaldo,
            estado: estadoActual,
          };
          request$ = this.administrativoService.actualizar(updateAdmin);
          break;
      }
    }

    // Ejecutar Petición
    if (request$) {
      this.isLoading = true; // Bloquear botón
      request$.subscribe({
        next: () => {
          this.dialogRef.close(true); // Cerrar y notificar éxito
        },
        error: (err) => {
          this.isLoading = false;
          alert(
            'Error: ' + (err.error?.message || 'Ocurrió un error inesperado.'),
          );
        },
      });
    }
  }

  cancelar() {
    this.dialogRef.close();
  }
}
