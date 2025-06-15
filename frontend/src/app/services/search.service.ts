import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

interface Product {
  ProductId: string;
  Name: string;
  Image: string;
  Price: number;
  Description: string;
  BrandName: string;
}

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private apiUrl = 'https://sunglasses-api-degkate8a0azc3dr.northeurope-01.azurewebsites.net/api/Products/search';  

  constructor(private http: HttpClient) {}

  getProducts(brandName: string = '', category: number | null = null): Observable<Product[]> {
    let params = new HttpParams();
    
    if (brandName) {
      params = params.set('brandName', brandName);
    }
    
    if (category !== null) {
      params = params.set('category', category.toString());
    }

    return this.http.get<Product[]>(this.apiUrl, { params });
  }
}