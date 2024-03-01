import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';

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
    path: 'recommendation', 
    loadComponent: () => import('./shared/components/recommendation/recommendation.component').then(m => m.RecommendationComponent)
  },
  
  {
    path: 'book-details/:id', 
    loadComponent: () => import('./Book/components/book-details/book-details.component').then(m => m.BookDetailsComponent)
  },

  {
     path: 'book/:id', 
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