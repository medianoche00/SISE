import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { RegistroEgresadoRoutingModule } from './registro-egresado-routing.module';
import { RegistroEgresadoComponent } from './registro-egresado.component';

// Angular Material
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDividerModule } from '@angular/material/divider';
import { MatTooltipModule } from '@angular/material/tooltip';
import { DatosPersonalesComponent } from './components/datos-personales/datos-personales.component';
import { ExperienciaLaboralComponent } from './components/experiencia-laboral/experiencia-laboral.component';
import { FormacionComplementariaComponent } from './components/formacion-complementaria/formacion-complementaria.component';
import { ExperienciaDialogComponent } from './components/experiencia-dialog/experiencia-dialog.component';
import { FormacionDialogComponent } from './components/formacion-dialog/formacion-dialog.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    RegistroEgresadoComponent,
    DatosPersonalesComponent,
    ExperienciaLaboralComponent,
    FormacionComplementariaComponent,
    ExperienciaDialogComponent,
    FormacionDialogComponent
  ],
  imports: [
    CommonModule,
    RegistroEgresadoRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,

    // Material
    MatProgressBarModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatFormFieldModule,
    MatDividerModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    RouterModule
  ],
})
export class RegistroEgresadoModule {}
