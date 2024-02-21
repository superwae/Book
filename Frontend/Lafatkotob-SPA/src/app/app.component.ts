import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginRegisterComponent } from './Auth/login-register/login-register.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,LoginRegisterComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Lafatkotob-SPA';
}
