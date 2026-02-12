import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./pages/home/home').then((m) => m.Home) },
  { path: 'products', loadComponent: () => import('./pages/products/products').then((m) => m.Products) },
  { path: 'categories', loadComponent: () => import('./pages/categories/categories').then((m) => m.Categories) },
  { path: '**', redirectTo: '' },
];
