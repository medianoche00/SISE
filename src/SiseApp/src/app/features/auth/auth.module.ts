import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common'; // <--- IMPORTANTE: Usar este en lugar de BrowserModule
import { ReactiveFormsModule } from '@angular/forms';

// Material
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon'; // <--- MatIconModule, NO MatIcon
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';

// Routing y Componentes
import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './login/login.component';

@NgModule({
  declarations: [
    LoginComponent
  ],
  imports: [
    CommonModule, // <--- Reemplaza a BrowserModule y BrowserAnimationsModule
    AuthRoutingModule,
    ReactiveFormsModule,
    
    // Material Modules
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ]
})
export class AuthModule {}