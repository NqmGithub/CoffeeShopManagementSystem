import { Component, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import {MatToolbarModule} from '@angular/material/toolbar';
import { SidebarService } from '../../service/common/sidebar.service';
import { Router,RouterModule } from '@angular/router';
import { AuthService } from '../../service/auth.service';
import { MatMenuModule } from '@angular/material/menu';
import { MatCardModule } from '@angular/material/card';
import { User } from '../../Interfaces/user';
import { MatButtonModule } from '@angular/material/button';
@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatIconModule,
    RouterModule,
    MatMenuModule,
    MatCardModule,
    MatButtonModule
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
  currentUser: User | null = null;
  ngOnInit(): void {
    this.router.events.subscribe(() => {
      this.isSidebarVisible = this.router.url.includes('admin');
    });
    this.auth.getCurrentUser().subscribe(u =>{
      this.currentUser = u;
    })
  }

  homeNavigate(){
    this.router.navigate(['home'])
  }
  
  loginNavigate(){
    this.router.navigate(['/login'])
  }

  signupNavigate(){
    this.router.navigate(['/register'])
  }

  profileNavigate(){
    this.router.navigate(['/profile'])
  }
  notificationNavigate(){
    this.router.navigate(['/notification'])
  }
  notificationHistory(){
    this.router.navigate(['/history'])
  }

  logout(){
    this.auth.logout();
  }
  toggleSideBar(){
    if(this.isSidebarVisible){
      this.sidebarService.toggleSidebar();
    }   
  }

  toProfile(){
    this.router.navigate(['profile'])
  }

  toDashBoard(){
    this.router.navigate(['admin'])
  }

  getImage(path: any){
    return `https://localhost:44344/wwwroot/Avatars/${path}`;
  }

}
