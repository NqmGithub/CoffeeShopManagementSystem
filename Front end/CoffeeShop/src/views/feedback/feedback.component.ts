import { Component } from '@angular/core';
import { NavbarComponent } from "../../layout/navbar/navbar.component";
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-feedback',
  standalone: true,
  imports: [
    NavbarComponent,
    MatCardModule,
    MatButtonModule
  ],
  templateUrl: './feedback.component.html',
  styleUrl: './feedback.component.scss'
})
export class FeedbackComponent {

  constructor() {
    document.body.style.background = "url('./backgroundFeedback.webp') no-repeat center center fixed";
    document.body.style.backgroundSize = "cover";
  }
}
