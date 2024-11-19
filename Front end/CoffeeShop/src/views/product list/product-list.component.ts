import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../Api/api.service';
import { Product } from '../../Interfaces/product';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { AuthService } from '../../service/auth.service';
import { Category } from '../../Interfaces/category';
@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatPaginator,
    NavbarComponent
  ],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  categories: Category[] = [];
  tempQuantity: { [key: string]: number } = {};
  defaultQuantity: number = 1;
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
Math: any;

  constructor(private router: Router, private apiService: ApiService, private authService: AuthService ) {}

  ngOnInit(): void {
    this.loadCategories();
    this.loadProducts();
  }

  getImage(name: string) {
    return `https://localhost:44344/wwwroot/Images/${name}`;
  }

  loadCategories(): void {
    this.apiService.getAllCateogryNames().subscribe((categories) => {
      this.categories = categories.map((cat: any) => cat.categoryName);
    });
  }
  

  loadProducts(): void {
    this.apiService
      .getProductLists(
        this.search,
        this.categoryFilter || '',
        this.minPrice,
        this.maxPrice,
        this.currentPage,
        this.pageSize,
        this.sortColumn,
        this.sortDirection
      )
      .subscribe((response) => {
        this.products = response.list;
        // Đặt số lượng mặc định cho biến tạm thời
        this.products.forEach((product) => {
          this.tempQuantity[product.id] = this.tempQuantity[product.id] || 1;
        });
        this.totalRecords = response.total;
        this.totalPages = Math.ceil(this.totalRecords / this.pageSize);
      });
  }
  

  toggleSortDirection(): void {
    this.sortDirection = !this.sortDirection;
    this.loadProducts();
    this.loadCategories();
  }

  onSearchChange(): void {
    this.currentPage = 1;
    this.loadProducts();
    this.loadCategories();
  }

  onCategoryChange(): void {
    this.currentPage = 1;
    this.loadProducts();
    this.loadCategories();
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadProducts();
    this.loadCategories();
  }

  onSort(column: string): void {
    if (this.sortColumn === column) {
      this.sortDirection = !this.sortDirection;
    } else {
      this.sortColumn = column;
      this.sortDirection = false; // Default to ascending
    }
    this.loadProducts();
    this.loadCategories();
  }

  getPaginationArray(): number[] {
    return Array(this.totalPages).fill(0).map((x, i) => i + 1);
  }

  viewProductDetail(productId: string): void {
    this.router.navigate(['/productdetail', productId]);
  }
  setQuantity(product: any): void {
    // Nếu tempQuantity chưa được gán, thiết lập số lượng mặc định là 1
    if (!this.tempQuantity[product.id]) {
      this.tempQuantity[product.id] = this.defaultQuantity;
    }
  }
 
  // Tăng số lượng sản phẩm
  increaseQuantity(product: any): void {
    product.quantity = (product.quantity || 0) + 1;
  }

  // Giảm số lượng sản phẩm
  decreaseQuantity(product: any): void {
    if (product.quantity > 1) {
      product.quantity = (product.quantity || 1) - 1;
    }
  }
  onQuantityChange(productId: string): void {
    if (this.tempQuantity[productId] < 1) {
      this.tempQuantity[productId] = 1;  // Đảm bảo số lượng không dưới 1
    }
  }
  addToCart(product: any): void {
    const userId = 'testUser'; // Sử dụng userId mặc định khi test
    let cart = this.apiService.getCartItems(userId);
  
    // Tìm kiếm sản phẩm trong giỏ hàng
    const existingProduct = cart.find((item: any) => item.productId === product.id);
  
    // Sử dụng `tempQuantity` làm số lượng sản phẩm thêm vào giỏ hàng
    const quantityToAdd = this.tempQuantity[product.id] || 1;
  
    if (existingProduct) {
      // Nếu sản phẩm đã tồn tại trong giỏ hàng, tăng số lượng
      existingProduct.quantity += quantityToAdd;
    } else {
      // Nếu sản phẩm chưa tồn tại, thêm sản phẩm mới
      cart.push({
        productId: product.id,
        productName: product.productName,
        quantity: quantityToAdd,
        price: product.price,
        thumbnail: product.thumbnail,
      });
    }
  
    // Lưu giỏ hàng đã cập nhật vào ApiService
    this.apiService.saveCartItems(userId, cart);
    alert('Product added to cart');
    // Đặt lại `tempQuantity` về 1 sau khi thêm vào giỏ hàng
    this.tempQuantity[product.id] = 1;
  }
  
  }
  

