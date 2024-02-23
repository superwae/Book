import { CommonModule } from '@angular/common';
import { Component, HostListener, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  imports: [CommonModule,RouterLink],
  standalone: true
})
export class NavbarComponent {
  private router = inject(Router);
  currentIndex = 0;
  isLoginPage(): boolean {
    return this.router.url === '/login';
  }
  @HostListener('window:scroll', ['$event'])
  onWindowScroll() {
    const scrollY = window.scrollY;
    const innerHeight = window.innerHeight;
    this.currentIndex = Math.floor(scrollY / (innerHeight - 75));
   
  }
}
