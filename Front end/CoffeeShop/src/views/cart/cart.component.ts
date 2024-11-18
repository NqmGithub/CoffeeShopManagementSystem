import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../Api/api.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
  ],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cart: any[] = [];  // Cart items
  totalPrice: number = 0;  // Total price of the cart items

  constructor(
    private apiService: ApiService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadCart();
  }

  // Load cart items from cookies
  loadCart(): void {
    const cartData = this.apiService.getCookie('cart');
    if (cartData) {
      this.cart = JSON.parse(cartData);
      this.calculateTotal();
    }
  }

  // Calculate the total price
  calculateTotal(): void {
    this.totalPrice = this.cart.reduce(
      (total, item) => total + item.price * item.quantity,
      0
    );
  }

  // Remove item from cart
  removeFromCart(productId: string): void {
    this.cart = this.cart.filter(item => item.productId !== productId);
    this.updateCart();
  }

  // Update the quantity of an item
  updateQuantity(productId: string, quantity: number): void {
    const product = this.cart.find(item => item.productId === productId);
    if (product) {
      product.quantity = quantity;
      this.updateCart();
    }
  }

  // Save the cart back to cookies
  updateCart(): void {
    this.apiService.setCookie('cart', JSON.stringify(this.cart), 7);
    this.calculateTotal();
  }

  // Proceed to checkout (navigate to a checkout page or order summary)
  checkout(): void {
    // Redirect to the checkout page (you can add the checkout logic here)
    this.router.navigate(['/checkout']);
  }
}
