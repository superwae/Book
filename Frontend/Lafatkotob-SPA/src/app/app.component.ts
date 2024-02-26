import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { LoginRegisterComponent } from './Auth/components/login-register/login-register.component';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { AddBookComponent } from './shared/components/add-book/add-book.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,LoginRegisterComponent,NavbarComponent,AddBookComponent,RouterLink],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Lafatkotob-SPA';
}
