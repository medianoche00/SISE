import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormacionComplementaria } from '../../../../core/services/formaciones.service';

@Component({
  selector: 'app-formacion-dialog',
  templateUrl: './formacion-dialog.component.html',
  styleUrls: ['./formacion-dialog.component.css'],
})
export class FormacionDialogComponent implements OnInit {
  form: FormGroup;
  esEdicion: boolean = false;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<FormacionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: FormacionComplementaria | null
  ) {
    this.esEdicion = !!data; // Si hay data, es edición
    this.form = this.initForm();
  }

  ngOnInit(): void {
    if (this.data) {
      this.esEdicion = true;
      this.form.patchValue(this.data);
    }
  }

  initForm(): FormGroup {
    return this.fb.group({
      tipoFormacion: ['', Validators.required],
      nombreDelCurso: ['', Validators.required],
      institucion: ['', Validators.required],
      fechaInicio: ['', Validators.required],
      fechaFin: ['', Validators.required],
    });
  }

  guardar() {
    if (this.form.invalid) return;

    const formValue = this.form.getRawValue();

    // PREPARACIÓN DE DATOS (Mapeo manual para evitar errores)
    const payload: FormacionComplementaria = {
      // Mantenemos el ID si existe (edición)
      idFormacion: this.data?.idFormacion,
      tipoFormacion: formValue.tipoFormacion,
      nombreDelCurso: formValue.nombreDelCurso,
      institucion: formValue.institucion,
      fechaInicio: formValue.fechaInicio,
      fechaFin: formValue.fechaFin,
      estado: true
    };

    this.dialogRef.close(payload);
  }

  cerrar() {
    this.dialogRef.close();
  }
}
