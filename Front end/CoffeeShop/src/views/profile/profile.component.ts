import { Component, inject } from '@angular/core';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { MatCardModule } from '@angular/material/card';

import { User } from '../../Interfaces/user';
import { AuthService } from '../../service/auth.service';
@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [NavbarComponent, MatCardModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  auth: AuthService = inject(AuthService)
  currentUser!: User | null;

  constructor(){
    this.auth.currentUser$.subscribe(user => this.currentUser = user);
  }
}
