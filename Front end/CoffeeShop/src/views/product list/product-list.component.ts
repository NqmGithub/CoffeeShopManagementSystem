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
import { MatIconModule } from '@angular/material/icon';
import { User } from '../../Interfaces/user';
@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatPaginator,
    NavbarComponent,
    MatIconModule
  ],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  stars = [1, 2, 3, 4, 5];
  products: Product[] = [];
  listCategoryName: string[] = [];
  tempQuantity: { [key: string]: number } = {};
  defaultQuantity: number = 1;
  
  // Pagination
  totalRecords: number = 0;
  totalPages: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;

  // Filters and Sorting
  search: string = '';
  categoryFilter: string = ''; // Category filter
  minPrice: number = 0;
  maxPrice: number = 1000000;
  sortColumn: string = '';
  sortDirection: boolean = false;
  Math: any;
  user:User|null = null;
  constructor(private router: Router, private apiService: ApiService, private authService: AuthService ) {
    this.authService.getCurrentUser().subscribe(
      res =>{
        this.user = res;
      }
    )
  }

  ngOnInit(): void {
    this.loadCategories();
    this.loadProducts();
  }

  getImage(name: string) {
    return `https://localhost:44344/wwwroot/Images/${name}`;
  }

  loadCategories(): void {
    this.apiService.getAllCateogryNames().subscribe((categories: string[]) => {
      this.listCategoryName = categories; // Gán trực tiếp nếu API trả về mảng chuỗi
    });
  }
  
  loadProducts(): void {
    this.apiService
      .getProductLists(
        this.search,
        this.categoryFilter || '',  // Sử dụng categoryFilter đã được chọn
        this.minPrice,
        this.maxPrice,
        this.currentPage,
        this.pageSize,
        this.sortColumn,
        this.sortDirection
      )
      .subscribe((response) => {
        this.products = response.list;
        this.products.forEach((product) => {
          this.tempQuantity[product.id] = this.tempQuantity[product.id] || 1;
        });
        this.totalRecords = response.total;
        this.totalPages = Math.ceil(this.totalRecords / this.pageSize);
      });
  }

  onSearchChange(): void {
    this.currentPage = 1;
    this.loadProducts();
  }

  onCategoryChange(): void {
    this.currentPage = 1;
    this.loadProducts();  // Reload products khi category thay đổi
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadProducts();
  }

  onSort(column: string): void {
    if (this.sortColumn === column) {
      this.sortDirection = !this.sortDirection; // Nếu đã sắp xếp theo cột này, đảo chiều
    } else {
      this.sortColumn = column;
      this.sortDirection = false; 
    }
    this.loadProducts(); 
  }

  addToCart(product: any): void {
    const userId = this.user?.id; // Sử dụng userId mặc định khi test
    let cart = this.apiService.getCartItems(userId!);

    const existingProduct = cart.find((item: any) => item.productId === product.id);
    const quantityToAdd = this.tempQuantity[product.id] || 1;

    if (existingProduct) {
      existingProduct.quantity += quantityToAdd;
    } else {
      cart.push({
        productId: product.id,
        productName: product.productName,
        quantity: quantityToAdd,
        price: product.price,
        thumbnail: product.thumbnail,
      });
    }

    this.apiService.saveCartItems(userId!, cart);
    alert('Product added to cart');
    this.tempQuantity[product.id] = 1;
  }
  viewProductDetail(productId: string): void {
    this.router.navigate(['/productdetail', productId]);
  }
  onQuantityChange(productId: string): void {
    if (this.tempQuantity[productId] < 1) {
      this.tempQuantity[productId] = 1;  // Đảm bảo số lượng không dưới 1
    }
  }
}
