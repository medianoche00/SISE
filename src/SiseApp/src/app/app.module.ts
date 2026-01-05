import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms'; // <--- IMPORTANTE: Necesario para los filtros [(ngModel)]

// Routing y Componentes Principales
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';

// Componentes Shared/Pages
import { HeaderComponent } from './shared/header/header.component';
import { SidebarComponent } from './shared/sidebar/sidebar.component';
import { DashboardReporteComponent } from './pages/dashboard-reporte/dashboard-reporte.component';

// Interceptores
import { AuthInterceptor } from './core/interceptors/auth.interceptor';

// Angular Material Modules
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  MAT_DATE_LOCALE,
  MatNativeDateModule,
} from '@angular/material/core';

// Feature Modules
import { RegistroEgresadoModule } from './features/registro-egresado/registro-egresado.module';
import { EstadisticasComponent } from './pages/estadisticas/estadisticas.component';
import { ReportesComponent } from './pages/reportes/reportes.component';

@NgModule({
  declarations: [
    AppComponent,
    MainLayoutComponent,
    AuthLayoutComponent,
    HeaderComponent,
    SidebarComponent,
    DashboardReporteComponent,
    EstadisticasComponent,
    ReportesComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    RouterModule,
    FormsModule, // <--- Agregado para que funcionen los filtros del dashboard
    
    // Angular Material
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatBadgeModule,
    MatInputModule,
    MatListModule,
    MatSidenavModule,
    MatChipsModule,
    MatDialogModule,
    MatDatepickerModule,
    MatNativeDateModule,

    // Feature Modules
    RegistroEgresadoModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }, 
    { provide: MAT_DATE_LOCALE, useValue: 'es-PE' }
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}