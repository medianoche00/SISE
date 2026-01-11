import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Layouts
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { DashboardReporteComponent } from './pages/dashboard-reporte/dashboard-reporte.component';
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
  // 1. RUTA PRINCIPAL (Usa MainLayout -> Mantiene Navbar y Sidebar)
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardReporteComponent },   // Menú de botones
      { path: 'estadisticas', component: EstadisticasComponent },    // Gráficos y Tablas
      { path: 'reportes', component: ReportesComponent },            // Lista de Empresas
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }       // Redirección automática
    ]
  },

  // 2. RUTA DE AUTENTICACIÓN (Login - Opcional si ya lo tienes)
  {
    path: 'auth',
    component: AuthLayoutComponent,
    canActivate: [LoginGuard],
    children: [
      {
        path: 'login',
        loadChildren: () => import('./features/auth/auth.module').then((m) => m.AuthModule),
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
      { path: 'dashboard-reporte', component: DashboardReporteComponent },
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
    ],
  },

  { path: '**', redirectTo: 'dashboard' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
