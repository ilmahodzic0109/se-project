import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'

@Injectable({
  providedIn: 'root'
})
export class DeleteProductsService {
  private apiUrl = '${environment.apiUrl}/Products'; 

  constructor(private http: HttpClient) {}
  softDeleteProduct(productId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/delete/${productId}`);
  }
  restoreProduct(productId: string): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/restore/${productId}`, {});
  }
}