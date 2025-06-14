import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private apiUrl = 'http://localhost:5206/api/Cart';  
  private cartItemCountSubject = new BehaviorSubject<number>(0);  

  cartItemCount$ = this.cartItemCountSubject.asObservable();
  constructor(private http: HttpClient) {}


  addToCart(userId: string, productId: string, quantity: number): Observable<any> {
    const body = { userId, productId, quantity };
    return this.http.post(`${this.apiUrl}/add`, body).pipe(
      
      switchMap(() => this.getCartItemCount(userId)));
  }
  getCartItems(userId: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/view/${userId}`);
  }
  updateCartItem(item: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/update`, item);
  }
  getCartItemCount(userId: string): Observable<{ count: number }> {
    return this.http.get<{ count: number }>(`${this.apiUrl}/count/${userId}`).pipe(
      
      tap(response => this.cartItemCountSubject.next(response.count))
    );
  }
  updateCartItemSelection(userId: string, productId: string, isSelected: boolean): Observable<any> {
    const body = { userId, productId, isSelected };
    return this.http.put(`${this.apiUrl}/update-selection`, body);
  }
  getSelectedCartItems(userId: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/get-selected-cart-items/${userId}`);
  }
  removeSelection(userId: string, productId: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/remove-selection/${userId}/${productId}`, {});
  } 
  placeOrder(orderRequest: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/place-order`, orderRequest);
  }
  clearCart(userId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/clear-cart/${userId}`, {});
  }
  
buyNow(body: { userId: string, productId: string, quantity: number }): Observable<any> {
  return this.http.put(`${this.apiUrl}/buy-now`, body);
}

}

