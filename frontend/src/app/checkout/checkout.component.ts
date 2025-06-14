import { Component, OnInit, OnDestroy } from '@angular/core';
import { CartService } from '../services/cart.service';
import { CommonModule, NgFor } from '@angular/common';
import { Router, NavigationStart, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, NgFor, FormsModule],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  userId: string = ''; 
  cartItems: any[] = [];
  totalAmount: number = 0;
  orderDetails: any = {  
    address: '',
    city: '',
    country: '',
    postalCode: '',
    phoneNumber: ''
  };
  orderPlaced: boolean = false;
  noSelectedItemsError: boolean = false;
  quantityErrorMessage: string = '';
  constructor(private cartService: CartService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.userId = localStorage.getItem('userId') || '';  
    if (this.userId) {
      this.loadSelectedCartItems();
    } else {
      console.error("User is not logged in");
    }
    this.route.queryParams.subscribe((params:any) => {
      const productId = params['productId'];
      const quantity = params['quantity'];
      const totalPrice = params['totalPrice'];

      if (productId) {
        this.orderDetails = {
          productId,
          quantity,
          totalPrice,
        };
        this.totalAmount = totalPrice;
      }
    });
  }

  loadSelectedCartItems() {
    this.cartService.getSelectedCartItems(this.userId).subscribe(
      (response) => {
        this.cartItems = response.cartItems;
        this.totalAmount = this.cartItems.reduce((total, item) => total + item.subtotal, 0);
      },
      (error) => {
        console.error('Error loading cart items:', error);
      }
    );
  }

  placeOrder() {
    if (this.cartItems.length === 0) {
      console.error('No selected products to place the order');
      this.noSelectedItemsError = true; 
      return;
    }
    this.noSelectedItemsError = false;
   
    this.quantityErrorMessage = '';
    const orderRequest = {
      userId: this.userId,
      address: this.orderDetails.address,
      city: this.orderDetails.city,
      country: this.orderDetails.country,
      postalCode: this.orderDetails.postalCode,
      phoneNumber: this.orderDetails.phoneNumber
    };

    this.cartService.placeOrder(orderRequest).subscribe(
      (response) => {
        console.log('Order placed successfully', response);
        
        this.orderPlaced = true;
        
        this.cartService.clearCart(this.userId).subscribe(
          (clearResponse) => {
            console.log('Cart cleared successfully after order:', clearResponse);
            this.cartItems = [];
            this.totalAmount = 0;
            this.resetOrderForm();
            this.cartService.getCartItemCount(this.userId).subscribe(
              (countResponse) => {
                console.log('Updated cart item count:', countResponse.count);
              },
              (error) => {
                console.error('Error updating cart item count:', error);
              }
            );
            setTimeout(() => {
              this.orderPlaced = false;  
            }, 4000);
            
          },
          (clearError) => {
            console.error('Error clearing cart:', clearError);
          }
        );
      },
      (error) => {
        console.error('Error placing order:', error);
        if (error.error && error.error.message) {
          this.quantityErrorMessage = error.error.message;
          setTimeout(() => {
            this.quantityErrorMessage = '';
          }, 4000);
        } else {
          alert('Failed to place the order');
        }
    });
  }

  
  removeItemFromCheckout(productId: string) {
    
    console.log('Product ID to remove:', productId);  
  
    if (productId) {
      this.cartService.removeSelection(this.userId, productId).subscribe(
        (response) => {
          console.log(response.message);  
          this.cartItems = this.cartItems.filter(item => item.productId !== productId); 
          this.calculateTotalAmount(); 
        },
        (error) => {
          console.error('Error removing item from checkout:', error);
        }
      );
    } else {
      console.error('Product ID is undefined or null');
    }
  }
  

  calculateTotalAmount() {
    this.totalAmount = this.cartItems
      .reduce((total, item) => total + item.total, 0); 
  }
  resetOrderForm() {
    this.orderDetails = {
      address: '',
      city: '',
      country: '',
      postalCode: '',
      phoneNumber: ''
    };
  }
}