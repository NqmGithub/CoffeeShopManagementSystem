import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [
    NavbarComponent,
    SidebarComponent
  ],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})
export class AdminComponent{

}
