import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RegistrationService } from '../services/registration.service';
import { CommonModule, NgIf } from '@angular/common';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [FormsModule, CommonModule, NgIf],
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {
  user = {
    email: '',
    password: '',
    confirmPassword: ''
  };
  showSuccessMessage: boolean = false;
  showPasswordError: boolean = false;
  showEmailError: boolean = false;

  constructor(private registrationService: RegistrationService, private router: Router) {}

  onRegister() {
    
    this.showPasswordError = false;
    this.showEmailError = false;

    if (this.user.password !== this.user.confirmPassword) {
      this.showPasswordError = true;  
      return;
    }

    this.registrationService.registerUser(this.user).subscribe({
      next: (response) => {
        console.log('Registration successful:', response);
        if (typeof window !== 'undefined') {
          localStorage.setItem('userId', response.userId); 
          localStorage.setItem('isAdmin', response.isAdmin.toString());
          localStorage.setItem('isLogged', 'true');  
        }
        this.showSuccessMessage = true; 
       
        setTimeout(() => {
          this.router.navigate(['/homepage']);  
        }, 3000);  
      },
      error: (err) => {
        console.error('Error registering user:', err);

        
        if (err.error && err.error.message) {
          if (err.error.message.includes("Email already in use")) {
            this.showEmailError = true;  
          } else if (err.error.message.includes("Passwords do not match")) {
            this.showPasswordError = true;  
          } else {
            alert(err.error.message); 
          }
        } else {
          alert('An unknown error occurred. Please try again later.');
        }
      }
    });
   
  }
}