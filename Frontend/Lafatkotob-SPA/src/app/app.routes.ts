import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';

export const routes: Route[] = [

  { path: '', pathMatch: 'full', redirectTo: 'home-page' },


 /* { path: 'user/:username', component: UserProfileComponent, children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'events', component: EventsComponent },
      { path: 'wishlist', component: WishlistComponent },
      { path: 'notifications', component: NotificationsComponent },
      { path: 'messages', component: MessagesComponent },
      { path: 'settings', component: SettingsComponent }
    ]
  },*/
  {
    path:'user/:username',
    loadComponent: () => import('./Profile/components/user-profile/user-profile.component').then(m => m.UserProfileComponent),
  },

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
    path: 'sidebar',
    loadComponent: () => import('./shared/components/sidebar/sidebar.component').then(m => m.SidebarComponent)
  },

  {
    path: 'home-page',
    loadComponent: () => import('./Home/components/home-page/home-page.component').then(m => m.HomePageComponent)
  },


  { path: '**', loadComponent: () => import('./Auth/components/login-register/login-register.component').then(m => m.LoginRegisterComponent)},

 
];
@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    
})
export class AppRoutingModule { }