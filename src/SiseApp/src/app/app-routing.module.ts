import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Layouts
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { EstadisticasComponent } from './pages/estadisticas/estadisticas.component';
import { ReportesComponent } from './pages/reportes/reportes.component';
import { AuthGuard } from './core/guards/auth.guard';
import { LoginGuard } from './core/guards/login.guard';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { OfertasDisponiblesComponent } from './features/ofertas-disponibles/ofertas-disponibles.component';
import { MisPostulacionesComponent } from './features/mis-postulaciones/mis-postulaciones.component';
import { RegistroEgresadoComponent } from './features/registro-egresado/registro-egresado.component';
import { DatosPersonalesComponent } from './features/registro-egresado/components/datos-personales/datos-personales.component';
import { ExperienciaLaboralComponent } from './features/registro-egresado/components/experiencia-laboral/experiencia-laboral.component';
import { FormacionComplementariaComponent } from './features/registro-egresado/components/formacion-complementaria/formacion-complementaria.component';
import { PersonasListComponent } from './features/personas/personas-list/personas-list.component';
import { UsuariosListComponent } from './features/usuarios/usuarios-list/usuarios-list.component';

const routes: Routes = [
  {
    path: 'auth',
    component: AuthLayoutComponent,
    canActivate: [LoginGuard],
    children: [
      {
        path: 'login',
        loadChildren: () =>
          import('./features/auth/auth.module').then((m) => m.AuthModule),
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
      { path: 'admin/personas', component: PersonasListComponent },
      { path: 'admin/usuarios', component: UsuariosListComponent },
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
      { path: 'stats', component: EstadisticasComponent },
      { path: 'reports', component: ReportesComponent },
    ],
  },

  { path: '**', redirectTo: 'dashboard' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
