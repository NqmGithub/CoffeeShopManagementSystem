import { Component, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Contact } from '../../../Interfaces/contact';
import { ApiService } from '../../../Api/api.service';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-contact-manager',
  standalone: true,
  imports: [
    MatIconModule,
    MatTableModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
    MatSelectModule,
    MatSortModule,
    MatPaginator,
  ],
  templateUrl: './contact-manager.component.html',
  styleUrl: './contact-manager.component.scss'
})
export class ContactManagerComponent {
  contacts: Contact[] = [];
  contactsData = new MatTableDataSource<Contact>();  
  submitted = false;
  totalContacts = 10;
  search:string="";
  filterStatus: string="";
  page: number=0;
  pageSize: number=6;
  sortColumn: string="SendDate";
  sortDirection:string="des";

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor( private apiService: ApiService, private router:Router) { }

  ngOnInit(): void {   

    //apply matsort and matpaginator
    this.contactsData.sort = this.sort;
    this.contactsData.paginator = this.paginator;
    this.loadContacts();
  }

  onChange(){
    this.loadContacts();
  }

  onSortChange(){
    this.sortColumn= this.sort.active;
    this.sortDirection = this.sort.direction;
    this.loadContacts();
  }
  onPageChange(event:PageEvent):void{
    this.page = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadContacts();
  }


  loadContacts(){
    // load data
    this.apiService.getContacts(this.search,this.filterStatus,this.page,this.pageSize,this.sortColumn,this.sortDirection).subscribe(
      (response: { list: Contact[], total: number }) => {
        this.contacts = response.list;
        this.contactsData.data = this.contacts;
        this.totalContacts = response.total;
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }

  viewDetail(contact:Contact){
    this.router.navigate(['/admin',{ outlets: { mainContent: ['contact-detail', contact.id] } }]);
  }
}
