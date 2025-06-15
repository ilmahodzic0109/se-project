import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiUrl = 'https://sunglasses-api-degkate8a0azc3dr.northeurope-01.azurewebsites.net/api/Products';  

  constructor(private http: HttpClient) {}

  getProductDetails(productId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${productId}`);
  }
  getPurchaseHistory(userId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/purchase-history/${userId}`);
  }
}