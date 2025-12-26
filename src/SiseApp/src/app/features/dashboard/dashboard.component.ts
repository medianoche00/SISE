import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { MenuService, MenuItem } from '../../core/services/menu.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  menuItems: MenuItem[] = [];
  userName: string = 'Usuario'; // Opcional: para personalizar el saludo

  constructor(
    private authService: AuthService,
    private menuService: MenuService
  ) {}

  ngOnInit(): void {
    const role = this.authService.getRole();
    this.menuItems = this.menuService.getMenuByRole(role);
    this.userName = this.authService.getUsername();
  }
}