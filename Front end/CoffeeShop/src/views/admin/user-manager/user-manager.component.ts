import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { ApiService } from '../../../Api/api.service';
import { User } from '../../../Interfaces/user';
import { MatDialog } from '@angular/material/dialog';
import { UserDialogComponent } from './user-dialog/user-dialog.component';
import { ChangeStatusDialogComponent } from './change-status-dialog/change-status-dialog.component';

@Component({
  selector: 'app-user-manager',
  standalone: true,
  imports: [MatIconModule,
    MatTableModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
    MatSelectModule,
    MatSortModule,
    MatPaginator,],
  templateUrl: './user-manager.component.html',
  styleUrl: './user-manager.component.scss'
})
export class UserManagerComponent implements OnInit{

  constructor(private api: ApiService, private dialog: MatDialog){}

  status: string = 'all';
  search: string ="";
  userData: User[] = [];
  userCount!: number;
  pageNumber: number = 0;
  
  ngOnInit(): void {
    this.api.getUserCount().subscribe(data => this.userCount = data);
    this.loadUser();
  }

  onSearchChange(){
    this.pageNumber = 0;
    this.loadUser();
  }

  sortData(sort: Sort){
    const data = this.userData.slice();
    if (!sort.active || sort.direction === '') {
      this.userData = data;
      return;
    }

    this.userData = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'userName':
          return compare(a.userName, b.userName, isAsc);
        case 'email':
          return compare(a.email, b.email, isAsc);
        case 'phoneNumber':
          return compare(a.phoneNumber, b.phoneNumber, isAsc);
        case 'address':
          return compare(a.address, b.address, isAsc);
        case 'role':
          return compare(a.role, b.role, isAsc);
        default:
          return 0;
      }
    });
  }

  addUser(){
    const dialogRef = this.dialog.open(UserDialogComponent);
  }

  getImage(name: string){

  }

  viewDetail(user: User){

  }

  updateUser(user: User){
    const dialogRef = this.dialog.open(UserDialogComponent, {
      data: user
    });
  }

  changeStatus(user: User){
    const dialogRef = this.dialog.open(ChangeStatusDialogComponent, {
      data: user
    });

    dialogRef.afterClosed().subscribe((result: string) => {
      if (result) {
        this.api.makeUserInactive(user.id).subscribe(
          {
            error: (e) => console.error("error", e)
          }
        )
      }
    });
  }

  onPageChange(e: PageEvent){
    this.pageNumber = e.pageIndex;
    this.loadUser();
  }

  loadUser(){
    this.api.searchUser(this.search, this.status, this.pageNumber+1, 5)
    .subscribe((data) => {
      this.userData = data;
    });
  }
}

function compare(a: number | string, b: number | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}