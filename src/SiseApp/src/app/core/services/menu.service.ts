import { Injectable } from '@angular/core';

export interface MenuItem {
  label: string;
  icon: string;
  route: string;
}

@Injectable({
  providedIn: 'root',
})
export class MenuService {
  getMenuByRole(role: string): MenuItem[] {
    switch (role) {
      case 'Administrador':
        return [
          { label: 'Inicio', icon: 'home', route: '/dashboard' },
          { label: 'Gestión Usuarios', icon: 'people', route: '/admin/usuarios' },
        ];
      case 'Egresado':
        return [
          { label: 'Inicio', icon: 'home', route: '/dashboard' },
          {
            label: 'Mi Perfil',
            icon: 'person',
            route: 'egresado/perfil',
          },
          {
            label: 'Ofertas Laborales',
            icon: 'work',
            route: 'egresado/ofertas-disponibles',
          },
          {
            label: 'Mis Postulaciones',
            icon: 'assignment',
            route: 'egresado/mis-postulaciones',
          },
        ];
      case 'Representante':
        return [
          { label: 'Inicio', icon: 'home', route: '/dashboard' },
          {
            label: 'Publicar Oferta',
            icon: 'add_circle',
            route: 'representante/post-job',
          },
          {
            label: 'Candidatos',
            icon: 'group',
            route: 'representante/candidates',
          },
        ];
      case 'Administrativo':
        return [
          { label: 'Inicio', icon: 'home', route: '/dashboard' },
          {
            label: 'Estadísticas',
            icon: 'bar_chart',
            route: 'administrativo/stats',
          },
          {
            label: 'Reportes',
            icon: 'description',
            route: 'administrativo/reports',
          },
        ];
      default:
        return [];
    }
  }
}
