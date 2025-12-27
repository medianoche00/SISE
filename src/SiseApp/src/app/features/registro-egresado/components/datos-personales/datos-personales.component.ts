import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EgresadoService } from '../../../../core/services/egresado.service';
import { Carrera } from '../../../../core/models/egresado.model';

@Component({
  selector: 'app-datos-personales',
  templateUrl: './datos-personales.component.html',
  styleUrls: ['./datos-personales.component.css']
})
export class DatosPersonalesComponent implements OnInit {

  datosForm: FormGroup;
  listaCarreras: Carrera[] = [];
  enviando: boolean = false;

  constructor(private fb: FormBuilder, private egresadoService: EgresadoService) {
    this.datosForm = this.initForm();
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
        codigoUniversitario: [{ value: '201802551', disabled: true }, Validators.required],
        añoEgreso: ['', [Validators.required, Validators.min(1990)]]
      })
    });
  }

  cargarCarreras() {
    this.egresadoService.obtenerCarreras().subscribe({
      next: (data) => this.listaCarreras = data,
      error: (err) => console.error(err)
    });
  }

  guardarDatos() {
    if (this.datosForm.invalid) return;
    this.enviando = true;
    const formValue = this.datosForm.getRawValue();

    // Solo enviamos contacto
    const payload = {
      telefono: formValue.datosPersonales.telefono,
      correoPersonal: formValue.datosPersonales.correoPersonal,
      // No enviamos experiencia
    };

    this.egresadoService.actualizarPerfil(payload).subscribe({
      next: () => {
        this.enviando = false;
        alert('Datos de contacto actualizados');
      },
      error: (err) => {
        this.enviando = false;
        alert('Error al guardar');
      }
    });
  }
}
