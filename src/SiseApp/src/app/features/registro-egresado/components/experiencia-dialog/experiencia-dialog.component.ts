import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ExperienciaLaboral } from '../../../../core/services/experiencias.service';

@Component({
  selector: 'app-experiencia-dialog',
  templateUrl: './experiencia-dialog.component.html',
  styleUrls: ['./experiencia-dialog.component.css'] // Opcional si usas estilos inline
})
export class ExperienciaDialogComponent implements OnInit {
  form: FormGroup;
  esEdicion: boolean = false;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ExperienciaDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ExperienciaLaboral | null
  ) {
    this.esEdicion = !!data; // Si hay data, es edición
    this.form = this.initForm();
  }

  ngOnInit(): void {
    // Si es edición, llenamos el formulario
    if (this.data) {
      this.form.patchValue(this.data);
    }

    // Listener para deshabilitar fecha fin si marca "Actualmente"
    this.form.get('actualmente')?.valueChanges.subscribe((checked) => {
      const fechaFinControl = this.form.get('fechaFin');
      if (checked) {
        fechaFinControl?.disable();
        fechaFinControl?.setValue(null);
      } else {
        fechaFinControl?.enable();
      }
    });
  }

  initForm(): FormGroup {
    return this.fb.group({
      empresa: ['', Validators.required],
      cargo: ['', Validators.required],
      fechaInicio: ['', Validators.required],
      fechaFin: [''],
      actualmente: [false],
      descripcion: ['']
    });
  }

  guardar() {
    if (this.form.invalid) return;
    // Retornamos los valores al componente padre para que él llame al servicio
    this.dialogRef.close(this.form.getRawValue());
  }

  cerrar() {
    this.dialogRef.close();
  }
}