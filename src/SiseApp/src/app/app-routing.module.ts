import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { AuthGuard } from './core/guards/auth.guard';
import { LoginGuard } from './core/guards/login.guard';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { OfertasDisponiblesComponent } from './features/ofertas-disponibles/ofertas-disponibles.component';
import { RegistroEgresadoComponent } from './features/registro-egresado/registro-egresado.component';
import { DatosPersonalesComponent } from './features/registro-egresado/components/datos-personales/datos-personales.component';
import { ExperienciaLaboralComponent } from './features/registro-egresado/components/experiencia-laboral/experiencia-laboral.component';
import { FormacionComplementariaComponent } from './features/registro-egresado/components/formacion-complementaria/formacion-complementaria.component';
import { MisPostulacionesComponent } from './features/mis-postulaciones/mis-postulaciones.component';

// --- NUEVO: Importamos el componente del reporte ---
import { DashboardReporteComponent } from './pages/dashboard-reporte/dashboard-reporte.component';

const routes: Routes = [
  {
    path: 'auth',
    component: AuthLayoutComponent,
    children: [
      {
        path: 'login',
        loadChildren: () =>
          import('./features/auth/auth.module').then((m) => m.AuthModule),
        canActivate: [LoginGuard],
      },
    ],
  },
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      
      // --- NUEVO: Aquí agregamos la ruta para ver el reporte ---
      { path: 'dashboard-reporte', component: DashboardReporteComponent },

      { path: 'ofertasdisponibles', component: OfertasDisponiblesComponent },
      { path: 'mis-postulaciones', component: MisPostulacionesComponent },
      {
        path: 'perfil',
        component: RegistroEgresadoComponent,
        children: [
          { path: '', redirectTo: 'datos', pathMatch: 'full' },
          { path: 'datos', component: DatosPersonalesComponent },
          { path: 'experiencia', component: ExperienciaLaboralComponent },
          { path: 'formacion', component: FormacionComplementariaComponent },
        ],
      },
    ],
  },
  { path: '**', redirectTo: 'dashboard' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'top' })],
  exports: [RouterModule],
})
export class AppRoutingModule {}