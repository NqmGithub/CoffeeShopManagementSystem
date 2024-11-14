import { Component, OnInit, ViewChild } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { ApiService } from '../../../Api/api.service';
import { Product } from '../../../Interfaces/product';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {FormsModule} from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ProductDialogComponent } from './product-dialog/product-dialog.component';
import { CreateProduct } from '../../../Interfaces/createProduct';
import { ChangeStatusDialogComponent } from './change-status-dialog/change-status-dialog.component';
@Component({
  selector: 'app-product-manager',
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
  templateUrl: './product-manager.component.html',
  styleUrl: './product-manager.component.scss'
})
export class ProductManagerComponent implements OnInit {
  products: Product[] = [];
  productsData = new MatTableDataSource<Product>();  
  submitted = false;
  totalProducts = 10;
  search:string="";
  filterCategory:string="";
  filterStatus: string="";
  page: number=0;
  pageSize: number=6;
  sortColumn: string="ProductName";
  sortDirection:string="asc";

  listCategoryName: string[] = [];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private dialog: MatDialog, private apiService: ApiService) { }

  ngOnInit(): void {   

    //apply matsort and matpaginator
    this.productsData.sort = this.sort;
    this.productsData.paginator = this.paginator;
    this.loadProducts();
    this.loadCategoryNames();
  }

  onChange(){
    console.log(this.filterCategory)
    this.loadProducts();
  }

  onSortChange(){
    this.sortColumn= this.sort.active;
    this.sortDirection = this.sort.direction;
    this.loadProducts();
  }
  
  onPageChange(event:PageEvent):void{
    console.log(event.pageSize)
    this.page = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadProducts();
  }

  // search:string,filterCategory:string, filterStatus: string,page: number, pageSize: number,
  //   sortColumn: string,sortDirection:string
  loadProducts(){
    // load data
    this.apiService.getProducts(this.search,this.filterCategory,this.filterStatus,this.page,this.pageSize,this.sortColumn,this.sortDirection).subscribe(
      (response: { list: Product[], total: number }) => {
        this.products = response.list;
        this.productsData.data = this.products;
        this.totalProducts = response.total;
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }

  getImage(name: string){
    return `https://localhost:44344/Resources/Images/${name}`;
  }

  loadCategoryNames(){
    this.apiService.getAllCateogryNames().subscribe(
      (data : string[]) => {
        this.listCategoryName = data;
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }

  addProduct() {
    const dialogRef = this.dialog.open(ProductDialogComponent);

    dialogRef.afterClosed().subscribe((result: CreateProduct) => {
      if (result) {
        this.apiService.postProduct(result).subscribe(
          async response => {
            this.submitted = response,
            this.loadProducts();
        },
        error => {
            console.error('Error fetching data', error);
        }
        )
      }
    });
  }

  viewDetail(product: Product) {
  }

  updateProduct(product: Product) {
    const dialogRef = this.dialog.open(ProductDialogComponent, {
      data: product
    });

    dialogRef.afterClosed().subscribe((result: Product) => {
      if (result) {
        this.apiService.putProduct(product.id,result).subscribe(
          async response => {
            this.submitted = response,
            this.loadProducts();
        },
        error => {
            console.error('Error fetching data', error);
        }
        )
      }
    });
  }

  changeStatus(product: Product) {
    const dialogRef = this.dialog.open(ChangeStatusDialogComponent, {
      data: product
    });

    dialogRef.afterClosed().subscribe((result: string) => {
      if (result) {
        this.apiService.changeStatus(product.id,result).subscribe(
          async response => {
            this.submitted = response,
            this.loadProducts();
        },
        error => {
            console.error('Error fetching data', error);
        }
        )
      }
    });
  }
}
