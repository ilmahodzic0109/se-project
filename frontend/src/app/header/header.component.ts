import { Component,OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {CommonModule, NgIf} from '@angular/common';
import { LogoutService } from '../services/logout.service';
import { CartService } from '../services/cart.service';
@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, NgIf],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit{
  cartItemCount: number = 0;
  constructor(private router: Router, private userService: LogoutService, private cartService: CartService) {}

  ngOnInit(): void {
    const userId = localStorage.getItem('userId');
    if (userId) {
      
      this.cartService.cartItemCount$.subscribe(
        (count) => {
          this.cartItemCount = count;
        },
        (error) => {
          console.error('Error fetching cart item count:', error);
        }
      );

      
      this.cartService.getCartItemCount(userId).subscribe();
    }
  }
  loadCartItemCount(): void {
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.cartService.getCartItemCount(userId).subscribe(
        (response) => {
          this.cartItemCount = response.count;  
        },
        (error) => {
          console.error('Error fetching cart item count:', error);
        }
      );
    }
  }
  logout(): void {
    const userId = localStorage.getItem('userId');  

    if (userId) {
      this.userService.logoutUser(userId).subscribe({
        next: () => {
          localStorage.removeItem('userId');
          localStorage.removeItem('isAdmin');
          localStorage.removeItem('isLogged');
          this.router.navigate(['/login']);
        },
        error: (err) => {
          console.error('Logout failed:', err);
        }
      });
    } else {
      console.error('User not found in localStorage');
    }
  }
  isAdmin(): boolean {
    return localStorage.getItem('isAdmin') === 'true'; 
  }
  isLoggedIn(): boolean {
    return localStorage.getItem('isLogged') === 'true';
  }
}