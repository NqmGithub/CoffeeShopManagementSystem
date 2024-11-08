import { Component } from '@angular/core';
import { NavigationStart, Router, RouterOutlet } from '@angular/router';
import { HomeComponent } from '../views/home/home.component';
import { AdminComponent } from '../views/admin/admin.component';
import { filter } from 'rxjs';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    HomeComponent,
    AdminComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  constructor( private router: Router) {
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationStart) 
      )
      .subscribe((event: NavigationStart) => {
        const currentUrl = event.url; 

        if (currentUrl === '/' || currentUrl === '') {
          this.router.navigate(['home'])
        }
      });
  }
}