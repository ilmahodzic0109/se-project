import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'
@Injectable({
  providedIn: 'root'
})
export class ModifyProductService {
  private apiUrl = 'https://sunglasses-api-degkate8a0azc3dr.northeurope-01.azurewebsites.net/api/Products';  

  constructor(private http: HttpClient) {}

  
  getProductById(productId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${productId}`);
  }

  
  modifyProduct(productId: string, updatedProduct: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/modify/${productId}`, updatedProduct);
  }
}