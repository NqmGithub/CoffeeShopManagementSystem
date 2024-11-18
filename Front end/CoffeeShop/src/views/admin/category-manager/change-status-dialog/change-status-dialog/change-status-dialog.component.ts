import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { Category } from '../../../../../Interfaces/category';
import { CommonModule } from '@angular/common';

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
  category:Category;
  constructor(@Inject(MAT_DIALOG_DATA) public data: Category,private dialogRef: MatDialogRef<ChangeStatusDialogComponent>) {
    this.category = {...data};
  }

  onCancel(): void {
    this.dialogRef.close(); 
  }

  onConfirm(): void {
    if (this.category.status == 1) {
      this.dialogRef.close(0);
    } else if (this.category.status == 0) {
      this.dialogRef.close(1);
    }
  }
  
    
}

