import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { Product } from '../../../../Interfaces/product';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-delete-product-dialog',
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
  product:Product;
  constructor(@Inject(MAT_DIALOG_DATA) public data: Product,private dialogRef: MatDialogRef<ChangeStatusDialogComponent>) {
    this.product = {...data};
  }

  onCancel(): void {
    this.dialogRef.close(); 
  }

  onConfirm(): void {
    this.dialogRef.close(this.product.status=='Active'?'InActive':'Active');
  }
}
