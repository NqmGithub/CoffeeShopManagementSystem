import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router, RouterModule } from '@angular/router';
import { ApiService } from '../../../Api/api.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatInputModule, 
    MatButton, 
    ReactiveFormsModule, 
    MatCardModule, 
    RouterModule
  ],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent {
  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email])
  })
  msg: string = ''
  isLoading = false;
  api: ApiService = inject(ApiService);
  router: Router = inject(Router);
  
  sendPass(){
    this.isLoading = true;
    const email = this.loginForm.value.email ?? '';
    this.api.resetPassword(email).subscribe({
      next: (result) =>{ 
        this.msg = "New password sent to email";
      },
      complete:() => {
        this.isLoading = false;
      },    
      error:(e) => {
        console.error("error", e);
        this.msg = e;
        this.isLoading = false
      }
    }); 
    
  }

  toHome(){
    this.router.navigate(['home']);
  }
}
