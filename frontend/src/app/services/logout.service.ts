import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'
@Injectable({
  providedIn: 'root'
})
export class LogoutService {
  private apiUrl = 'https://sunglasses-api-degkate8a0azc3dr.northeurope-01.azurewebsites.net/api/User/logout';  

  constructor(private http: HttpClient) {}

  logoutUser(userId: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, { userId });
  }
}