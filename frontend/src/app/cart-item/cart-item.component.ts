import { Component,Input, SimpleChanges, OnChanges, Output,EventEmitter } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../services/product.service';
import { CartService } from '../services/cart.service';
@Component({
  selector: 'app-cart-item',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './cart-item.component.html',
  styleUrl: './cart-item.component.css'
})
export class CartItemComponent implements OnChanges {
  @Input() item: any;
  @Output() totalUpdated = new EventEmitter<number>(); 
  constructor(private cartService: CartService) {}
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['item'] && this.item) {
      this.updateSubtotal();
    }
  }
  updateSubtotal(): void {
    this.item.subtotal = this.item.price * this.item.quantity;
    this.totalUpdated.emit(this.item.subtotal);
  }
  restrictQuantity(): void {
    if (this.item.quantity < 1 || isNaN(this.item.quantity)) {
      this.item.quantity = 1;  
    }
  }

  onQuantityChange(): void {
    this.restrictQuantity();  
    this.updateSubtotal();
    this.updateCart(); 
  }
  updateCart(): void {
    this.cartService.updateCartItem(this.item).subscribe(
      (response) => {
        console.log('Cart updated successfully', response);
      },
      (error) => {
        console.error('Error updating cart:', error);
      }
    );
  }
  onCheckboxChange(): void {
    this.cartService.updateCartItemSelection(this.item.userId, this.item.productId, this.item.isSelected)
      .subscribe(
        (response) => {
          console.log('Cart item selection updated:', response);
        },
        (error) => {
          console.error('Error updating selection:', error);
        }
      );
      this.totalUpdated.emit(this.item.subtotal);
  }
  
}

