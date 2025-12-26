import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OfertaListComponent } from './oferta-list/oferta-list.component';

const routes: Routes = [
  { path: '', component: OfertaListComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OfertasRoutingModule { }