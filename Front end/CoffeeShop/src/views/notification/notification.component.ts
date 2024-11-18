import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { Contact } from '../../Interfaces/contact';
import { ApiService } from '../../Api/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../service/auth.service';
import { NavbarComponent } from "../../layout/navbar/navbar.component";

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [
    MatSidenavModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    CommonModule,
    ReactiveFormsModule,
    NavbarComponent
],
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.scss'
})
export class NotificationComponent {
  contacts: Contact[] = [];
	selectedContact:Contact = this.contacts[0];
	selectedContactId:string ='';
	adminName:string|null = null;
	adminId:string|null = null;
	public editorControl = new FormControl('');

	constructor(private apiService:ApiService, private auth:AuthService, private router: Router) {
		this.loadContacts();
	}
	

	loadContacts() {
		this.apiService.getAllContacts().subscribe(
		  (response: Contact[]) => {
			this.contacts = response;
      this.selectedContact = this.contacts[0];
      this.selectedContactId = this.selectedContact.id;
		  },
		  (error) => {
			console.error('Error fetching contacts:', error);
			// Thêm xử lý giao diện nếu cần
		  }
		);
	  }
	
	onClick(id: string){
		this.apiService.getContactById(id).subscribe(
			(response:Contact) =>{
				this.selectedContact=response
				this.selectedContactId = response.id
				
			},
			(error) => {
				console.error('Error fetching contact',error);
			}
		)
	}
}
