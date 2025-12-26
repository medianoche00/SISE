import { Component, Input } from '@angular/core';
import { AuthService } from '../../core/services/auth.service'; // Ajusta tu path
import { MatSidenav } from '@angular/material/sidenav';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  @Input() sidenav!: MatSidenav;
  username: string = '';

  constructor(private authService: AuthService) {}

  logout() {
    this.authService.logout();
  }

  ngOnInit(): void {
    this.username = this.authService.getUsername(); 
  }
}