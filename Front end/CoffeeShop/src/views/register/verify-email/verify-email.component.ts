import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ApiService } from '../../../Api/api.service';
import { AuthService } from '../../../service/auth.service';
import { User } from '../../../Interfaces/user';
import { switchMap } from 'rxjs';

@Component({
  selector: 'app-verify-email',
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
  templateUrl: './verify-email.component.html',
  styleUrl: './verify-email.component.scss'
})
export class VerifyEmailComponent implements OnInit{

  constructor(private route: ActivatedRoute) {}

  currentUser!: User;

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
        this.currentUser = {
          id: params['id'] || '',
          address: params['address'] || '',
          email: params['email'] || '',
          phoneNumber: params['phoneNumber'] || '',
          userName: params['userName'] || '',
          password: params['password'] || '',
          avatar: "",
          status: 1,
          role: 1
        }
        this.api.sendOtp(this.currentUser.email);
      }   
    )
  }

  loginForm = new FormGroup({
    otp: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(6)])
  })
  msg: string = ''
  isLoadingSend = false;
  isLoadingCheck = false;
  api: ApiService = inject(ApiService);
  auth: AuthService = inject(AuthService);
  router: Router = inject(Router);
  

  sendOtp(){
    this.isLoadingSend = true;
    this.api.sendOtp(this.currentUser.email).subscribe(
      {
        error: (e) => {
          this.isLoadingSend = false
          this.msg = e.error.message;
          console.error("ERROR", e.error)
        },
        complete: () => {
          this.isLoadingSend =false;
          this.msg = "otp sent";
        }
      }
    )  
  }

  validateOtp() {
    this.msg = '';
    this.isLoadingCheck = true;
  
    const otpCode = this.loginForm.value.otp ?? '';
  
    this.api.validateOtp(this.currentUser.email, otpCode).pipe(
      switchMap(() => {
        // OTP validated, proceed to signup
        return this.auth.signup(this.currentUser);
      })
    ).subscribe({
      next: () => {
        this.isLoadingCheck = false;
        this.msg = 'Signup successful!';
      },
      error: (e) => {
        this.isLoadingCheck = false;
        this.msg = e.error?.message || 'An error occurred.';
        console.error('Error', e);
      }
    });
  }

  toHome(){
    this.router.navigate(['home']);
  }
}
