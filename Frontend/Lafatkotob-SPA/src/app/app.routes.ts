import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { UserProfileComponent } from './Profile/components/user-profile/user-profile.component';
import { EventsComponent } from './Event/components/events/events.component';
import { BooksComponent } from './Book/components/books/books.component';

export const routes: Route[] = [

  { path: '', pathMatch: 'full', redirectTo: 'home-page' },


  {
    path: 'user/:username',
    component: UserProfileComponent,
    children: [
      { path: '', redirectTo: 'books', pathMatch: 'full' },
      { path: '', pathMatch: 'full', redirectTo: 'books' },
      { path: 'books', component: BooksComponent },
      { path: 'events', component: EventsComponent },
    ]
  },


  {
    path: 'event/:id',
    loadComponent: () => import('./Event/components/event/event.component').then(m => m.EventComponent),
  },
  {
    path: 'events',
    loadComponent: () => import('./Event/components/events/events.component').then(m => m.EventsComponent),
  },
  {
    path: 'event-details/:id',
    loadComponent: () => import('./Event/components/event-details/event-details.component').then(m => m.EventDetailsComponent),
  },


  {
    path: 'login',
    loadComponent: () => import('./Auth/components/login-register/login-register.component').then(m => m.LoginRegisterComponent)
  },

  {
    path: 'forgot-password',
    loadComponent: () => import('./Auth/components/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent)

  },


  {
    path: 'reset-password',
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
    path: 'home',
    loadComponent: () => import('./Home/components/home-page/home-page.component').then(m => m.HomePageComponent)
  },


  { path: '**', loadComponent: () => import('./Auth/components/login-register/login-register.component').then(m => m.LoginRegisterComponent) },


];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],

})
export class AppRoutingModule { }