import { Component, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import {MatToolbarModule} from '@angular/material/toolbar';
import { SidebarService } from '../../service/common/sidebar.service';
import { Router,RouterModule } from '@angular/router';
@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatIconModule,
    RouterModule
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit {
  isSidebarVisible: boolean = true;
  constructor(private sidebarService: SidebarService, private router: Router) {
    
  }
  ngOnInit(): void {
    this.router.events.subscribe(() => {
      this.isSidebarVisible = this.router.url.includes('admin');
    });
  }
  
  loginNavigate(){
    this.router.navigate(['admin'])
  }
  toggleSideBar(){
    if(this.isSidebarVisible){
      this.sidebarService.toggleSidebar();
    }   
  }
}
