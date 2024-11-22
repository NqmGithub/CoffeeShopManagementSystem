import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { ApiService } from '../../../../Api/api.service';
import { Order } from '../../../../Interfaces/order';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-change-status-dialog',
  standalone: true,
  imports: [
    MatDialogModule,
    MatButtonModule,
    CommonModule
  ],
  templateUrl: './change-status-dialog.component.html',
  styleUrls: ['./change-status-dialog.component.scss']
})
export class ChangeStatusDialogComponent {
  // Khai báo các biến nhận dữ liệu từ dialog
  order!: Order

  constructor(@Inject(MAT_DIALOG_DATA) public data: Order,private dialogRef: MatDialogRef<ChangeStatusDialogComponent>, private apiService: ApiService) {
    this.order = {...data};
  }

  // Cập nhật trạng thái đơn hàng
  changeStatus(): void {
    // Gọi API để cập nhật trạng thái đơn hàng
    this.apiService.updateOrderStatus(this.order.id).subscribe({
      next: (response) => {
        console.log('Status updated successfully:', response);
        this.dialogRef.close(true); // Đóng dialog và thông báo thành công
      },
      error: (err) => {
        console.error('Error updating order status:', err);
        this.dialogRef.close(false); // Đóng dialog và thông báo lỗi
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}

