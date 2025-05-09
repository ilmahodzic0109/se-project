import { provideHttpClient, withFetch } from '@angular/common/http';
import { Routes, provideRouter } from '@angular/router';


import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { HeroComponent } from './hero/hero.component';
import { ItemComponent } from './item/item.component';

export const appRoutes: Routes = [
  { path: '', redirectTo: 'homepage', pathMatch: 'full' },
  { path: 'header', component: HeaderComponent },
  { path: 'footer', component: FooterComponent },
  { path: 'homepage', component: HeroComponent },
  { path: 'item', component: ItemComponent },
  { path: '**', redirectTo: 'homepage' },
];

export const appProviders = [
  provideRouter(appRoutes), // Provide the routes
  provideHttpClient(withFetch()), // Configure HttpClient with fetch support
];