import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { MenuService, MenuItem } from '../../core/services/menu.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule, RouterLink],
})
export class DashboardComponent implements OnInit {
  menuItems: MenuItem[] = [];
  userName: string = 'Usuario';

  constructor(
    private authService: AuthService,
    private menuService: MenuService
  ) {}

  ngOnInit(): void {
    const role = this.authService.getRole();
    this.menuItems = this.menuService.getMenuByRole(role).filter(item => item.label !== 'Inicio' );
    this.userName = this.authService.getUsername();
  }
}
