import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-formacion-dialog',
  templateUrl: './formacion-dialog.component.html',
  styleUrls: ['./formacion-dialog.component.css']
})
export class FormacionDialogComponent implements OnInit {
  form: FormGroup;
  esEdicion: boolean = false;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<FormacionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    // Ajusta estos campos según tu Base de Datos real
    this.form = this.fb.group({
      institucion: ['', Validators.required],
      nombreFormacion: ['', Validators.required], // Ej: Curso de Angular
      fechaInicio: ['', Validators.required],
      fechaFin: ['', Validators.required],
      horas: [''] // Opcional
    });
  }

  ngOnInit(): void {
    if (this.data) {
      this.esEdicion = true;
      this.form.patchValue(this.data);
    }
  }

  guardar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.dialogRef.close(this.form.value);
  }

  cancelar() {
    this.dialogRef.close();
  }
}
