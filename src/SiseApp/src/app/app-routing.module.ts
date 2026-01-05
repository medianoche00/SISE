import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Layouts
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';

// Componentes (Páginas)
import { DashboardReporteComponent } from './pages/dashboard-reporte/dashboard-reporte.component';
import { EstadisticasComponent } from './pages/estadisticas/estadisticas.component';
import { ReportesComponent } from './pages/reportes/reportes.component';
// (Si tienes un login, iría aquí, pero me enfoco en lo que pediste)

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
    children: [
      // { path: 'login', component: LoginComponent } 
    ]
  },

  { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }