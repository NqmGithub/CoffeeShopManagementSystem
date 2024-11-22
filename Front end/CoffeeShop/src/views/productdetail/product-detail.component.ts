import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../Api/api.service';
import { Product } from '../../Interfaces/product';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { AuthService } from '../../service/auth.service';
import { User } from '../../Interfaces/user';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NavbarComponent
  ],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  productId: string = '';  // ID of the current product
  product: Product | null = null;  // Product details
  relatedProducts: Product[] = [];  // Related products list
  tempQuantity: number = 1;  // Temporary quantity of the product
  user:User|null = null;
  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService,  // API service
    private router: Router,
    private authService: AuthService
  ) {
    this.authService.getCurrentUser().subscribe(
      res =>{
        this.user = res;
      }
    )
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.productId = params.get('productId') || '';

      if (!this.productId) {
        console.error('Product ID is missing!');
        return;
      }

      this.loadProductDetail(this.productId);
      this.loadRelatedProducts(this.productId);
    });
  }

  loadProductDetail(productId: string): void {
    this.apiService.getProductById(productId).subscribe({
      next: (product) => {
        this.product = product;
        this.tempQuantity = 1; // Initialize temporary quantity to 1
      },
      error: (err) => {
        console.error('Error fetching product details:', err);
      }
    });
  }

  loadRelatedProducts(productId: string): void {
    this.apiService.getRelatedProducts(productId).subscribe({
      next: (relatedProducts) => {
        this.relatedProducts = relatedProducts;
      },
      error: (err) => {
        console.error('Error fetching related products:', err);
      }
    });
  }

  getImage(name: string): string {
    return `https://localhost:44344/wwwroot/Images/${name}`;
  }

  viewProductDetail(productId: string): void {
    this.router.navigate(['/productdetail', productId]);
  }

  // Increase quantity
  increaseQuantity(): void {
    this.tempQuantity++;
  }

  // Decrease quantity
  decreaseQuantity(): void {
    if (this.tempQuantity > 1) {
      this.tempQuantity--;
    }
  }

  // Ensure that quantity cannot be less than 1
  onQuantityChange(): void {
    if (this.tempQuantity < 1) {
      this.tempQuantity = 1;
    }
  }

  addToCart(): void {
    if (!this.product) {
      console.error('No product to add to the cart!');
      return;
    }

    const userId = this.user?.id; // Default userId for testing
    let cart = this.apiService.getCartItems(userId!);

    // Check if product already exists in the cart
    const existingProduct = cart.find((item: any) => item.productId === this.product?.id);

    // Use tempQuantity to determine the quantity of product being added to cart
    const quantityToAdd = this.tempQuantity;

    if (existingProduct) {
      // If product exists in cart, increase quantity
      existingProduct.quantity += quantityToAdd;
    } else {
      // If product does not exist, add new product to the cart
      cart.push({
        productId: this.product.id,
        productName: this.product.productName,
        quantity: quantityToAdd,
        price: this.product.price,
        thumbnail: this.product.thumbnail,
      });
    }

    // Save updated cart
    this.apiService.saveCartItems(userId!, cart);
    alert('Product added to cart');
    // Reset tempQuantity after adding to cart
    this.tempQuantity = 1;
  }
}

