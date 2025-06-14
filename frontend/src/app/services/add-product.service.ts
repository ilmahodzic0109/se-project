import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'
@Injectable({
  providedIn: 'root'
})
export class AddProductService {

  private apiUrl = '${environment.apiUrl}/Products/product';  

  constructor(private http: HttpClient) {}

  addProduct(productData: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, productData);
  }
}