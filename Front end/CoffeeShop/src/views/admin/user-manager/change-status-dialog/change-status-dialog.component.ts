import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { User } from '../../../../Interfaces/user';

@Component({
  selector: 'app-change-status-dialog',
  standalone: true,
  imports: [
    MatIconModule,
    MatDialogModule,
    MatButtonModule,
    CommonModule
  ],
  templateUrl: './change-status-dialog.component.html',
  styleUrl: './change-status-dialog.component.scss'
})
export class ChangeStatusDialogComponent {
  user:User;
  constructor(@Inject(MAT_DIALOG_DATA) public data: User,private dialogRef: MatDialogRef<ChangeStatusDialogComponent>) {
    this.user = {...data};
  }

  onCancel(): void {
    this.dialogRef.close(); 
  }

  onConfirm(): void {
    this.dialogRef.close(this.user.status == 1 ?'InActive':'Active');
  }
}
