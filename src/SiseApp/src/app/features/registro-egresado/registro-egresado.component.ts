import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms'; // AGREGADO: FormArray
import { EgresadoService } from '../../core/services/egresado.service';
import { Carrera } from '../../core/models/egresado.model';
import { MatCard } from "@angular/material/card";

@Component({
  selector: 'app-registro-egresado',
  templateUrl: './registro-egresado.component.html',
  styleUrls: ['./registro-egresado.component.css'],
  //imports: [MatCard]
})
export class RegistroEgresadoComponent implements OnInit {

  egresadoForm: FormGroup;
  listaCarreras: Carrera[] = [];
  enviando: boolean = false;

  constructor(
    private fb: FormBuilder,
    private egresadoService: EgresadoService
  ) {
    this.egresadoForm = this.initForm();
  }

  ngOnInit(): void {
    this.cargarCarreras();
  }

  initForm(): FormGroup {
    return this.fb.group({
      datosPersonales: this.fb.group({
        nombres: [{ value: 'JUAN ALBERTO', disabled: true }, Validators.required],
        apellidoPaterno: [{ value: 'PEREZ', disabled: true }, Validators.required],
        apellidoMaterno: [{ value: 'GOMEZ', disabled: true }, Validators.required],
        documentoIdentidad: [{ value: '12345678', disabled: true }, [Validators.required, Validators.minLength(8)]],

        telefono: ['', [Validators.required, Validators.pattern(/^[0-9]{9}$/)]],
        correoPersonal: ['', [Validators.required, Validators.email]]
      }),
      datosAcademicos: this.fb.group({
        idCarrera: ['', Validators.required],
        codigoUniversitario: [{ value: '201802551', disabled: true }, Validators.required], // Bloqueado
        añoEgreso: ['', [Validators.required, Validators.min(1990), Validators.max(2026)]]
      }),
      experienciaLaboral: this.fb.array([])
    });
  }

  get experiencias(): FormArray {
    return this.egresadoForm.get('experienciaLaboral') as FormArray;
  }

  nuevaExperiencia(): FormGroup {
    return this.fb.group({
      empresa: ['', Validators.required],
      cargo: ['', Validators.required],
      fechaInicio: ['', Validators.required],
      fechaFin: [''],
      actualmente: [false]
    });
  }

  agregarExperiencia() {
    this.experiencias.push(this.nuevaExperiencia());
  }

  eliminarExperiencia(indice: number) {
    this.experiencias.removeAt(indice);
  }

  cargarCarreras() {
    this.egresadoService.obtenerCarreras().subscribe({
      next: (data: Carrera[]) => {
        this.listaCarreras = data;
      },
      error: (err: any) => {
        console.error('Error cargando carreras', err);
      }
    });
  }

  onSubmit() {
    if (this.egresadoForm.invalid) {
      this.egresadoForm.markAllAsTouched();
      return;
    }

    this.enviando = true;

    const formValue = this.egresadoForm.getRawValue();

    const payload = {
      ...formValue.datosPersonales,
      ...formValue.datosAcademicos,
      idCarrera: Number(formValue.datosAcademicos.idCarrera),
      experienciaLaboral: formValue.experienciaLaboral
    };

    console.log('Enviando payload:', payload); 

    this.egresadoService.completarPerfil(payload).subscribe({
      next: (res: any) => {
        this.enviando = false;
        alert('¡Información actualizada con éxito!');
      },
      error: (err: any) => {
        this.enviando = false;
        console.error(err);
        const mensaje = err.error?.message || 'Ocurrió un error inesperado';
        alert('Error: ' + mensaje);
      }
    });
  }

  esInvalido(grupo: string, campo: string): boolean {
    const control = this.egresadoForm.get(grupo)?.get(campo);
    return control ? control.invalid && control.touched : false;
  }
}
