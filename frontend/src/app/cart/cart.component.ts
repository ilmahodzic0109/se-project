import { Component, OnInit } from '@angular/core';
import { CartService } from '../services/cart.service';
import { CommonModule } from '@angular/common';
import { CartItemComponent } from '../cart-item/cart-item.component';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CartItemComponent, CommonModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  cartItems: any[] = []; 
  totalAmount: number = 0; 

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.loadCartItems();
  }

  loadCartItems(): void {
    const userId = localStorage.getItem('userId'); 
    if (userId) {
      this.cartService.getCartItems(userId).subscribe(
        (data) => {
          console.log('Cart items:', data);
          this.cartItems = data.cartItems;
          this.calculateTotalAmount(); 
        },
        (error) => {
          console.error('Error fetching cart items:', error);
        }
      );
    }
  }
  
  calculateTotalAmount(): void {
    this.totalAmount = this.cartItems
      .filter(item => item.isSelected)  
      .reduce((acc, item) => acc + item.subtotal, 0); 
  }

  
  onCheckboxChange(): void {
    this.calculateTotalAmount();
  }
  hasSelectedItems(): boolean {
    return this.cartItems.some(item => item.isSelected);  
  }
}
