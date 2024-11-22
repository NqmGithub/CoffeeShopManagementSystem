import { Component } from '@angular/core';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { SliderComponent } from './slider/slider.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { Product } from '../../Interfaces/product';
import { ApiService } from '../../Api/api.service';
import { MatIconModule } from '@angular/material/icon';
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    NavbarComponent,
    SliderComponent,
    MatCardModule,
    MatButtonModule,
    RouterModule,
    MatIconModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent  {
  stars = [1, 2, 3, 4, 5];
  listBestSellerProducts: Product[] =[];

  constructor(private apiService: ApiService) {
    this.apiService.getTop3BestSeller().subscribe(
      response => {
        this.listBestSellerProducts = response;
      },
      error =>{
        console.log(error);
      }
    )    
  }

  getImage(name: string){
    return `https://localhost:44344/wwwroot/Images/${name}`;
  }

  addToCart(product: any): void {
    const userId = 'testUser'; // Sử dụng userId mặc định khi test
    let cart = this.apiService.getCartItems(userId);
  
    // Tìm kiếm sản phẩm trong giỏ hàng
    const existingProduct = cart.find((item: any) => item.productId === product.id);
  
    // Sử dụng `tempQuantity` làm số lượng sản phẩm thêm vào giỏ hàng
    const quantityToAdd = 1;
  
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
  }
}
