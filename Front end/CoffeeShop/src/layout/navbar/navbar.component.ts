import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import {MatToolbarModule} from '@angular/material/toolbar';
import { AuthService } from '../../service/auth.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatIconModule,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {

  route: Router = inject(Router)
  auth: AuthService = inject(AuthService)
  isLoggedIn = this.auth.isLoggedIn();
  
  loginNavigate(){
    this.route.navigate(['/login'])
  }

  signupNavigate(){
    this.route.navigate(['/register'])
  }

  profileNavigate(){
    
  }

  logout(){
    this.auth.logout();
  }
}
