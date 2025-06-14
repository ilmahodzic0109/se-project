import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule, NgFor } from '@angular/common';
import { ProductItemService } from '../services/product-item.service';
import { CartService } from '../services/cart.service';  // Import CartService
import { RouterModule, Router } from '@angular/router';

interface Product {
  productId: string;
  category: string;
  brandName: string;
  image: string;
  price: number;
}

@Component({
  selector: 'app-item-categories',
  standalone: true,
  imports: [CommonModule, NgFor, RouterModule],
  templateUrl: './item-categories.component.html',
  styleUrls: ['./item-categories.component.css']
})
export class ItemCategoriesComponent implements OnInit, OnChanges {
  @Input() currentPage: number = 1; 
  @Input() category: boolean | undefined;  
  @Input() minPrice: number | undefined;  
  @Input() maxPrice: number | undefined; 
  @Input() condition: boolean | undefined;  
  @Input() products: Product[] = [];
  @Input() sortBy: number | null = null; 
  @Input() brandName: string = ''; 

  pageSize: number = 9;  
  successMessage: string = '';  
  isAddingToCart: boolean = false; 
  addedProductId: string | null = null; 

  constructor(
    private productService: ProductItemService,
    private cartService: CartService ,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadProducts();  
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['currentPage'] || changes['category'] || changes['minPrice'] || changes['maxPrice'] || changes['condition'] || changes['sortBy'] || changes['brandName']) {
      this.loadProducts();  
    }
  }

  loadProducts(): void {
    console.log('Fetching products for page', this.currentPage);

    this.productService.getProductsForShop(
      this.currentPage, 
      this.pageSize, 
      this.category, 
      this.minPrice, 
      this.maxPrice,
      this.condition,
      this.sortBy,
      this.brandName  
    ).subscribe({
      next: (data) => {
        console.log('Fetched products:', data);
        this.products = data.products;  
      },
      error: (err) => {
        console.error('Error fetching products:', err);
      }
    });
  }

  addToCart(product: Product): void {
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

  isAdmin(): boolean {
    return localStorage.getItem('isAdmin') === 'true'; 
  }

  isLoggedIn(): boolean {
    return localStorage.getItem('isLogged') === 'true';
  }

  buyNow(product: Product): void {
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
