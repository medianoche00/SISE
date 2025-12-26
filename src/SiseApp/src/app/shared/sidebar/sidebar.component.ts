import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { MenuService, MenuItem } from '../../core/services/menu.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  menuItems: MenuItem[] = [];
  role: string = '';

  constructor(
    private authService: AuthService,
    private menuService: MenuService
  ) {}

  ngOnInit(): void {
    this.role = this.authService.getRole(); 
    this.menuItems = this.menuService.getMenuByRole(this.role);
  }
}