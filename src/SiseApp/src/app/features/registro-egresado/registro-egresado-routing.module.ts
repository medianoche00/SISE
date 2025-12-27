import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegistroEgresadoComponent } from './registro-egresado.component';
import { DatosPersonalesComponent } from './components/datos-personales/datos-personales.component';
import { ExperienciaLaboralComponent } from './components/experiencia-laboral/experiencia-laboral.component';
import { FormacionComplementariaComponent } from './components/formacion-complementaria/formacion-complementaria.component';

const routes: Routes = [
  {
    path: '',
    component: RegistroEgresadoComponent, // El componente "Marco" con los botones
    children: [
      { path: '', redirectTo: 'datos', pathMatch: 'full' }, // Por defecto abre Datos
      { path: 'datos', component: DatosPersonalesComponent },
      { path: 'experiencia', component: ExperienciaLaboralComponent },
      { path: 'formacion', component: FormacionComplementariaComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistroEgresadoRoutingModule { }
