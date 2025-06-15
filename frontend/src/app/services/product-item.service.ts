import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'
@Injectable({
  providedIn: 'root'
})
export class ProductItemService {

  private apiUrl = 'https://sunglasses-api-degkate8a0azc3dr.northeurope-01.azurewebsites.net/api/Products/shop';  

  constructor(@Inject(PLATFORM_ID) private platformId: Object, private http: HttpClient) {}

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
    if (isPlatformBrowser(this.platformId)) {
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