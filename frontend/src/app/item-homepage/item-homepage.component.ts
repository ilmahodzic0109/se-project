import { Component,OnInit, Input } from '@angular/core';
import { CommonModule, NgFor } from '@angular/common';
import { SearchService } from '../services/search.service';
import { CartService } from '../services/cart.service';  
import { Router } from '@angular/router';
@Component({
  
  selector: 'app-item-homepage',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './item-homepage.component.html',
  styleUrl: './item-homepage.component.css'
})
export class ItemHomepageComponent implements OnInit {
  @Input() products: any[] = [];  
  isLoading: boolean = true;
  successMessage: string = '';  
  addedProductId: string | null = null;
  constructor(
    private productService: SearchService,
    private cartService: CartService,
    private router: Router
  ) {}
  

  ngOnInit() {
    if (this.products.length === 0) {
      this.loadProducts();
    }
  }


  onSearchChanged(filters: { brandName: string, category: boolean | null }) {
    const categoryNumber = filters.category === null ? null : (filters.category ? 1 : 2);
    this.loadProducts(filters.brandName, categoryNumber);
  }
  isAdmin(): boolean {
    return localStorage.getItem('isAdmin') === 'true'; 
  }
  isLoggedIn(): boolean {
    return localStorage.getItem('isLogged') === 'true';
  }
  
  loadProducts(brandName: string = '', category: number | null = null) {
    this.isLoading = true;
    this.productService.getProducts(brandName, category).subscribe(
      (data: any[]) => {
        this.products = data.slice(0,6);
        this.isLoading = false;
      },
      (error) => {
        console.error('Error fetching products:', error);
        this.isLoading = false;
      }
    );
  }
  addToCart(product: any): void {
    const userId = localStorage.getItem('userId');
    if (userId) {
      const quantity = 1;  
      this.cartService.addToCart(userId, product.productId, quantity).subscribe(
        (response) => {
          console.log('Product added to cart:', response);
          this.addedProductId = product.productId;
          this.successMessage = `${product.brandName} added to cart!`;  
          this.updateCartCount(); 
        },
        (error) => {
          console.error('Error adding product to cart:', error);
          alert('Failed to add product to cart.');
        }
      );
    } else {
      alert('Please log in to add items to the cart.');
    }
  }

  
  updateCartCount(): void {
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.cartService.getCartItems(userId).subscribe(
        (data) => {
          let totalQuantity = 0;
          data.cartItems.forEach((item: any) => {
            totalQuantity += item.quantity;  
          });
          localStorage.setItem('cartItemCount', totalQuantity.toString());  
        },
        (error) => {
          console.error('Error fetching cart items for count:', error);
        }
      );
    }
  }
  buyNow(product: any): void {
    if (!this.isLoggedIn()) {
      alert('You need to be logged in to place an order.');
      return;
    }

    const userId = localStorage.getItem('userId');
    if (userId) {
      const productId = product.productId;
      const quantity = 1; 
      const totalPrice = product.price * quantity;

      console.log('Sending product directly to checkout:', userId, productId, quantity, totalPrice);

      
      const body = {
        userId,           
        productId,        
        quantity,         
        isSelected: true  
      };

    
      this.cartService.buyNow(body).subscribe(
        (selectionUpdateResponse) => {
          console.log('Product marked as selected:', selectionUpdateResponse);
          this.addedProductId = product.productId;
          this.successMessage = `${product.brandName} added to cart!`;

          this.router.navigate(['/checkout'], {
            queryParams: { productId, quantity, totalPrice }
          });
        },
        (error) => {
          console.error('Error updating product selection:', error);
          alert('Failed to select product for checkout.');
        }
      );
    }
  }
}

