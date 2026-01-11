import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service'; // Asumo que tienes un servicio de auth

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    // 1. Obtenemos los roles esperados desde la configuración de la ruta
    const expectedRoles = route.data['roles'] as Array<string>;

    // 2. Obtenemos el rol actual del usuario
    const userRole = this.authService.getRole();

    // 3. Verificamos si el rol del usuario está incluido en los permitidos
    if (userRole && expectedRoles.includes(userRole)) {
      return true;
    }

    // 4. Si no tiene permiso, redirigimos
    this.router.navigate(['/dashboard']);
    return false;
  }
}
