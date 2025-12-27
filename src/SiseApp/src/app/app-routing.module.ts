import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { AuthGuard } from './core/guards/auth.guard';
import { LoginGuard } from './core/guards/login.guard';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { OfertasDisponiblesComponent } from './features/ofertas-disponibles/ofertas-disponibles.component';
import { RegistroEgresadoComponent } from './features/registro-egresado/registro-egresado.component';

const routes: Routes = [
  // 1. Rutas de Autenticación
  // CAMBIO CLAVE: Agregamos "path: 'auth'" para que la URL final sea /auth/login
  // Esto coincide con lo que tu Guard y AuthService están pidiendo.
  {
    path: 'auth',
    component: AuthLayoutComponent,
    children: [
      {
        path: 'login',
        loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule),
        canActivate: [LoginGuard]
       }
    ]
  },

  // 2. Rutas de la Aplicación (Dashboard)
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'ofertasdisponibles', component: OfertasDisponiblesComponent },
      { path: 'registro-egresado', component: RegistroEgresadoComponent },
    ]
  },

  // 3. Comodín Seguro
  // Si la ruta no existe, mandarlo a dashboard. 
  // El AuthGuard se encargará de mandarlo a /auth/login si no tiene permiso.
  { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'top' })],
  exports: [RouterModule]
})
export class AppRoutingModule {}