import { bootstrapApplication } from '@angular/platform-browser';
import { importProvidersFrom } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app/app.component';
import { AppRoutingModule } from './app/app.routes';

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(AppRoutingModule),
    importProvidersFrom(HttpClientModule) 
  ]
}).catch(err => console.error(err));
