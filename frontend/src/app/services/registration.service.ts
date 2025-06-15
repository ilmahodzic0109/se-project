import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'
@Injectable({
  providedIn: 'root'
})
export class RegistrationService {
  private apiUrl = 'https://sunglasses-api-degkate8a0azc3dr.northeurope-01.azurewebsites.net/api/User';  

  constructor(private http: HttpClient) {}

  registerUser(user: any): Observable<any> {
    return this.http.post('${apiUrl}/register', user);  
  }
}