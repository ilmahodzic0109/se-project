import { Component, EventEmitter, Output } from '@angular/core';
import { SearchService } from '../services/search.service';
import { FormsModule } from '@angular/forms';
import { CommonModule,NgIf } from '@angular/common';
@Component({
  selector: 'app-search',
  standalone: true,
  imports: [ FormsModule, CommonModule,NgIf],
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {
  keyword: string = '';
  category: number | null = null;
  noProductsFound: boolean = false;
  errorMessage: string = '';

  @Output() searchResults = new EventEmitter<any>();

  constructor(private searchService: SearchService) {}

  onSearch() {
    console.log('Search button clicked');
    this.searchService.getProducts(this.keyword, this.category).subscribe(
      (products) => {
        if (products.length === 0) {
          this.noProductsFound = true;
          this.errorMessage = 'No products found matching your search criteria.';
        } else {
          this.noProductsFound = false;
          this.errorMessage = '';
        }
        this.searchResults.emit(products);
      },
      (error) => {
        console.error('Error fetching products:', error);
        if (error.status === 404) {
          this.noProductsFound = true;
          this.errorMessage = 'No products found matching your search criteria.';
        } else {
          this.noProductsFound = true;
          this.errorMessage = 'An error occurred while fetching products.';
        }
      }
    );
  }
}