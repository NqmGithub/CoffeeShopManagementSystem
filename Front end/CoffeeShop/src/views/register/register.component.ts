import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../service/auth.service';
import { MatCardModule } from '@angular/material/card';
import { Router, RouterModule } from '@angular/router';
import { User } from '../../Interfaces/user';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatButton, ReactiveFormsModule, MatCardModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  signupForm = new FormGroup({
    email: new FormControl(''),
    password: new FormControl(''),
    rePassword: new FormControl(''),
    address: new FormControl(''),
    phoneNumber: new FormControl(''),
    userName: new FormControl('')
  })

  auth: AuthService = inject(AuthService);
  router: Router = inject(Router)
  signup(){
    const email = this.signupForm.value.email ?? '';
    const password = this.signupForm.value.password ?? '';
    const rePassword = this.signupForm.value.rePassword ?? '';
    const userName = this.signupForm.value.userName ?? '';
    const address = this.signupForm.value.address ?? '';
    const phoneNumber = this.signupForm.value.phoneNumber ?? '';
    if(rePassword != password){
      alert("Password does not match");
      return;
    }
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
    this.auth.signup(user)
  }
}
