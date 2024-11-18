import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { User } from '../../../../Interfaces/user';
import { UpdateProfile } from '../../../../Interfaces/updateProfile';
import { ApiService } from '../../../../Api/api.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';  // Import CommonModule here

@Component({
  selector: 'app-update-profile',
  standalone: true,
  imports: [
    CommonModule,  // Add CommonModule here
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule, 
    FormsModule
  ],
  templateUrl: './update-profile.component.html',
  styleUrls: ['./update-profile.component.scss']
})
export class UpdateProfileComponent implements OnInit {
  updatedProfile: UpdateProfile = {
    userName: '',
    phoneNumber: '',
    address: '',
    avatar: '',
  };

  selectedFile: File | null = null;
  previewUrl: string | null = null;

  constructor(
    public dialogRef: MatDialogRef<UpdateProfileComponent>,
    @Inject(MAT_DIALOG_DATA) public data: User,
    private authService: ApiService
  ) {}

  ngOnInit(): void {
    this.updatedProfile.userName = this.data.userName;
    this.updatedProfile.phoneNumber = this.data.phoneNumber;
    this.updatedProfile.address = this.data.address;
  }
  onFileSelected(event: Event): void {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      this.selectedFile = fileInput.files[0];
  
      const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];
      if (!allowedTypes.includes(this.selectedFile.type)) {
        alert('Please select a valid image file (JPG, PNG, GIF).');
        return;
      }
      const maxSize = 5 * 1024 * 1024; 
      if (this.selectedFile.size > maxSize) {
        alert('File size exceeds the 5 MB limit.');
        return;
      }
      console.log('Selected file:', this.selectedFile);


      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.previewUrl = e.target.result;
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }
  
  saveChanges(): void {
    // Input validation
    if (this.updatedProfile.userName.length < 3 || this.updatedProfile.userName.length > 20) {
      alert('UserName must be between 3 and 20 characters.');
      return;
    }
    if (this.updatedProfile.phoneNumber.length !== 10) {
      alert('Phone number must be exactly 10 digits.');
      return;
    }
    if (this.updatedProfile.address.length < 2 || this.updatedProfile.address.length > 30) {
      alert('Address must be between 2 and 30 characters.');
      return;
    }
  
    const formData = new FormData();
    formData.append('UserName', this.updatedProfile.userName);
    formData.append('PhoneNumber', this.updatedProfile.phoneNumber);
    formData.append('Address', this.updatedProfile.address);
  
    if (this.selectedFile) {
      formData.append('Avatar', this.selectedFile, this.selectedFile.name);
    }
    console.log('FormData:', formData);

    this.authService.updateProfile(this.data.id, formData).subscribe({
      next: () => {
        alert('Profile updated successfully!');
        this.dialogRef.close(this.updatedProfile);
      },
      error: (err) => {
        console.error('Failed to update profile:', err);
        alert('An error occurred while updating the profile. Please try again later.');
      }
    });
  }
  

  getImage(name: string){
    return `https://localhost:44344/wwwroot/Avatars/${name}`;
  }
  
  cancel(): void {
    this.dialogRef.close();
  }
}
