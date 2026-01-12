import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Layouts
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { EstadisticasComponent } from './features/estadisticas/estadisticas.component';
import { ReportesComponent } from './features/reportes/reportes.component';
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
import { RoleGuard } from './core/guards/role.guard';

const routes: Routes = [
  // ----------------------------------------------------------------
  // RUTAS PÚBLICAS (Login, Auth)
  // ----------------------------------------------------------------
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

  // ----------------------------------------------------------------
  // RUTAS PROTEGIDAS (Layout Principal)
  // ----------------------------------------------------------------
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard], // Primero verifica que esté logueado
    children: [
      // > RUTA COMÚN (Accesible para todos los logueados)
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },

      // ----------------------------------------------------------------
      // > SECTOR ADMINISTRADOR (Admin y Administrativo)
      // ----------------------------------------------------------------
      {
        path: 'admin',
        canActivate: [RoleGuard], // Verifica el rol
        data: { roles: ['Administrador'] },
        children: [
          { path: 'personas', component: PersonasListComponent },
          { path: 'usuarios', component: UsuariosListComponent },
        ],
      },
      {
        path: 'administrativo',
        canActivate: [RoleGuard], // Verifica el rol
        data: { roles: ['Administrativo'] },
        children: [
          { path: 'stats', component: EstadisticasComponent },
          { path: 'reports', component: ReportesComponent },
        ],
      },

      // ----------------------------------------------------------------
      // > SECTOR EGRESADO
      // ----------------------------------------------------------------
      {
        path: 'egresado',
        canActivate: [RoleGuard],
        data: { roles: ['Egresado'] },
        children: [
          {
            path: 'ofertas-disponibles',
            component: OfertasDisponiblesComponent,
          },
          { path: 'mis-postulaciones', component: MisPostulacionesComponent },
          {
            path: 'perfil',
            component: RegistroEgresadoComponent,
            children: [
              { path: '', redirectTo: 'datos', pathMatch: 'full' },
              { path: 'datos', component: DatosPersonalesComponent },
              { path: 'experiencia', component: ExperienciaLaboralComponent },
              {
                path: 'formacion',
                component: FormacionComplementariaComponent,
              },
            ],
          },
        ],
      },

      // ----------------------------------------------------------------
      // > SECTOR REPRESENTANTE (Ejemplo)
      // ----------------------------------------------------------------
      {
        path: 'representante',
        canActivate: [RoleGuard],
        data: { roles: ['Representante'] },
        children: [
          // Aquí irían los componentes específicos del representante
          // { path: 'validar-ofertas', component: ... }
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
