import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeleteProductsService {
  private apiUrl = 'http://localhost:5206/api/Products'; 

  constructor(private http: HttpClient) {}
  softDeleteProduct(productId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/delete/${productId}`);
  }
  restoreProduct(productId: string): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/restore/${productId}`, {});
  }
}