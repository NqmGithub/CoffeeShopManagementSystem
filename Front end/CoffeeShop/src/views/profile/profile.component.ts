import { Component, inject, OnInit } from '@angular/core';
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
export class ProfileComponent implements OnInit{

  constructor(private auth: AuthService){}
  currentUser: User | null = null;
  
  ngOnInit(): void {
    this.auth.currentUser$.subscribe(user => this.currentUser = user);
    // this.currentUser = this.auth.getCurrentUser();
  }
}
