import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { Persona } from '../../core/models/persona.model';
import { UbicacionService } from '../../core/services/ubicacion.service';
import { DepartamentoDto, ProvinciaDto, DistritoDto } from '../../core/models/ubicacion.model';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-persona-detail',
  templateUrl: './persona-detail.component.html',
  styleUrls: ['./persona-detail.component.css'],
  standalone: true,
  imports: [
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatInputModule,
    MatOptionModule,
    MatSelectModule
  ],
})
export class PersonaDetailComponent implements OnInit {
  form: FormGroup;
  titulo: string = 'Nueva Persona';

  // Listas de datos
  listaTipoDoc: any[] = [
    { id: 1, nombre: 'DNI' },
    { id: 2, nombre: 'Pasaporte' },
    { id: 3, nombre: 'C.E.' },
  ];

  // Listas para los desplegables de Ubicación
  listaDepartamentos: DepartamentoDto[] = [];
  listaProvincias: ProvinciaDto[] = [];
  listaDistritos: DistritoDto[] = [];

  listaEstados: string[] = ['Activo', 'Eliminado'];

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<PersonaDetailComponent>,
    private ubicacionService: UbicacionService,
    @Inject(MAT_DIALOG_DATA) public data: Persona | null,
  ) {
    // Inicialización del formulario
    this.form = this.fb.group({
      idPersona: [0],
      nombres: ['', Validators.required],
      apellidoPaterno: ['', Validators.required],
      apellidoMaterno: ['', Validators.required],
      idTipoDocumento: [1, Validators.required],
      numeroDocumento: ['', [Validators.required, Validators.minLength(8)]],
      correoPersonal: ['', [Validators.required, Validators.email]],
      telefono: [''],

      // Dirección
      calle: ['', Validators.required],
      numero: ['', Validators.required],
      pisoDepartamento: [''],
      referencia: [''],

      // Ubigeo (Selectores)
      departamento: [null, Validators.required],
      provincia: [null, Validators.required],
      idDistrito: [null, Validators.required],

      estado: ['Activo', Validators.required],
    });
  }

  ngOnInit(): void {
    // 1. Primero cargamos las ubicaciones
    this.cargarUbicaciones();
  }

  cargarUbicaciones() {
    this.ubicacionService.getUbicacionesCompleta().subscribe({
      next: (resp) => {
        this.listaDepartamentos = resp;

        // 2. Si hay data (Edición), llenamos el formulario DESPUÉS de tener las ubicaciones
        if (this.data) {
          this.titulo = 'Editar Persona';
          this.cargarDatosEdicion(this.data);
        }
      },
      error: (err) => console.error('Error cargando ubicaciones', err),
    });
  }

  cargarDatosEdicion(persona: Persona) {
    // Llenamos datos básicos
    this.form.patchValue({
      idPersona: persona.idPersona,
      nombres: persona.nombres,
      apellidoPaterno: persona.apellidoPaterno,
      apellidoMaterno: persona.apellidoMaterno,
      idTipoDocumento: persona.idTipoDocumento,
      numeroDocumento: persona.numeroDocumento,
      correoPersonal: persona.correoPersonal,
      telefono: persona.telefono,
      calle: persona.calle,
      numero: persona.numero,
      pisoDepartamento: persona.pisoDepartamento,
      referencia: persona.referencia,
      estado: persona.estado,
    });

    // Lógica para pre-seleccionar los combos de Ubigeo
    if (persona.idDistrito) {
      this.seleccionarUbicacionPorDistrito(persona.idDistrito);
    }
  }

  /**
   * Busca en el árbol jerárquico a qué provincia y departamento pertenece el distrito
   */
  seleccionarUbicacionPorDistrito(idDistrito: number) {
    // Recorremos Departamentos
    for (const dep of this.listaDepartamentos) {
      // Recorremos Provincias
      for (const prov of dep.provincias) {
        // Buscamos el Distrito
        const distritoEncontrado = prov.distritos.find(
          (d) => d.id === idDistrito,
        );

        if (distritoEncontrado) {
          // ¡Encontrado!
          // 1. Seteamos Departamento y cargamos provincias
          this.form.controls['departamento'].setValue(dep.id);
          this.listaProvincias = dep.provincias;

          // 2. Seteamos Provincia y cargamos distritos
          this.form.controls['provincia'].setValue(prov.id);
          this.listaDistritos = prov.distritos;

          // 3. Seteamos el Distrito final
          this.form.controls['idDistrito'].setValue(idDistrito);
          return; // Terminamos la búsqueda
        }
      }
    }
  }

  // --- Eventos de Cambio en los Selects (Cascada) ---

  alCambiarDepartamento(idDep: number) {
    // Limpiar hijos
    this.form.controls['provincia'].setValue(null);
    this.form.controls['idDistrito'].setValue(null);
    this.listaDistritos = [];

    // Buscar el departamento seleccionado en la lista cargada en memoria
    const depSeleccionado = this.listaDepartamentos.find((d) => d.id === idDep);

    if (depSeleccionado) {
      this.listaProvincias = depSeleccionado.provincias;
    } else {
      this.listaProvincias = [];
    }
  }

  alCambiarProvincia(idProv: number) {
    // Limpiar hijo
    this.form.controls['idDistrito'].setValue(null);

    // Buscar la provincia seleccionada en la lista actual
    const provSeleccionada = this.listaProvincias.find((p) => p.id === idProv);

    if (provSeleccionada) {
      this.listaDistritos = provSeleccionada.distritos;
    } else {
      this.listaDistritos = [];
    }
  }

  guardar() {
    if (this.form.valid) {

      const formValue = this.form.value;
      const tipoDocSeleccionado = this.listaTipoDoc.find(t => t.id === formValue.idTipoDocumento);
      const nombreTipoDoc = tipoDocSeleccionado ? tipoDocSeleccionado.nombre : '';
      //const estadoFinal = this.data ? this.data.estado : 'Activo';

      const personaFinal: Persona = {
        ...formValue,
        nombreTipoDocumento: nombreTipoDoc,
        //estado: estadoFinal
      };

      this.dialogRef.close(personaFinal);

    } else {
      this.form.markAllAsTouched();
    }
  }

  cancelar() {
    this.dialogRef.close();
  }
}
