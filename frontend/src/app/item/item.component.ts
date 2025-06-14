import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../services/product.service';
import { CommonModule, NgIf } from '@angular/common';
import { DatePipe } from '@angular/common';
import { DeleteProductsService } from '../services/delete-products.service';  
import { FormsModule } from '@angular/forms';
import { CartService } from '../services/cart.service';
import { RouterModule, Router } from '@angular/router';
@Component({
  selector: 'app-item',
  standalone: true,
  imports: [CommonModule, NgIf,FormsModule,RouterModule],
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})

export class ItemComponent implements OnInit {
  productId: string = '';
  productDetails: any;
  quantity: number = 1;  
  totalPrice: number = 0;  
  showDeleteModal: boolean = false;  
  showError: boolean = false;
  showLoginMessage = false;
  showSuccessMessage: boolean = false;
  
  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private productDeleteService: DeleteProductsService,  
    private cartService: CartService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('productId') || '';
    this.getProductDetails();
  }

  getProductDetails() {
    this.productService.getProductDetails(this.productId).subscribe(
      data => {
        this.productDetails = data;
        this.calculateTotalPrice(); 
      },
      error => {
        console.error('Error fetching product details', error);
      }
    );
  }

  
  calculateTotalPrice(): void {
    if (this.productDetails) {
      this.totalPrice = this.productDetails.price * this.quantity;
    }
  }

 
  onQuantityChange(value: number): void {
    if (value < 1) {
      this.showError = true;  
      this.quantity = 1;  
    } else {
      this.showError = false;  
    }
    this.calculateTotalPrice();  
  }

  isAdmin(): boolean {
    return localStorage.getItem('isAdmin') === 'true'; 
  }

  openDeleteModal() {
    this.showDeleteModal = true;
  }

  closeDeleteModal() {
    this.showDeleteModal = false;
  }

  onDeleteConfirmed() {
    this.productDeleteService.softDeleteProduct(this.productId).subscribe(
      (response) => {
        console.log('Product soft-deleted successfully:', response);
        this.getProductDetails();
        this.closeDeleteModal();  
      },
      (error) => {
        console.error('Error deleting product:', error);
      }
    );
  }
  isLoggedIn(): boolean {
    return localStorage.getItem('isLogged') === 'true';
  }
  alertLoginRequired(): void {
    this.showLoginMessage = true;
    setTimeout(() => {
      this.showLoginMessage = false;
      this.router.navigate(['/login']);
    }, 3000);  

  }
 
  onRestore() {
    this.productDeleteService.restoreProduct(this.productId).subscribe(
      (response) => {
        console.log('Product restored successfully:', response);
        this.getProductDetails();  
      },
      (error) => {
        console.error('Error restoring product:', error);
      }
    );
  }

  addToCart(): void {
    if (!this.isLoggedIn()) {
      this.alertLoginRequired();
      return;
    }

    const userId = localStorage.getItem('userId'); 
    if (userId) {
      this.cartService.addToCart(userId, this.productId, this.quantity).subscribe(
        response => {
          this.showSuccessMessage = true; 
                setTimeout(() => {
                    this.showSuccessMessage = false; 
                }, 3000);
        },
        error => {
          console.error('Error adding product to cart:', error);
          alert('Failed to add product to cart.');
        }
      );
    } else {
      alert('User ID not found.');
    }
  }
  buyNow(): void {
    if (!this.isLoggedIn()) {
      this.alertLoginRequired();
      return;
    }
  
    const userId = localStorage.getItem('userId');
    if (userId) {
      const productId = this.productDetails.productId;
      const quantity = this.quantity;
      const totalPrice = this.productDetails.price * quantity;
  
      
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
