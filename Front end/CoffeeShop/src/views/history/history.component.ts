import { Component } from '@angular/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { Order, OrderDTO } from '../../Interfaces/order';
import { ApiService } from '../../Api/api.service';
import { AuthService } from '../../service/auth.service';
import { User } from '../../Interfaces/user';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from "../../layout/navbar/navbar.component";
import { UserOrderDetails } from '../../Interfaces/userOrderDetails';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [MatExpansionModule, CommonModule, NavbarComponent,MatCardModule, MatButtonModule,MatIconModule],
  templateUrl: './history.component.html',
  styleUrl: './history.component.scss'
})
export class HistoryComponent {
  stars = [1, 2, 3, 4, 5];
  orders: OrderDTO[] = [];
  orderDetails: UserOrderDetails[]=[];
  user:User|null = null
  constructor(private authService: AuthService,private apiService: ApiService,private activatedRoute: ActivatedRoute) {
    authService.getCurrentUser().subscribe(
      res =>{
        this.user = res;
        if(this.user){
          this.loadOrders(this.user.id);          
        }
      }
    );
    this.activatedRoute.queryParams.subscribe((params) => {
      // Call paymentCallback API with query parameters
      this.apiService.paymentCallback(params).subscribe({
        next: (response) => {
          console.log(response)
          if (response.vnPayResponseCode === '00') {
            alert('Payment successful: ' + response);
            // Call the API to add product to the database (e.g., this.apiService.addProductToDatabase(productDetails))
          } else {
            // If response does not have vnp_ResponseCode=00, show a failure alert
            alert('Payment failed: ' + response.vnp_ResponseCode);
          }
        },
        error: (err) => {
          console.error('Error handling payment callback:', err);
        },
      });
    });
  }

  loadOrders(id:string){
    this.apiService.getAllOrdersByUserId(id).subscribe(
      res =>{
        this.orders = res;
        for (let i = 0; i < this.orders.length; i++) {
          this.apiService.getOrderDetailsByOrderId(this.orders[i].id).subscribe(
            res =>{
              this.orders[i].product = res;
              for (const element of this.orders[i].product) {
                if (element.rating > 0) {
                    this.orders[i].isRated = true;
                    break; 
                }
            }
            }
          )
          
        }
      }
    );
  }

  getImage(name: string){
    return `https://localhost:44344/wwwroot/Images/${name}`;
  }
  
  setRating(orderIndex: number, productIndex: number, rating: number): void {
    this.orders[orderIndex].product[productIndex].rating = rating;
  }

  submitRatings(orderIndex: number): void {
    const ratedProducts = this.orders[orderIndex].product.map(product => ({
      id: product.id,
      rating: product.rating
    }));

    this.apiService.rating(ratedProducts).subscribe();
  }
}
