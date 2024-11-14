import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-unauthorized',
  standalone: true,
  imports: [],
  templateUrl: './unauthorized.component.html',
  styleUrl: './unauthorized.component.scss'
})
export class UnauthorizedComponent {
  router: Router = inject(Router)
  tohome(){
    this.router.navigate(['home']);
  }
}
