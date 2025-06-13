import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LogoutService {
  private apiUrl = 'http://localhost:5206/api/User/logout';  

  constructor(private http: HttpClient) {}

  logoutUser(userId: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, { userId });
  }
}