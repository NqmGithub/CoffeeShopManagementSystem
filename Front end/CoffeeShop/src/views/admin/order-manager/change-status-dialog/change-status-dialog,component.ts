import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ApiService } from '../../../../Api/api.service';

@Component({
  selector: 'app-change-status-dialog',
  templateUrl: './change-status-dialog.component.html',
  styleUrls: ['./change-status-dialog.component.scss']
})
export class ChangeStatusDialogComponent {
  orderId: string;
  currentStatus: string;
  newStatus: string;

  constructor(
    public dialogRef: MatDialogRef<ChangeStatusDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private apiService: ApiService  
  ) {
    this.orderId = data.orderId;
    this.currentStatus = data.currentStatus;
    this.newStatus = data.newStatus;  
  }

  // Change order status
  changeStatus(): void {
    // Gọi API để cập nhật trạng thái đơn hàng
    this.apiService.updateOrderStatus(this.orderId).subscribe({
      next: () => {
        this.dialogRef.close(true); // Đóng dialog và thông báo thành công
      },
      error: () => {
        this.dialogRef.close(false); // Đóng dialog và thông báo lỗi
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close(); // Đóng dialog khi hủy
  }
}



