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
import { MatIconModule } from '@angular/material/icon'; // Agregado para el icono opcional

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
    MatSelectModule,
    MatIconModule
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

  listaDepartamentos: DepartamentoDto[] = [];
  listaProvincias: ProvinciaDto[] = [];
  listaDistritos: DistritoDto[] = [];
  //listaEstados: string[] = ['Activo', 'Eliminado'];

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

      // Ubigeo
      departamento: [null, Validators.required],
      provincia: [null, Validators.required],
      idDistrito: [null, Validators.required],

      estado: ['Activo', Validators.required],

      // NUEVO CAMPO: Documento de Respaldo (Obligatorio siempre)
      documentoRespaldo: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.cargarUbicaciones();
  }

  cargarUbicaciones() {
    this.ubicacionService.getUbicacionesCompleta().subscribe({
      next: (resp) => {
        this.listaDepartamentos = resp;

        if (this.data) {
          this.titulo = 'Editar Persona';
          this.cargarDatosEdicion(this.data);
        }
      },
      error: (err) => console.error('Error cargando ubicaciones', err),
    });
  }

  cargarDatosEdicion(persona: Persona) {
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
      //estado: persona.estado,
      documentoRespaldo: ''
    });

    if (persona.idDistrito) {
      this.seleccionarUbicacionPorDistrito(persona.idDistrito);
    }
  }

  seleccionarUbicacionPorDistrito(idDistrito: number) {
    for (const dep of this.listaDepartamentos) {
      for (const prov of dep.provincias) {
        const distritoEncontrado = prov.distritos.find((d) => d.id === idDistrito);
        if (distritoEncontrado) {
          this.form.controls['departamento'].setValue(dep.id);
          this.listaProvincias = dep.provincias;
          this.form.controls['provincia'].setValue(prov.id);
          this.listaDistritos = prov.distritos;
          this.form.controls['idDistrito'].setValue(idDistrito);
          return;
        }
      }
    }
  }

  alCambiarDepartamento(idDep: number) {
    this.form.controls['provincia'].setValue(null);
    this.form.controls['idDistrito'].setValue(null);
    this.listaDistritos = [];
    const depSeleccionado = this.listaDepartamentos.find((d) => d.id === idDep);
    this.listaProvincias = depSeleccionado ? depSeleccionado.provincias : [];
  }

  alCambiarProvincia(idProv: number) {
    this.form.controls['idDistrito'].setValue(null);
    const provSeleccionada = this.listaProvincias.find((p) => p.id === idProv);
    this.listaDistritos = provSeleccionada ? provSeleccionada.distritos : [];
  }

  guardar() {
    if (this.form.valid) {
      const formValue = this.form.value;
      const tipoDocSeleccionado = this.listaTipoDoc.find(t => t.id === formValue.idTipoDocumento);
      const nombreTipoDoc = tipoDocSeleccionado ? tipoDocSeleccionado.nombre : '';

      // Retornamos todo el objeto, incluyendo el documentoRespaldo
      const dataFinal = {
        ...formValue,
        nombreTipoDocumento: nombreTipoDoc
      };

      this.dialogRef.close(dataFinal);
    } else {
      this.form.markAllAsTouched();
    }
  }

  cancelar() {
    this.dialogRef.close();
  }
}
