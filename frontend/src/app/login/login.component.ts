import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { LoginService } from '../services/login.service';
import { NgIf } from '@angular/common';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginData = {
    email: '',
    password: ''
  };
  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(private loginService: LoginService, private router: Router) {}

  onLogin(): void {
    this.loginService.login(this.loginData.email, this.loginData.password)
      .subscribe({
        next: (response) => {
          console.log('Login successful:', response);
          if (response.userId && response.isAdmin !== undefined) {
            localStorage.setItem('userId', response.userId);
            localStorage.setItem('isAdmin', response.isAdmin.toString());
            localStorage.setItem('isLogged', response.isLogged.toString());
          }
          this.successMessage = 'Login successful! Welcome back.';
          this.errorMessage = null; 
          setTimeout(() => {
            this.router.navigate(['/homepage']);  
          }, 3000);
        },
        error: (err) => {
          console.error('Error logging in:', err);
          this.successMessage = null; 
          this.errorMessage = 'Invalid credentials. Please try again.'; 
        }
      });
  }
}