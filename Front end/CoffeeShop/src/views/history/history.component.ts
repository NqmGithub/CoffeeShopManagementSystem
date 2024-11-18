import { Component } from '@angular/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { Order } from '../../Interfaces/order';
import { ApiService } from '../../Api/api.service';
import { AuthService } from '../../service/auth.service';
import { User } from '../../Interfaces/user';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from "../../layout/navbar/navbar.component";

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [MatExpansionModule, CommonModule, NavbarComponent],
  templateUrl: './history.component.html',
  styleUrl: './history.component.scss'
})
export class HistoryComponent {

  orders: Order[] = [];
  user:User|null = null
  constructor(private authService: AuthService,private apiService: ApiService) {
    authService.getCurrentUser().subscribe(
      res =>{
        this.user = res;
        console.log(this.user);
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
      }
    );
  }
}
