import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { NavbarComponent } from "../../layout/navbar/navbar.component";
import { AuthService } from '../../service/auth.service';
import { User } from '../../Interfaces/user';
import { ApiService } from '../../Api/api.service';
import { PaymentInfor } from '../../Interfaces/paymentInfor';
import { catchError, delay, Observable, switchMap, tap, throwError } from 'rxjs';
@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [MatCardModule, MatFormField, MatLabel, MatSelectModule, CommonModule, ReactiveFormsModule, MatButtonModule, MatInputModule, NavbarComponent],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent{
  cart: any[] = [];  // Cart items
  totalPrice: number = 0;  // Total price of the cart items
  user:User|null = null;
  constructor(private authService:AuthService, private apiService:ApiService) {
    this.authService.getCurrentUser().subscribe(
      res => {
        this.user = res;
        this.loadCart(this.user?.id!)
      }
    )
  }

  getImage(name: string) {
    return `https://localhost:44344/wwwroot/Images/${name}`;
  }
  // Load cart items from ApiService
  loadCart(userId:string): void {

    this.cart = this.apiService.getCartItems(userId);
    this.calculateTotal();
  }

  // Calculate the total price of items in the cart
  calculateTotal(): void {
    this.totalPrice = this.cart.reduce(
      (total, item) => total + item.price * item.quantity, 0
    );
  }

  createPayment(): void {
    const paymentModel: PaymentInfor = {
      orderType: 'other',
      amount: this.totalPrice,
      orderDescription: 'Thanh toan den CoffeeShop',
      name: this.user == null ? "" : this.user?.userName
    };
  
    // Ensure addOrder completes before creating the payment URL
    this.addOrderObservable().pipe(
      switchMap(() => this.apiService.createPaymentUrl(paymentModel)) // Wait for addOrder to complete before creating payment URL
    ).subscribe({
      next: (url) => {
        // Redirect to VNPay after successful URL creation
        window.location.href = url;
      },
      error: (err) => {
        console.error('Error creating payment URL:', err);
      },
    });
  }

  addOrderObservable(): Observable<any> {
    // Check if the cart is empty
    if (this.cart.length === 0) {
      alert('Your cart is empty.');
      return throwError(() => new Error('Cart is empty.'));
    }
  
    // Create OrderCreateDTO
    const orderCreateDTO = {
      userID: this.user?.id,
      details: this.cart.map(item => ({
        productId: item.productId,
        quantity: item.quantity
      }))
    };
  
    // Return the API call observable
    return this.apiService.addOrder(orderCreateDTO).pipe(
      tap((response) => {
        console.log('Order placed successfully!');
        sessionStorage.setItem("orderId", response.id);
        // Optionally, clear the cart here if needed
        // this.apiService.clearCart(this.user?.id!);
      }),
      catchError((error) => {
        console.error('Error placing order:', error);
        alert(`Failed to place order: ${error.message}`);
        return throwError(() => error); // Rethrow the error so the outer observable knows
      })
    );
  }
}
