import { Component, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import {MatToolbarModule} from '@angular/material/toolbar';
import { SidebarService } from '../../service/common/sidebar.service';
import { Router,RouterModule } from '@angular/router';
import { AuthService } from '../../service/auth.service';
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
  isLoggedIn:boolean;
  constructor(private sidebarService: SidebarService, private router: Router, private auth: AuthService) {
    this.isLoggedIn = this.auth.isLoggedIn();
  }
  ngOnInit(): void {
    this.router.events.subscribe(() => {
      this.isSidebarVisible = this.router.url.includes('admin');
    });
  }

  
  loginNavigate(){
    this.router.navigate(['/login'])
  }

  signupNavigate(){
    this.router.navigate(['/register'])
  }

  profileNavigate(){
    
  }

  logout(){
    this.auth.logout();
  }
  toggleSideBar(){
    if(this.isSidebarVisible){
      this.sidebarService.toggleSidebar();
    }   
  }
}
