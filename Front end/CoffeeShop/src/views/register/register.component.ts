import { Component, inject } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../service/auth.service';
import { MatCardModule } from '@angular/material/card';
import { Router, RouterModule } from '@angular/router';
import { User } from '../../Interfaces/user';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ApiService } from '../../Api/api.service';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [MatProgressSpinnerModule, MatFormFieldModule, MatInputModule, MatButton, ReactiveFormsModule, MatCardModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  signupForm: FormGroup;
  constructor(){
    this.signupForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]),
      rePassword: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]),
      address: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(30)]),
      phoneNumber: new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(10)]),
      userName: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(20)])
    }, { validators: passwordMatchValidator });
  }

  msg: string = '';
  isLoading = false;

  auth: AuthService = inject(AuthService);
  api: ApiService = inject(ApiService);
  router: Router = inject(Router);
  signup(){
    this.isLoading = true;
    const email = this.signupForm.value.email ?? '';
    const password = this.signupForm.value.password ?? '';
    const userName = this.signupForm.value.userName ?? '';
    const address = this.signupForm.value.address ?? '';
    const phoneNumber = this.signupForm.value.phoneNumber ?? '';
    const user: User = {
      id: "",
      address: address,
      email: email,
      phoneNumber: phoneNumber,
      userName: userName,
      password: password,
      avatar: "",
      status: 1,
      role: 1
    }
    this.api.checkEmail(email).subscribe({
      complete:() => {
        this.router.navigate(['verifyEmail'], {
          queryParams: {
            id: user.id,
            address: user.address,
            email: user.email,
            phoneNumber: user.phoneNumber,
            userName: user.userName,
            password: user.password
          }
      })
      },
      error: e =>{
        this.msg = e.error;
        this.isLoading = false;
      }
    })
  }

  toHome(){
    this.router.navigate(['home']);
  }
}

function passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
  const password = control.get('password')?.value;
  const rePassword = control.get('rePassword')?.value;

  if (password !== rePassword) {
    return { passwordMismatch: true };
  }
  return null;
}