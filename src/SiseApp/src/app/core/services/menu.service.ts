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
          {
            label: 'Ofertas Laborales',
            icon: 'work',
            route: '/ofertas',
          }, //! -------------------QUITAR LUEGO------------------------
          { label: 'Usuarios', icon: 'people', route: '/dashboard/users' },
          {
            label: 'Configuración',
            icon: 'settings',
            route: '/dashboard/settings',
          },
        ];
      case 'Egresado':
        return [
          { label: 'Inicio', icon: 'home', route: '/dashboard' },
          { label: 'Mi Perfil', icon: 'person', route: '/dashboard/profile' },
          {
            label: 'Ofertas Laborales',
            icon: 'work',
            route: '/dashboard/jobs',
          },
          {
            label: 'Mis Postulaciones',
            icon: 'assignment',
            route: '/dashboard/applications',
          },
        ];
      case 'Representante':
        return [
          { label: 'Inicio', icon: 'home', route: '/dashboard' },
          {
            label: 'Publicar Oferta',
            icon: 'add_circle',
            route: '/dashboard/post-job',
          },
          {
            label: 'Candidatos',
            icon: 'group',
            route: '/dashboard/candidates',
          },
        ];
      case 'Administrativo':
        return [
          { label: 'Inicio', icon: 'home', route: '/dashboard' },
          {
            label: 'Estadísticas',
            icon: 'bar_chart',
            route: '/dashboard/stats',
          },
          {
            label: 'Reportes',
            icon: 'description',
            route: '/dashboard/reports',
          },
        ];
      default:
        return [];
    }
  }
}
