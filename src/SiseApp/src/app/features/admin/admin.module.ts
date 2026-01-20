import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { UsuariosComponent } from './pages/usuarios/usuarios.component';
import { UsuarioDialogComponent } from './components/usuario-dialog/usuario-dialog.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';

@NgModule({
  declarations: [UsuariosComponent, UsuarioDialogComponent],
  imports: [
    CommonModule, AdminRoutingModule, ReactiveFormsModule, FormsModule,
    MatTableModule, MatButtonModule, MatIconModule, MatDialogModule,
    MatInputModule, MatSelectModule, MatCardModule
  ]
})
export class AdminModule { }
