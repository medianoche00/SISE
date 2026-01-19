import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatOptionModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-persona-detail',
  templateUrl: './persona-detail.component.html',
  styleUrls: ['./persona-detail.component.scss'],
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
    MatIconModule
  ],
})
export class PersonasDetailComponent implements OnInit {
  form: FormGroup;
  titulo: string = 'Nueva Persona';
  esVer: boolean = false;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<PersonasDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) {
    this.form = this.fb.group({
      id: [null],
      nombre: ['', Validators.required],
      apellidoPaterno: ['', Validators.required],
      apellidoMaterno: ['', Validators.required],
      tipoDocumento: ['DNI', Validators.required],
      numeroDocumento: ['', Validators.required],
      telefono: [''],
      correo: ['', [Validators.email]],
      // Dirección
      departamento: [''],
      provincia: [''],
      distrito: [''],
      direccionEspecifica: [''],
    });
  }

  ngOnInit(): void {
    if (this.data) {
      this.titulo =
        this.data.accion === 'crear'
          ? 'Nueva Persona'
          : this.data.accion === 'editar'
            ? 'Editar Persona'
            : 'Detalle de Persona';

      this.esVer = this.data.accion === 'ver';

      if (this.data.persona) {
        this.form.patchValue(this.data.persona);
      }

      if (this.esVer) {
        this.form.disable(); // Bloquea todos los inputs de Material
      }
    }
  }

  guardar(): void {
    if (this.form.valid) {
      this.dialogRef.close(this.form.getRawValue());
    }
  }

  cerrar(): void {
    this.dialogRef.close();
  }
}
