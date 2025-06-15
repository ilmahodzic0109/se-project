import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { ItemCategoriesComponent } from '../item-categories/item-categories.component';
import { CommonModule, NgIf, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductItemService } from '../services/product-item.service';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [ItemCategoriesComponent, FormsModule, CommonModule, NgIf],
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent {
  currentPage: number = 1;
  totalPages: number = 1;

  
  filters = {
    category: undefined as boolean | undefined,
    minPrice: undefined as number | undefined,
    maxPrice: undefined as number | undefined,
    condition: undefined as boolean | undefined,
    brandName: '' as string,
    sortBy: 0 as number,
  };

  products: any[] = [];
  noResultsFound: boolean = false; 

  constructor(@Inject(PLATFORM_ID) private platformId: Object, private productService: ProductItemService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  
  loadProducts(): void {
    const { category, minPrice, maxPrice, condition, brandName, sortBy } = this.filters;
  
   
    this.productService.getProductsForShop(
      this.currentPage,
      9,
      category,
      minPrice,
      maxPrice,
      condition,
      sortBy,
      brandName 
    ).subscribe({
      next: (data) => {
     
        console.log('Fetched products:', data);
  
        if (data && Array.isArray(data.products)) {
         
          this.products = data.products;
          this.totalPages = data.totalPages; 
          this.noResultsFound = false;
        } else {
        
          this.noResultsFound = true;
          this.products = [];
        }
      },
      error: (err) => {
        console.error('Error fetching products with filters and search:', err);
        this.noResultsFound = true;
      }
    });
  }
  

  updateSortOrder(): void {
    this.loadProducts();
  }

 
  updateSearchTerm(): void {
    this.filters.brandName = this.filters.brandName.trim(); 
    this.loadProducts();  
  }


  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadProducts();
    }
  }

  isAdmin(): boolean {
    return isPlatformBrowser(this.platformId) && localStorage.getItem('isAdmin') === 'true';
  }

  
  changeCategoryFilter(category: boolean): void {
    this.filters.category = this.filters.category === category ? undefined : category;
    this.currentPage = 1;
    this.loadProducts();
  }

  
  changePriceRangeFilter(min: number, max: number): void {
    if (this.filters.minPrice === min && this.filters.maxPrice === max) {
      this.filters.minPrice = undefined;
      this.filters.maxPrice = undefined;
    } else {
      this.filters.minPrice = min;
      this.filters.maxPrice = max;
    }
    this.loadProducts();
  }


  changeConditionFilter(condition: boolean): void {
    this.filters.condition = this.filters.condition === condition ? undefined : condition;
    this.loadProducts();
  }
}