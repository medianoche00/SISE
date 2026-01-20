import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
// Importamos el servicio y las interfaces
import { UbigeoService, Departamento, Provincia, Distrito } from '../../core/services/ubigeo.service';

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
    MatSelectModule,
    MatIconModule
  ],
})
export class PersonasDetailComponent implements OnInit {
  form: FormGroup;
  titulo: string = 'Nueva Persona';
  esVer: boolean = false;

  // Listas para los desplegables
  listaDepartamentos: Departamento[] = [];
  listaProvincias: Provincia[] = [];
  listaDistritos: Distrito[] = [];

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<PersonasDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private ubigeoService: UbigeoService // Inyectamos el servicio
  ) {
    this.form = this.fb.group({
      idPersona: [null],
      nombre: ['', Validators.required],
      apellidoPaterno: ['', Validators.required],
      apellidoMaterno: ['', Validators.required],
      tipoDocumento: ['DNI', Validators.required],
      numeroDocumento: ['', Validators.required],
      telefono: [''],
      correo: ['', [Validators.email]],
      // Ubigeo
      departamento: [''],
      provincia: [''],
      distrito: [''],
      direccionEspecifica: [''],
      estado: ['Activo', Validators.required],
      rol: ['']
    });
  }

  ngOnInit(): void {
    // 1. Cargar Departamentos al iniciar
    this.ubigeoService.getDepartamentos().subscribe(deps => {
      this.listaDepartamentos = deps;

      // CONFIGURACIÓN INICIAL (Si es Editar/Ver)
      if (this.data && this.data.persona) {
        this.inicializarFormularioConDatos(this.data.persona);
      }
    });

    if (this.data) {
      this.titulo = this.data.accion === 'crear' ? 'Nueva Persona' :
        this.data.accion === 'editar' ? 'Editar Persona' : 'Detalle de Persona';
      this.esVer = this.data.accion === 'ver';
    }
  }

  // Lógica para pre-cargar los combos en cascada cuando se edita
  inicializarFormularioConDatos(persona: any) {
    this.form.patchValue(persona);

    // Si hay un departamento seleccionado (texto), buscamos su ID para cargar provincias
    if (persona.departamento) {
      const depEncontrado = this.listaDepartamentos.find(d => d.nombre === persona.departamento);
      if (depEncontrado) {
        this.ubigeoService.getProvincias(depEncontrado.id).subscribe(provs => {
          this.listaProvincias = provs;

          // Si hay provincia, cargamos distritos
          if (persona.provincia) {
            const provEncontrada = this.listaProvincias.find(p => p.nombre === persona.provincia);
            if (provEncontrada) {
              this.ubigeoService.getDistritos(provEncontrada.id).subscribe(dists => {
                this.listaDistritos = dists;
                // Re-seteamos valores para asegurar que el MatSelect los reconozca
                this.form.patchValue({
                  provincia: persona.provincia,
                  distrito: persona.distrito
                });
                if (this.esVer) this.form.disable();
              });
            }
          } else if (this.esVer) this.form.disable();
        });
      }
    } else if (this.esVer) this.form.disable();
  }

  // EVENTO: Cambio de Departamento
  seleccionarDepartamento(nombreDepartamento: string) {
    // Reseteamos hijos
    this.form.get('provincia')?.setValue('');
    this.form.get('distrito')?.setValue('');
    this.listaProvincias = [];
    this.listaDistritos = [];

    const dep = this.listaDepartamentos.find(d => d.nombre === nombreDepartamento);
    if (dep) {
      this.ubigeoService.getProvincias(dep.id).subscribe(res => this.listaProvincias = res);
    }
  }

  // EVENTO: Cambio de Provincia
  seleccionarProvincia(nombreProvincia: string) {
    // Reseteamos hijo
    this.form.get('distrito')?.setValue('');
    this.listaDistritos = [];

    const prov = this.listaProvincias.find(p => p.nombre === nombreProvincia);
    if (prov) {
      this.ubigeoService.getDistritos(prov.id).subscribe(res => this.listaDistritos = res);
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
