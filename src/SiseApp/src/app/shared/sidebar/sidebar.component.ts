import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { MenuService, MenuItem } from '../../core/services/menu.service';
import { MatNavList } from '@angular/material/list';
import { MatSidenav } from '@angular/material/sidenav';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent implements OnInit {
  @Input() sidenav!: MatSidenav;
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
  closeSidebar(item: MenuItem) {
    if (/*item.closeSidenav && */this.sidenav) {
      this.sidenav.close();
    }
  }
}
