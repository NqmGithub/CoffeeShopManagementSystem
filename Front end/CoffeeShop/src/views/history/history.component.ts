import { Component } from '@angular/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { Order } from '../../Interfaces/order';
import { ApiService } from '../../Api/api.service';
import { AuthService } from '../../service/auth.service';
import { User } from '../../Interfaces/user';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from "../../layout/navbar/navbar.component";
import { UserOrderDetails } from '../../Interfaces/userOrderDetails';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [MatExpansionModule, CommonModule, NavbarComponent,MatCardModule, MatButtonModule,MatIconModule],
  templateUrl: './history.component.html',
  styleUrl: './history.component.scss'
})
export class HistoryComponent {
  stars = [1, 2, 3, 4, 5];
  orders: Order[] = [];
  orderDetails: UserOrderDetails[]=[];
  user:User|null = null
  constructor(private authService: AuthService,private apiService: ApiService) {
    authService.getCurrentUser().subscribe(
      res =>{
        this.user = res;
        if(this.user){
          this.loadOrders(this.user.id);          
        }
      }
    );
  }

  loadOrders(id:string){
    this.apiService.getAllOrdersByUserId(id).subscribe(
      res =>{
        this.orders = res;
        for (let i = 0; i < this.orders.length; i++) {
          this.apiService.getOrderDetailsByOrderId(this.orders[i].id).subscribe(
            res =>{
              this.orders[i].product = res;
              console.log(res)
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
      productName: product.productName,
      rating: product.rating
    }));
  }
}
