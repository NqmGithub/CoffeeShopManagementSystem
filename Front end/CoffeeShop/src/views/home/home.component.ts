import { Component } from '@angular/core';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { SliderComponent } from './slider/slider.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { Product } from '../../Interfaces/product';
import { ApiService } from '../../Api/api.service';
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    NavbarComponent,
    SliderComponent,
    MatCardModule,
    MatButtonModule,
    RouterModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent  {

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
}
