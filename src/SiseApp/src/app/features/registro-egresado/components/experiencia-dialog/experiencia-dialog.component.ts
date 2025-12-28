import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-experiencia-dialog',
  templateUrl: './experiencia-dialog.component.html',
  styleUrls: ['./experiencia-dialog.component.css'] // Si existe
})
export class ExperienciaDialogComponent implements OnInit {
  form: FormGroup;
  esEdicion: boolean = false;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<ExperienciaDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any // Aquí recibimos los datos si es editar
  ) {
    this.form = this.fb.group({
      empresa: ['', Validators.required],
      cargo: ['', Validators.required],
      fechaInicio: ['', Validators.required],
      fechaFin: [''],
      actualmente: [false]
    });
  }

  ngOnInit(): void {
    if (this.data) {
      this.esEdicion = true;
      // Convertir fechas string a formato que acepte el input date (yyyy-MM-dd) si es necesario
      this.form.patchValue(this.data);
    }
  }

  guardar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    // Cerramos el modal y enviamos los datos al padre
    this.dialogRef.close(this.form.value);
  }

  cancelar() {
    this.dialogRef.close();
  }
}
