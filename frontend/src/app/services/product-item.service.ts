import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductItemService {

  private apiUrl = 'http://localhost:5206/api/Products/shop';  

  constructor(private http: HttpClient) {}

  getProductsForShop(
    pageNumber: number,
    pageSize: number,
    category: boolean | undefined,
    minPrice: number | undefined,
    maxPrice: number | undefined,
    condition: boolean | undefined,
    sortBy: number | null,
    brandName: string = ''
    
  ): Observable<any> {
    const isAdmin = localStorage.getItem('isAdmin') === 'true';
    const headers = new HttpHeaders({
      'isAdmin': isAdmin.toString()
    });

    let url = `${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`;

    if (category !== undefined) url += `&category=${category}`;
    if (minPrice !== undefined) url += `&minPrice=${minPrice}`;
    if (maxPrice !== undefined) url += `&maxPrice=${maxPrice}`;
    if (condition !== undefined) url += `&condition=${condition}`;
    if (sortBy !== null) url += `&sortBy=${sortBy}`;
    if (brandName) url += `&brandName=${brandName}`;

    return this.http.get<any>(url, { headers });
  }
}