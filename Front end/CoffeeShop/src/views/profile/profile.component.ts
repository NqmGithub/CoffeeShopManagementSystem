import { Component, inject, OnInit } from '@angular/core';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { MatCardModule } from '@angular/material/card';
import { UpdateProfile } from '../../Interfaces/updateProfile';
import { User } from '../../Interfaces/user';
import { AuthService } from '../../service/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { UpdateProfileComponent } from './update-profile/update-profile/update-profile.component';
import { ChangePasswordComponent } from './change-password/change-password/change-password.component';
@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [NavbarComponent, MatCardModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit{

  constructor(private dialog: MatDialog, private auth: AuthService){}
  currentUser: User | null = null;
  
  ngOnInit(): void {
    this.auth.getCurrentUser().subscribe(u =>{
      this.currentUser = u;
    })
  }
  getImage(name: string){
    return `https://localhost:44344/wwwroot/Avatars/${name}`;
  }
  openUpdateProfileDialog(): void {
    const dialogRef = this.dialog.open(UpdateProfileComponent, {
      width: '400px',
      data: this.currentUser 
    });
  
    dialogRef.afterClosed().subscribe(updatedProfile => {
      if (updatedProfile) {
        this.currentUser = { ...this.currentUser, ...updatedProfile };
      }
    });
  }
  updateProfile() {
    const dialogRef = this.dialog.open(UpdateProfileComponent, {
      width: '600px',
      data: { ...this.currentUser } 
    });
  
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.currentUser = { ...this.currentUser, ...result };
        console.log('Profile updated:', this.currentUser);
      }
    });
  }
  openChangePasswordDialog(): void {
    const dialogRef = this.dialog.open(ChangePasswordComponent, {
      width: '600px',
      data: { userId: this.currentUser?.id } 
    });
  
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Password changed successfully');
      }
    });
  }
  
}