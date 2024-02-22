import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Route, RouterModule } from '@angular/router';
import { ForgotPasswordComponent } from './Auth/forgot-password/forgot-password.component';
import { LoginRegisterComponent } from './Auth/login-register/login-register.component';

export const routes: Route[] = [

  { path: '', pathMatch: 'full', redirectTo: 'login' },

  { 
    path: 'login', 
    loadComponent: () => import('./Auth/login-register/login-register.component').then(m => m.LoginRegisterComponent)
  },

  {
    path:'forgot-password',
    loadComponent: () => import('./Auth/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent)
      
  },
  

  {
    path:'reset-password',
    loadComponent: () => import('./Auth/reset-password/reset-password.component').then(m => m.ResetPasswordComponent)
  },


 
];
@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    
})
export class AppRoutingModule { }