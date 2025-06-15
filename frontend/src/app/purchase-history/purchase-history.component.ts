import { Component,OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { CommonModule, NgFor } from '@angular/common';
import { Router } from '@angular/router';
@Component({
  selector: 'app-purchase-history',
  standalone: true,
  imports: [CommonModule, NgFor],
  templateUrl: './purchase-history.component.html',
  styleUrl: './purchase-history.component.css'
})

  export class PurchaseHistoryComponent implements OnInit {
    userId: string = ''; // User ID from localStorage
    purchaseHistory: any[] = [];
  
    constructor(
      private purchaseHistoryService: ProductService,
      private router: Router
    ) { }
  
    ngOnInit(): void {
      if (typeof window !== 'undefined') {
        this.userId = localStorage.getItem('userId') || '';
      }
      if (this.userId) {
        this.fetchPurchaseHistory();
      }
    }
  
    fetchPurchaseHistory(): void {
      this.purchaseHistoryService.getPurchaseHistory(this.userId).subscribe(
        (data) => {
          console.log('Purchase history:', data);
          this.purchaseHistory = data;
        },
        (error) => {
          console.error('Error fetching purchase history:', error);
        }
      );
    }
  }


