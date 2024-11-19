import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ApiService } from '../../../../Api/api.service';
import { Order } from '../../../../Interfaces/order';

@Component({
  selector: 'app-change-status-dialog',
  templateUrl: './change-status-dialog.component.html',
  styleUrls: ['./change-status-dialog.component.scss']
})
export class ChangeStatusDialogComponent {
  // Khai báo các biến nhận dữ liệu từ dialog
  orderId: string;
  //currentStatus: number;
  //newStatus: string;

  constructor(
    public dialogRef: MatDialogRef<ChangeStatusDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Order,
    private apiService: ApiService
  ) {
    // Lấy dữ liệu từ MAT_DIALOG_DATA và gán vào các biến
    this.orderId = data.id;
   // this.currentStatus = data.status;
    //this.newStatus = data.newStatus;
  }

  // Cập nhật trạng thái đơn hàng
  changeStatus(): void {
    alert("string");
    // Gọi API để cập nhật trạng thái đơn hàng
    this.apiService.updateOrderStatus(this.orderId).subscribe({
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

