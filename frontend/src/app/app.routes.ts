import { provideHttpClient, withFetch } from '@angular/common/http';
import { Routes, provideRouter } from '@angular/router';


import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { HeroComponent } from './hero/hero.component';
import { CategoriesComponent } from './categories/categories.component';
import { LoginComponent } from './login/login.component';
import { RegistrationComponent } from './registration/registration.component';
import { ItemComponent } from './item/item.component';
import { CartComponent } from './cart/cart.component';
import { PurchaseHistoryComponent } from './purchase-history/purchase-history.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { AddProductComponent } from './add-product/add-product.component';
import { ModifyProductComponent } from './modify-product/modify-product.component';

export const appRoutes: Routes = [
  { path: '', redirectTo: 'homepage', pathMatch: 'full' },
  { path: 'header', component: HeaderComponent },
  { path: 'footer', component: FooterComponent },
  { path: 'homepage', component: HeroComponent },
  { path: 'categories', component: CategoriesComponent },
  { path: 'login', component: LoginComponent },
  { path: 'registration', component: RegistrationComponent },
  { path: 'item/:productId', component: ItemComponent },
  { path: 'cart', component: CartComponent },
  { path: 'purchase-history', component: PurchaseHistoryComponent },
  { path: 'checkout', component: CheckoutComponent },
  {path: 'add-product', component: AddProductComponent},
  { path: 'modify-product/:productId', component: ModifyProductComponent },
  { path: '**', redirectTo: 'homepage' },
];

export const appProviders = [
  provideRouter(appRoutes), // Provide the routes
  provideHttpClient(withFetch()), // Configure HttpClient with fetch support
];
