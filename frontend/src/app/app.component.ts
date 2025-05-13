import { Component } from '@angular/core';
import { RouterModule } from '@angular/router'; 
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { CartComponent } from './cart/cart.component';
import { CartItemComponent } from './cart-item/cart-item.component';
import { ItemCategoriesComponent } from './item-categories/item-categories.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterModule, 
    HeaderComponent,
    FooterComponent,
    CartComponent,
    CartItemComponent,
    ItemCategoriesComponent
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'webpage';
}