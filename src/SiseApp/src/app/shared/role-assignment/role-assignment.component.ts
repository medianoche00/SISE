import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { Persona } from '../../core/models/persona.interface';

@Component({
  selector: 'app-role-assignment',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, FormsModule,
    MatDialogModule, MatButtonModule, MatFormFieldModule,
    MatInputModule, MatSelectModule, MatIconModule
  ],
  templateUrl: './role-assignment.component.html',
  styleUrls: ['./role-assignment.component.css']
})
export class RoleAssignmentComponent implements OnInit {
  form: FormGroup;
  persona: Persona;

  roles = ['Administrador', 'Egresado', 'Representante'];
  carreras = ['Ingeniería de Sistemas', 'Administración', 'Derecho'];
  cargosAdmin = ['Secretaría', 'Director Académico', 'Soporte Técnico'];
  empresas = ['Tech Solutions SAC', 'Constructora Global', 'Banco Financiero'];

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<RoleAssignmentComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: { persona: Persona }
  ) {
    this.persona = data.persona;

    this.form = this.fb.group({
      username: [this.persona.correoPersonal || '', [Validators.required, Validators.email]],
      password: ['Sise123.', [Validators.required, Validators.minLength(6)]],
      rol: ['', Validators.required],
      carrera: [''],
      codigoUniversitario: [''],
      anioEgreso: [''],
      cargoAdmin: [''],
      empresa: [''],
      cargoEmpresa: ['']
    });
  }

  ngOnInit(): void {
    this.form.get('rol')?.valueChanges.subscribe(rol => {
      this.actualizarValidaciones(rol);
    });
  }

  actualizarValidaciones(rol: string) {
    const campos = ['carrera', 'codigoUniversitario', 'anioEgreso', 'cargoAdmin', 'empresa', 'cargoEmpresa'];
    campos.forEach(c => this.form.get(c)?.clearValidators());

    if (rol === 'Egresado') {
      this.form.get('carrera')?.setValidators([Validators.required]);
      this.form.get('codigoUniversitario')?.setValidators([Validators.required]);
      this.form.get('anioEgreso')?.setValidators([Validators.required]);
    } else if (rol === 'Administrador') {
      this.form.get('cargoAdmin')?.setValidators([Validators.required]);
    } else if (rol === 'Representante') {
      this.form.get('empresa')?.setValidators([Validators.required]);
      this.form.get('cargoEmpresa')?.setValidators([Validators.required]);
    }

    campos.forEach(c => this.form.get(c)?.updateValueAndValidity());
  }

  guardar() {
    if (this.form.valid) {
      console.log('Creando usuario y perfil:', this.form.value);
      this.dialogRef.close(true); 
      this.router.navigate(['/usuarios']);
    }
  }

  cerrar() {
    this.dialogRef.close();
  }
}
