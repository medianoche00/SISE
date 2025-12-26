import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // Necesario para [(ngModel)]

import { OfertasRoutingModule } from './ofertas-routing.module';
import { OfertaListComponent } from './oferta-list/oferta-list.component';
//import { OfertaDetailComponent } from '../../shared/oferta-detail/oferta-detail.component';

// Material Imports necesarios
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDividerModule } from '@angular/material/divider';

@NgModule({
  declarations: [
    OfertaListComponent,
    //OfertaDetailComponent
  ],
  imports: [
    CommonModule,
    OfertasRoutingModule,
    FormsModule, 
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatInputModule,
    MatSelectModule,
    MatFormFieldModule,
    MatDividerModule
  ]
})
export class OfertasModule { }