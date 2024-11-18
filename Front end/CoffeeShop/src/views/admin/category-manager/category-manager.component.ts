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
import { Category } from '../../../Interfaces/category';
import { MatDialog } from '@angular/material/dialog';
import { CategoryDialogComponent } from './category-dialog/category-dialog/category-dialog.component';
import { ChangeStatusDialogComponent } from './change-status-dialog/change-status-dialog/change-status-dialog.component';
import { CreateCategory } from '../../../Interfaces/createCategory';
@Component({
  selector: 'app-category-manager',
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
  templateUrl: './category-manager.component.html',
  styleUrl: './category-manager.component.scss'
})
export class CategoryManagerComponent {
  submitted = false;

  constructor(private api: ApiService, private dialog: MatDialog){}

  status: string = 'all';
  search: string ="";
  categoryData: Category[] = [];
  categoryCount!: number;
  pageNumber: number = 0;
  

  ngOnInit(): void {
    this.api.getCategoryCount().subscribe(data => this.categoryCount = data);
    this.loadCategory();
  }

  onSearchChange(){
    this.pageNumber = 0;
    this.loadCategory();
  }

  sortData(sort: Sort){
    const data = this.categoryData.slice();
    if (!sort.active || sort.direction === '') {
      this.categoryData = data;
      return;
    }

    this.categoryData = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'categoryName':
          return compare(a.categoryName, b.categoryName, isAsc);     
        default:
          return 0;
      }
    });
  }



  addCategory() {
    const dialogRef = this.dialog.open(CategoryDialogComponent);

    dialogRef.afterClosed().subscribe((result: CreateCategory) => {
      if (result) {
        this.api.postCategory(result).subscribe(
          async response => {
            this.submitted = response,
            this.loadCategory();
        },
        error => {
            console.error('Error fetching data', error);
        }
        )
      }
    });
  }

  updateCategory(category: Category) {
    const dialogRef = this.dialog.open(CategoryDialogComponent, {
      data: category
    });

    dialogRef.afterClosed().subscribe((result: Category) => {
      if (result) {
        this.api.putCategory(category.id,result).subscribe(
          async response => {
            this.submitted = response,
            this.loadCategory();
        },
        error => {
            console.error('Error fetching data', error);
        }
        )
      }
    });
  }

  changeStatusCategory(category: Category) {
    const dialogRef = this.dialog.open(ChangeStatusDialogComponent, {
      data: category
    });
  
    dialogRef.afterClosed().subscribe((result: number) => {
      if (result !== undefined) {  
        const newStatus = category.status == 1 ? 0 : 1;
  
        this.api.changeStatusCategory(category.id, newStatus).subscribe(
          async response => {
            this.submitted = response;
            this.loadCategory();  
          },
          error => {
            console.error('Error fetching data', error);
          }
        );
      }
    });
  }
  

  onPageChange(e: PageEvent){
    this.pageNumber = e.pageIndex;
    this.loadCategory();
  }

  loadCategory(): void {
    this.api.searchCategory(this.search, this.status, this.pageNumber+1, 5)
      .subscribe((data: any) => {
        // Update category data
        this.categoryData = data.categories;
        // Update category count (total number of categories)
        this.categoryCount = data.totalCount;
      }, (error) => {
        // Handle error
        console.error('Error fetching categories', error);
      });
  }
}

function compare(a: number | string, b: number | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
