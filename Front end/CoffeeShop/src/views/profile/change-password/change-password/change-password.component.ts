import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { User } from '../../../../Interfaces/user';
import { ApiService } from '../../../../Api/api.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormBuilder, FormGroup, FormsModule, Validators ,ReactiveFormsModule} from '@angular/forms';
import { CommonModule } from '@angular/common';  
import { ChangePassword } from '../../../../Interfaces/changePassword';
@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [
    CommonModule,  
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule, 
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss'
})
export class ChangePasswordComponent {
  changePasswordForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private api: ApiService,
    private dialogRef: MatDialogRef<ChangePasswordComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { userId: string }
  ) {
    this.changePasswordForm = this.fb.group(
      {
        currentPassword: ['', Validators.required],
        newPassword: [
          '',
          [
            Validators.required,
            Validators.minLength(6),
            Validators.maxLength(20),
          ],
        ],
        confirmNewPassword: ['', Validators.required],
      },
      {
        validators: this.passwordMatchValidator,
      }
    );
  }

  // Custom validator to check if newPassword and confirmNewPassword match
  passwordMatchValidator(formGroup: FormGroup): { [key: string]: boolean } | null {
    const newPassword = formGroup.get('newPassword');
    const confirmNewPassword = formGroup.get('confirmNewPassword');

    return newPassword && confirmNewPassword && newPassword.value !== confirmNewPassword.value
      ? { passwordsMismatch: true }
      : null;
  }

  onSubmit(): void {
    if (this.changePasswordForm.valid) {
      const formValue = this.changePasswordForm.value;

      this.api.changePassword(this.data.userId, formValue).subscribe({
        next: () => {
          alert('Password changed successfully!');
          this.dialogRef.close(true);
        },
        error: (err) => {
          alert(err.error.message || 'Failed to change password');
        },
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}