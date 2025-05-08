import { provideHttpClient, withFetch } from '@angular/common/http';
import { Routes, provideRouter } from '@angular/router';

export const appRoutes: Routes = [
  { path: '**', redirectTo: 'homepage' },
];

export const appProviders = [
  provideRouter(appRoutes), 
  provideHttpClient(withFetch()), 
];
