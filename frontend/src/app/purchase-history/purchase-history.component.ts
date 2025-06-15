import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { ProductService } from '../services/product.service';
import { CommonModule, NgFor, isPlatformBrowser } from '@angular/common';
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
    @Inject(PLATFORM_ID) private platformId: Object,
    private purchaseHistoryService: ProductService,
    private router: Router
  ) { }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.userId = localStorage.getItem('userId') || '';
      if (this.userId) {
        this.fetchPurchaseHistory();
      }
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


