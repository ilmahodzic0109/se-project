import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ModifyProductService } from '../services/modify-product.service';
import { CommonModule, NgIf } from '@angular/common';

@Component({
  selector: 'app-modify-product',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './modify-product.component.html',
  styleUrls: ['./modify-product.component.css']
})
export class ModifyProductComponent implements OnInit {
  productId: string = '';
  productDetails: any = {}; 
  quantityInStock: number = 0;
  itemsSold: number = 0;
  successMessage: string = '';
  errorMessage: string = ''; 

  constructor(
    private route: ActivatedRoute,
    private productService: ModifyProductService,
    private router: Router
  ) {}

  ngOnInit(): void {
    
    this.productId = this.route.snapshot.paramMap.get('productId') as string;

   
    this.productService.getProductById(this.productId).subscribe(
      (product) => {
        this.productDetails = product;
        this.quantityInStock = product.quantityInStock;
        this.itemsSold = product.itemsSold;
        if (product.color === 'Black') this.productDetails.colorId = 1;
        if (product.color === 'Brown') this.productDetails.colorId = 2;
        if (product.color === 'Gold') this.productDetails.colorId = 3;
        if (product.color === 'Silver') this.productDetails.colorId = 4;
        if (product.color === 'Blue') this.productDetails.colorId = 5;

        if (product.condition === 'New') this.productDetails.conditionId = 1;
        if (product.condition === 'Used') this.productDetails.conditionId = 0;

        if (product.gender === 'Female') this.productDetails.gender = 1;
        if (product.gender === 'Male') this.productDetails.gender = 0;

        if (product.category === 'Female Category') this.productDetails.productCategory = 1;
        if (product.category === 'Male Category') this.productDetails.productCategory = 0;

        if (product.brand === 'Ray-Ban') this.productDetails.brandId = 1;
        if (product.brand === 'Oakley') this.productDetails.brandId = 2;
        if (product.brand === 'Gucci') this.productDetails.brandId = 3;
        if (product.brand === 'Prada') this.productDetails.brandId = 4;
        if (product.brand === 'Persol') this.productDetails.brandId = 5;
      },
      (error) => {
        console.error('Error fetching product details:', error);
      }
    );
  }

  saveProduct(): void {
    if (this.productDetails) {
      const updatedProduct = { 
        ...this.productDetails,
        quantityInStock: this.quantityInStock,
        itemsSold: this.itemsSold,
      };

      this.productService.modifyProduct(this.productId, updatedProduct).subscribe(
        (response) => {
          console.log('Product modified successfully:', response);
          this.successMessage = 'Product modified successfully!';
          this.errorMessage = ''; 
          setTimeout(() => this.router.navigate(['/categories']), 2000);
        },
        (error) => {
          console.error('Error updating product:', error);
          if (error.status === 400 && error.error && error.error.message) {
            this.errorMessage = error.error.message; 
          } else {
            this.errorMessage = 'An unexpected error occurred. Please try again later.';
          }
        }
      );
    }
  }

  cancelModification() {
    console.log('Modification cancelled');
    this.router.navigate(['/categories']);
  }
}