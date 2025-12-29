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

    const formValue = this.form.getRawValue();

    // PREPARACIÓN DE DATOS (Mapeo manual para evitar errores)
    const payload: ExperienciaLaboral = {
      // Mantenemos el ID si existe (edición)
      idExperiencia: this.data?.idExperiencia, 
      
      empresa: formValue.empresa,
      cargo: formValue.cargo,
      descripcion: formValue.descripcion,
      actualmente: formValue.actualmente,

      // CORRECCIÓN DE FECHAS
      // 1. Convertimos la fecha de inicio a string ISO simple o Date
      fechaInicio: formValue.fechaInicio, 
      
      // 2. Lógica crítica para Fecha Fin:
      // Si "actualmente" es true O si el campo está vacío/null -> enviamos null explícito
      fechaFin: (formValue.actualmente || !formValue.fechaFin) ? undefined : formValue.fechaFin
    };

    // Nota: Si tu backend es muy estricto con las fechas y sigue fallando,
    // avísame para agregar una función que las convierta a texto "YYYY-MM-DD".
    
    // Al usar undefined en JSON.stringify, el campo desaparece o se envía como null 
    // dependiendo de la configuración. Para asegurar que llegue null a .NET:
    
    if (payload.actualmente) {
      payload.fechaFin = null as any; 
    }

    this.dialogRef.close(payload);
  }

  cerrar() {
    this.dialogRef.close();
  }
}