import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class LoginGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router) {}

  canActivate(): boolean | UrlTree {
    // Si el usuario YA está logueado...
    if (this.auth.isLoggedIn()) {
      // ...lo mandamos al dashboard y BLOQUEAMOS la entrada al login
      return this.router.createUrlTree(['/dashboard']);
    }
    
    // Si NO está logueado, dejamos que entre al login tranquilamente
    return true;
  }
}