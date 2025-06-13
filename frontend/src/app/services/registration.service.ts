import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {
  private apiUrl = 'http://localhost:5206/api/User';  

  constructor(private http: HttpClient) {}

  registerUser(user: any): Observable<any> {
    return this.http.post('http://localhost:5206/api/User/register', user);  
  }
}