import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegistroEgresadoComponent } from './registro-egresado.component';

const routes: Routes = [
  { path: '', component: RegistroEgresadoComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistroEgresadoRoutingModule { }