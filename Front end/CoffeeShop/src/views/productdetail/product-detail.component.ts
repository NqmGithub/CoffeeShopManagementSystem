import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../Api/api.service';
import { Product } from '../../Interfaces/product';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { AuthService } from '../../service/auth.service';
@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NavbarComponent
  ],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  productId: string = '';  // ID của sản phẩm hiện tại
  product: Product | null = null;  // Chi tiết sản phẩm
  relatedProducts: Product[] = [];  // Danh sách sản phẩm liên quan
  quantity: number = 1;  // Số lượng sản phẩm người dùng chọn

  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService,  // Dịch vụ API
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.productId = params.get('productId') || '';

      if (!this.productId) {
        console.error('Product ID is missing!');
        return;
      }

      this.loadProductDetail(this.productId);
      this.loadRelatedProducts(this.productId);
    });
  }

  loadProductDetail(productId: string): void {
    this.apiService.getProductById(productId).subscribe({
      next: (product) => {
        this.product = product;
      },
      error: (err) => {
        console.error('Error fetching product details:', err);
      }
    });
  }

  loadRelatedProducts(productId: string): void {
    this.apiService.getRelatedProducts(productId).subscribe({
      next: (relatedProducts) => {
        this.relatedProducts = relatedProducts;
      },
      error: (err) => {
        console.error('Error fetching related products:', err);
      }
    });
  }

  getImage(name: string): string {
    return `https://localhost:44344/wwwroot/Images/${name}`;
  }

  viewProductDetail(productId: string): void {
    this.router.navigate(['/productdetail', productId]);
  }

  increaseQuantity(): void {
    this.quantity++;
  }

  decreaseQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  // Thêm sản phẩm vào giỏ hàng
  addToCart(product: any): void {
    const userId = 'testUser'; // Sử dụng userId mặc định khi test
  
    let cart = this.apiService.getCartItems(userId);
    
    // Kiểm tra nếu sản phẩm đã tồn tại trong giỏ hàng
    const existingProduct = cart.find((item: any) => item.productId === product.productId);
  
    if (existingProduct) {
      existingProduct.quantity += product.quantity || 1;
    } else {
      cart.push({
        productId: product.productId,
        productName: product.productName,
        quantity: product.quantity || 1,
        price: product.price,
        thumbnail: product.thumbnail
      });
    }
  
    this.apiService.saveCartItems(userId, cart);
    alert('Product added to cart');
  }
  
  
}
