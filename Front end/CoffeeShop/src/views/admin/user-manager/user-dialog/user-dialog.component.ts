import { Component, Inject } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { User } from '../../../../Interfaces/user';
import { ApiService } from '../../../../Api/api.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-user-dialog',
  standalone: true,
  imports: [
    MatDialogModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './user-dialog.component.html',
  styleUrl: './user-dialog.component.scss'
})
export class UserDialogComponent {

  header: string = "Add user"
  imageUrl: any;
  msg: string = '';
  isLoading = false;
  isUpdate = false;
  addUserForm: FormGroup;
  currentUser!: User;
  status: string = 'active';
  role: string = 'user';
  fileToUpload: File| null = null;
  fileExtension: string = '';

  constructor(@Inject(MAT_DIALOG_DATA) public data: User, private dialogRef: MatDialogRef<UserDialogComponent>, private api: ApiService) {
    this.addUserForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]),
      address: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(30)]),
      phoneNumber: new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(10)]),
      rePassword: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]),
      userName: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]),
    }, { validators: passwordMatchValidator });

    if (data) {
      this.currentUser = data;
      this.isUpdate = true;
      this.header = "Update user";
      const user = { ...data };
      this.addUserForm.setValue({
        email: user.email,
        userName: user.userName,
        address: user.address,
        phoneNumber: user.phoneNumber,
        rePassword: '',
        password: user.password
      });
    }
  }

  onCancel(){
    this.dialogRef.close();
  }

  getImage(path: string){
    return `https://localhost:44344/wwwroot/Avatars/${path}`;
  }

  onUpdate(){

  }

  onAdd(){
    this.isLoading = true;
    const email = this.addUserForm.value.email;
    const userName = this.addUserForm.value.userName;
    const address = this.addUserForm.value.address;
    const phoneNumber = this.addUserForm.value.phoneNumber
    const password = this.addUserForm.value.password;
    const user: User = {
      id: "",
      address: address,
      email: email,
      phoneNumber: phoneNumber,
      userName: userName,
      password: password,
      avatar: this.imageUrl,
      status: this.status == 'active' ? 1 : 2,
      role: 1
    }
    this.dialogRef.close(user); 
  }

  handleFileInput(event: Event){
    const input = event.target as HTMLInputElement;
    if (input && input.files && input.files.length > 0) {
      this.fileToUpload = input.files.item(0);
      
      // check file upload
      if (this.fileToUpload) {

        //get extension file
        const fileName = this.fileToUpload.name;
        const fileExtension = fileName.split('.').pop();
        this.fileExtension = fileExtension!;
        this.currentUser.avatar = this.currentUser.userName+'.'+this.fileExtension;

        let reader = new FileReader();
        reader.onload = (event: any) => {
          this.imageUrl = event.target.result;
        }
        reader.readAsDataURL(this.fileToUpload);
      } else {
        console.error("No file found or file is invalid.");
      }
    } else {
      console.error("File list is empty or input.files is null.");
    }
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