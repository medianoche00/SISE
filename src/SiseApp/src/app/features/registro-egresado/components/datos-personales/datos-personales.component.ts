import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EgresadoService } from '../../../../core/services/egresado.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-datos-personales',
  templateUrl: './datos-personales.component.html',
  styleUrls: ['./datos-personales.component.css']
})
export class DatosPersonalesComponent implements OnInit {

  datosForm: FormGroup;
  enviando: boolean = false;
  cargandoDatos: boolean = true;

  constructor(private fb: FormBuilder, private egresadoService: EgresadoService, private snackBar: MatSnackBar) {
    this.datosForm = this.initForm();
  }

  ngOnInit(): void { 
    this.cargarDatosPersonales();
  }

  initForm(): FormGroup {
    return this.fb.group({
      datosPersonales: this.fb.group({
        nombres: [{ value: '', disabled: true }, Validators.required],
        apellidoPaterno: [{ value: '', disabled: true }, Validators.required],
        apellidoMaterno: [{ value: '', disabled: true }, Validators.required],
        documentoIdentidad: [{ value: '', disabled: true }, Validators.required],
        telefono: ['', [Validators.required, Validators.pattern(/^[0-9]{9}$/)]],
        correoPersonal: ['', [Validators.required, Validators.email]]
      }),
      datosAcademicos: this.fb.group({
        idCarrera: [{ value: '', disabled: true }, Validators.required],
        codigoUniversitario: [{ value: '', disabled: true }, Validators.required],
        añoEgreso: [{ value: '', disabled: true }, Validators.required],
        carrera: [{ value: '', disabled: true }, Validators.required],
      })
    });
  }

  cargarDatosPersonales() {
    this.cargandoDatos = true;
    this.egresadoService.obtenerMiPerfil().subscribe({
      next: (data) => {
        this.datosForm.patchValue({
          datosPersonales: {
            nombres: data.nombres,
            apellidoPaterno: data.apellidoPaterno,
            apellidoMaterno: data.apellidoMaterno,
            documentoIdentidad: data.documentoIdentidad,
            telefono: data.telefono,
            correoPersonal: data.correoPersonal
          },
          datosAcademicos: {
            idCarrera: data.idCarrera,
            codigoUniversitario: data.codigoUniversitario,
            añoEgreso: data.añoEgreso,
            carrera: data.carrera
          }
        });
        this.cargandoDatos = false;
      },
      error: (err) => {
        console.error('Error cargando perfil', err);
        this.cargandoDatos = false;
        const mensajeError = err.error || 'Error al cargar datos personales';
        this.mostrarMensaje(mensajeError, 'error');
      }
    });
  }

  // Helper para mostrar mensajes tipo "Toast"
  mostrarMensaje(mensaje: string, tipo: 'success' | 'error') {
    this.snackBar.open(mensaje, 'CERRAR', {
      duration: 4000,
      panelClass:
        tipo === 'error'
          ? ['mat-toolbar', 'mat-warn']
          : ['mat-toolbar', 'mat-primary'],
      verticalPosition: 'top', // Para que salga arriba
    });
  }

  guardarDatos() {
    if (this.datosForm.invalid) return;
    this.enviando = true;
    
    const formValue = this.datosForm.getRawValue();

    const payload = {
      telefono: formValue.datosPersonales.telefono,
      correoPersonal: formValue.datosPersonales.correoPersonal,
    };

    this.egresadoService.actualizarPerfil(payload).subscribe({
      next: () => {
        this.enviando = false;
        alert('Datos de contacto actualizados correctamente');
      },
      error: (err) => {
        this.enviando = false;
        alert('Error al guardar cambios');
      }
    });
  }
}