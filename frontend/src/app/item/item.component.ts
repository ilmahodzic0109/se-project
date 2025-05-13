import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { CommonModule, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
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

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
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
            },
      error => {
        console.error('Error fetching product details', error);
      }
    );
  }
 }