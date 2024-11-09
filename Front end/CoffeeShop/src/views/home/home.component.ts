import { Component } from '@angular/core';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { SliderComponent } from './slider/slider.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
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

  coffeeBestsellers = [
    {
      title: 'Espresso',
      description: 'Strong and bold espresso made from 100% Arabica beans.',
      price: '$2.99',
      image: '/slider1.png'
    },
    {
      title: 'Cappuccino',
      description: 'A delicious blend of espresso, steamed milk, and foam.',
      price: '$3.99',
      image: '/slider1.png'
    },
    {
      title: 'Latte',
      description: 'Smooth and creamy latte with a hint of vanilla.',
      price: '$3.49',
      image: '/slider1.png'
    }
  ];
}
