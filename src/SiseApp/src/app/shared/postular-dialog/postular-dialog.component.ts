import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-postular-dialog',
  templateUrl: './postular-dialog.component.html',
  styleUrl: './postular-dialog.component.css',
  standalone: true,
  imports: [
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatInputModule,
  ],
})
export class PostularDialogComponent {
  public cartaPresentacion: string = '';
  public form: FormGroup;

  constructor(public dialogRef: MatDialogRef<PostularDialogComponent>) {
    this.form = new FormBuilder().group({
      cartaPresentacion: ['', Validators.required],
    });
  }

  cerrar() {
    this.dialogRef.close();
  }

  // Al dar click, solo devolvemos la acción al padre, no llamamos a la API aquí
  ejecutarAccionPrincipal() {
    if (this.form.invalid) return;

    const formValue = this.form.getRawValue();
    const payload: string = formValue.cartaPresentacion;

    this.dialogRef.close(payload);
  }
}
