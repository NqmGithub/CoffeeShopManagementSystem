import { Component, Input, OnInit } from '@angular/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatList, MatListItem, MatNavList } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { SidebarService } from '../../service/common/sidebar.service';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    MatSidenavModule,
    MatList,
    MatNavList,
    MatListItem,
    MatDividerModule,
    MatIconModule,
    RouterOutlet,
    RouterModule,
    MatDividerModule
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent{
  isSidebarOpen: boolean = false;
  constructor(private sidebarService: SidebarService,private router: Router) {    
  }
  ngOnInit(): void {
    this.sidebarService.isSidebarOpen$.subscribe(
      isOpen => this.isSidebarOpen = isOpen
    );   
  }

}
