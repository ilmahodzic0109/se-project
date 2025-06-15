import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'
@Injectable({
  providedIn: 'root'
})
export class ProductItemService {

  private apiUrl = '${environment.apiUrl}/Products/shop';  

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
    let isAdmin = false;
    if (typeof window !== 'undefined') {
      isAdmin = localStorage.getItem('isAdmin') === 'true';
    }
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