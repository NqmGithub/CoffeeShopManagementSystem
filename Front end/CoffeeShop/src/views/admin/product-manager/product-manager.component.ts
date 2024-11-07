import { Component, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { ApiService } from '../../../Api/api.service';
import { Product } from '../../../Interfaces/product';

@Component({
  selector: 'app-product-manager',
  standalone: true,
  imports: [
    MatIconModule,
    MatTableModule,
    MatButtonModule
  ],
  templateUrl: './product-manager.component.html',
  styleUrl: './product-manager.component.scss'
})
export class ProductManagerComponent implements OnInit {
  products: Product[] = [];
  productsData = new MatTableDataSource<Product>();  
  submitted = false;

  constructor(private dialog: MatDialog, private apiService: ApiService) { }

  ngOnInit(): void {
    // load data
    console.log('data')
    this.apiService.getProducts().subscribe(
      (data: Product[]) => {
        console.log(data)
        this.products = data;
        this.productsData.data = this.products;
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }

  addProduct() {
  }

  viewDetail(product: Product) {
  }

  updateProduct(product: Product) {
  }

  deleteProduct(product: Product) {
  }
}
