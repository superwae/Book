import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Route, RouterModule } from '@angular/router';
import { ForgotPasswordComponent } from './Auth/components/forgot-password/forgot-password.component';
import { LoginRegisterComponent } from './Auth/components/login-register/login-register.component';

export const routes: Route[] = [

  { path: '', pathMatch: 'full', redirectTo: 'login' },

  { 
    path: 'login', 
    loadComponent: () => import('./Auth/components/login-register/login-register.component').then(m => m.LoginRegisterComponent)
  },

  {
    path:'forgot-password',
    loadComponent: () => import('./Auth/components/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent)
      
  },
  

  {
    path:'reset-password',
    loadComponent: () => import('./Auth/components/reset-password/reset-password.component').then(m => m.ResetPasswordComponent)
  },
  
  {
    path: 'book-details/:id', 
    loadComponent: () => import('./Book/components/book-details/book-details.component').then(m => m.BookDetailsComponent)
  },

  {
     path: 'books/:id', 
    loadComponent: () => import('./Book/components/book/book.component').then(m => m.BookComponent)
  },
  {
    path: 'books',
    loadComponent: () => import('./Book/components/books/books.component').then(m => m.BooksComponent)
  },

  { path: '**', loadComponent: () => import('./Auth/components/login-register/login-register.component').then(m => m.LoginRegisterComponent)},

 
];
@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    
})
export class AppRoutingModule { }