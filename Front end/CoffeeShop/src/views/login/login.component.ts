import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../service/auth.service';
import { MatCardModule } from '@angular/material/card';
import { Router, RouterModule } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatProgressSpinnerModule, MatFormFieldModule, MatInputModule, MatButton, ReactiveFormsModule, MatCardModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required])
  })
  msg: string = ''
  isLoading = false;
  auth: AuthService = inject(AuthService);
  router: Router = inject(Router);
  
  login(){
    this.isLoading = true;
    const email = this.loginForm.value.email ?? '';
    const password = this.loginForm.value.password ?? '';
    this.auth.login(email, password).subscribe({
      next:() => {
        localStorage.setItem("email", email)
        this.isLoading = false
      },    
      error:(e) => {
        this.msg = e;
        this.isLoading = false
      }
    }); 
    
  }

  toHome(){
    this.router.navigate(['home']);
  }
}
