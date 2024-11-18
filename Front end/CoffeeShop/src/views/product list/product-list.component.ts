import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../Api/api.service';
import { Product } from '../../Interfaces/product';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatPaginator
  ],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  categories: string[] = [];
  
  // Pagination
  totalRecords: number = 0;
  totalPages: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;

  // Filters and Sorting
  search: string = '';
  categoryFilter: string = '';
  minPrice: number = 0;
  maxPrice: number = 1000000;
  sortColumn: string = '';
  sortDirection: boolean = false;

  constructor(private router: Router, private apiService: ApiService) {}

  ngOnInit(): void {
    this.loadCategories();
    this.loadProducts();
  }

  loadCategories(): void {
    this.apiService.getAllCategoryNames().subscribe((categories) => {
      this.categories = categories.map((cat: any) => cat.name);
    });
  }

  loadProducts(): void {
    this.apiService
      .getProductLists(
        this.search,
        this.categoryFilter || "",
        this.minPrice,
        this.maxPrice,
        this.currentPage,
        this.pageSize,
        this.sortColumn,
        this.sortDirection
      )
      .subscribe((response) => {
        this.products = response.list;
        this.totalRecords = response.total;
        this.totalPages = Math.ceil(this.totalRecords / this.pageSize);
      });
  }

  toggleSortDirection(): void {
    this.sortDirection = !this.sortDirection;
    this.loadProducts();
  }
  
  onSearchChange(): void {
    this.currentPage = 1;
    this.loadProducts();
  }

  onCategoryChange(): void {
    this.currentPage = 1;
    this.loadProducts();
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadProducts();
  }

  onSort(column: string): void {
    if (this.sortColumn === column) {
      this.sortDirection = !this.sortDirection;
    } else {
      this.sortColumn = column;
      this.sortDirection = false;  // Default to ascending
    }
    this.loadProducts();
  }

  getPaginationArray(): number[] {
    return Array(this.totalPages).fill(0).map((x, i) => i + 1);
  }
  viewProductDetail(productId: string) {
    this.router.navigate(['/productdetail', productId]);
  }

  // Tăng số lượng sản phẩm
  increaseQuantity(product: any) {
    product.quantity = (product.quantity || 0) + 1;
  }

  // Giảm số lượng sản phẩm
  decreaseQuantity(product: any) {
    if (product.quantity > 1) {
      product.quantity = (product.quantity || 1) - 1;
    }
  }

// Thêm sản phẩm vào giỏ hàng (lưu vào cookie)
addToCart(product: any): void {
    // Lấy giỏ hàng từ cookie, nếu chưa có thì khởi tạo là một mảng trống
    let cart: any[] = JSON.parse(this.apiService.getCookie('cart') || '[]');
  
    // Kiểm tra xem sản phẩm đã có trong giỏ chưa
    const existingProduct = cart.find((item: any) => item.productId === product.productId);
  
    if (existingProduct) {
      // Cập nhật số lượng nếu sản phẩm đã có trong giỏ
      existingProduct.quantity += product.quantity;
    } else {
      // Thêm sản phẩm mới vào giỏ
      cart.push({
        productId: product.productId,
        productName: product.productName,
        quantity: product.quantity,
        price: product.price,
        thumbnail: product.thumbnail
      });
    }
  
    // Lưu giỏ hàng vào cookie (giới hạn thời gian cookie nếu cần)
    this.apiService.setCookie('cart', JSON.stringify(cart), 7); // Giữ cookie trong 7 ngày
  }
  
}
