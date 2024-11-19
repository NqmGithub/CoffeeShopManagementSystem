import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../../Api/api.service';
import { Product } from '../../Interfaces/product';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  productId: string = '';  // Product ID
  product: Product | null = null;  // Product details
  relatedProducts: Product[] = [];  // Related products list
  quantity: number = 1;  // Product quantity

  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService  // Inject ApiService
  ) {}

  ngOnInit(): void {
    // Get productId from URL
    this.productId = this.route.snapshot.paramMap.get('id') || '';

    if (!this.productId) {
      console.error('Product ID is missing!');
      return;
    }

    // Fetch product details
    this.apiService.getProductById(this.productId).subscribe({
      next: (product) => {
        this.product = product;
      },
      error: (err) => {
        console.error('Error fetching product details:', err);
        // Handle error
      }
    });

    // Fetch related products
    this.apiService.getRelatedProducts(this.productId).subscribe({
      next: (relatedProducts) => {
        this.relatedProducts = relatedProducts;
      },
      error: (err) => {
        console.error('Error fetching related products:', err);
        // Handle error
      }
    });
  }

  // Increase quantity
  increaseQuantity(): void {
    this.quantity++;
  }

  // Decrease quantity
  decreaseQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  // Add product to cart (store in cookie)
  addToCart(product: any): void {
    let cart: any[] = JSON.parse(this.apiService.getCookie('cart') || '[]');
    
    // Check if product already exists in cart
    const existingProduct = cart.find((item: any) => item.productId === product.productId);
    
    if (existingProduct) {
      // Update quantity if product already in cart
      existingProduct.quantity += product.quantity;
    } else {
      // Add new product to cart
      cart.push({
        productId: product.productId,
        productName: product.productName,
        quantity: product.quantity,
        price: product.price,
        thumbnail: product.thumbnail
      });
    }
    
    // Save the updated cart to cookies
    this.apiService.setCookie('cart', JSON.stringify(cart), 7);
  }
  
}

