import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog'; 
import { Order, OrdersResponse } from '../../../../Interfaces/order';  
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ChangeStatusDialogComponent } from '../change-status-dialog/change-status-dialog,component';
import { ApiService } from '../../../../Api/api.service';
import { MatIconModule } from '@angular/material/icon';
@Component({
  selector: 'app-order-manager',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule,  
    MatTableModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatSortModule,
    MatPaginator,
    MatIconModule
  ],
  
  templateUrl: './order-manager.component.html',
  styleUrls: ['./order-manager.component.scss']
})
export class OrderManagerComponent implements OnInit {
  displayedColumns: string[] = ['OrderId', 'OrderDate', 'CustomerName', 'Status', 'TotalPrice', 'Actions'];
  orders: Order[] = []; 
  ordersData = new MatTableDataSource<Order>();  
  totalOrders: number = 0; 
  pageSize: number = 10;  
  pageNumber: number = 1; 
  search: string = '';  
  sortColumn: string = 'OrderDate';  
  sortDirection: string = 'asc'; 
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  constructor(private dialog: MatDialog, private apiService: ApiService) {}

  ngOnInit(): void {
    this.ordersData.sort = this.sort;
    this.ordersData.paginator = this.paginator;
    this.loadOrders();
  }
  onChange(){
    this.loadOrders();
  }
  onSortChange(){
    this.sortColumn= this.sort.active;
    this.sortDirection = this.sort.direction;
    this.loadOrders();
  }
  
  onPageChange(event: PageEvent): void {
    console.log(event.pageSize);
    this.pageNumber = event.pageIndex + 1;  // Make it 1-based
    this.pageSize = event.pageSize;
    this.loadOrders();
  }  
 
  loadOrders() {
    this.apiService.getOrders(
      this.search, 
      '', 
      this.sortColumn, 
      this.sortDirection, 
      this.pageNumber, 
      this.pageSize
    ).subscribe(
      (response: { list: Order[], total: number }) => {
        this.orders = response.list; // Access the 'list' of orders from the response
        this.ordersData.data = this.orders; // Use the 'list' in your data table or display logic
        this.totalOrders = response.total; // Set the total number of orders for pagination
      },
      (error) => {
        console.error('Error fetching orders:', error);
      }
    );
  }
  

  // Load the total number of orders to manage pagination
  loadTotalOrders(): void {
    this.apiService.getOrders(
      this.search, 
      '', 
      this.sortColumn, 
      this.sortDirection, 
      1, 
      10000
    ).subscribe(
      (response: { list: Order[], total: number }) => {
        this.totalOrders = response.total;  // Use 'total' directly from the response
      },
      (error) => {
        console.error('Error fetching total orders:', error);
      }
    );
  }
  

  openChangeStatusDialog(order: any): void {
    // Logic để xác định newStatus
    let newStatus: number;
    if (order.status === 0) {
      newStatus = 1;  // Chuyển từ "Pending" (0) sang "Completed" (1)
    } else if (order.status === 1) {
      newStatus = 2;  // Chuyển từ "Completed" (1) sang "Cancelled" (2)
    } else {
      newStatus = 0;  // Nếu đã bị "Cancelled", có thể quay lại "Pending"
    }
  
    const dialogRef = this.dialog.open(ChangeStatusDialogComponent, {
      data: order
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadOrders(); // Reload orders if status changed successfully
      }
    });
  }
  
  
  viewDetail(order: Order) {
  }
}
 


