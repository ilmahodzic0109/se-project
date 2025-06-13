import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AddProductService } from '../services/add-product.service';  
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [FormsModule,NgIf],
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {
  product = {
    name: '',
    model: '',
    description: '',
    deliveryTime: '',
    image: '',
    price: null,
    shippingPrice: null,
    colorId: '',
    conditionId: '',
    gender: '',
    productCategory: '',
    brandId: '',
    quantityInStock: null,
    itemsSold: null
  };
  successMessage: string = '';  
  isFormSubmitted: boolean = false; 
  constructor(
    private productService: AddProductService,
    public router: Router
  ) {}

  ngOnInit(): void {}

 
  onSubmit(addProductForm: any) {
    
    if (addProductForm.valid) {
      const formData = this.product;

      this.productService.addProduct(formData).subscribe(
        (response) => {
          console.log('Product added successfully:', response);
          this.successMessage = 'Product added successfully!';
          this.isFormSubmitted = true;
          this.product = {
            name: '',
            model: '',
            description: '',
            deliveryTime: '',
            image: '',
            price: null,
            shippingPrice: null,
            colorId: '',
            conditionId: '',
            gender: '',
            productCategory: '',
            brandId: '',
            quantityInStock: null,
            itemsSold: null
          };
          setTimeout(() => {
            this.isFormSubmitted = false;
          }, 3000);
        },
        (error) => {
          console.error('Error adding product:', error);
          this.successMessage = 'Error adding product. Please try again.';
        }
      );
    } else {
      console.log('Form is invalid');
    }
  }
}