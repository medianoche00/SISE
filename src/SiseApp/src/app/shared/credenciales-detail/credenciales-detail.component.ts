import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { CredencialesService } from '../../core/services/credenciales.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

export interface CredencialesModalData {
  idUsuario: number;
  username: string;
}

@Component({
  selector: 'app-credenciales-detail',
  templateUrl: './credenciales-detail.component.html',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    FormsModule,
    ReactiveFormsModule,
    MatProgressSpinnerModule,
  ],
})
export class CredencialesDetailComponent implements OnInit {
  form: FormGroup;
  isLoading = false;
  hidePassword = true;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CredencialesDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CredencialesModalData,
    private credencialesService: CredencialesService,
  ) {
    this.form = this.fb.group({
      username: [data.username || '', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      documentoRespaldo: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {}

  guardar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const val = this.form.value;

    this.credencialesService
      .actualizarCredenciales(
        this.data.idUsuario,
        val.username,
        val.password,
        val.documentoRespaldo,
      )
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.dialogRef.close(true);
        },
        error: (err) => {
          console.error('Error:', err);
          this.isLoading = false;
          alert(
            'Ocurrió un error al actualizar: ' +
              (err.error?.message || 'Error desconocido'),
          );
        },
      });
  }

  cancelar() {
    this.dialogRef.close();
  }
}
