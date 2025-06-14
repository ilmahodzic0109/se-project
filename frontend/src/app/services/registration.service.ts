import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'
@Injectable({
  providedIn: 'root'
})
export class RegistrationService {
  private apiUrl = '${environment.apiUrl}/User';  

  constructor(private http: HttpClient) {}

  registerUser(user: any): Observable<any> {
    return this.http.post('http://localhost:5206/api/User/register', user);  
  }
}