import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../Api/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../service/auth.service';
import { NavbarComponent } from "../../layout/navbar/navbar.component";
import { PaymentInfor } from '../../Interfaces/paymentInfor';
import { User } from '../../Interfaces/user';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NavbarComponent
  ],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cart: any[] = [];  // Cart items
  totalPrice: number = 0;  // Total price of the cart items
  user:User|null = null;
  constructor(
    private apiService: ApiService,
    private router: Router,
    private authService: AuthService,
  ) { 
    this.authService.getCurrentUser().subscribe(
      res =>{
        if(res){
          this.user = res;
          this.loadCart(this.user.id);  // Load cart when component initializes
        }
      }
    )
  }

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe(
      res =>{
        if(res){
          this.user = res;
          this.loadCart(this.user.id);  // Load cart when component initializes

        }
      }
    )
  }
  getImage(name: string) {
    return `https://localhost:44344/wwwroot/Images/${name}`;
  }
  // Load cart items from ApiService
  loadCart(userId: string): void {

    this.cart = this.apiService.getCartItems(userId);

    // Kiểm tra sản phẩm để tránh trùng lặp và tính toán tổng tiền
    this.cart = this.cart.filter((item, index, self) =>
      index === self.findIndex((t) => (
        t.productId === item.productId
      ))
    );

    this.calculateTotal();
  }

  // Calculate the total price of items in the cart
  calculateTotal(): void {
    this.totalPrice = this.cart.reduce(
      (total, item) => total + item.price * item.quantity, 0
    );
  }

  // Remove item from cart
  removeFromCart(productId: string): void {
    this.cart = this.cart.filter(item => item.productId !== productId); // Remove item by productId
    this.updateCart();  // Update the cart in ApiService and calculate total again
  }

  // Update the quantity of an item in the cart
  updateQuantity(productId: string, quantity: number): void {
    const product = this.cart.find(item => item.productId === productId);
    if (product) {
      product.quantity = quantity;  // Update the quantity of the product
      this.updateCart();  // Update the cart in ApiService and calculate total again
    }
  }

  // Save the updated cart items back to the cookie
  updateCart(): void {
    // const userId = this.authService.getId(); // Lấy userId từ AuthService (bỏ qua khi test)
    const userId = this.user?.id; // Sử dụng userId mặc định khi test
    // If user is not logged in, redirect to the login page
    if (!userId) {
      this.router.navigate(['/login']);
      return;
    }

    this.apiService.saveCartItems(userId, this.cart);  // Use ApiService to save updated cart
    this.calculateTotal();  // Recalculate total price
  } 

  // Proceed to checkout (navigate to a checkout page or order summary)
  checkout(): void {
    // this.addOrder(); // Gọi hàm thêm đơn hàng khi nhấn Checkout
    this.router.navigate(['checkout'])
  }
}
