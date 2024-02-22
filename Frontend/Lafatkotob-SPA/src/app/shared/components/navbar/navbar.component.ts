import { CommonModule } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  imports: [CommonModule,RouterLink],
  standalone: true
})
export class NavbarComponent {
  currentIndex = 0;

  @HostListener('window:scroll', ['$event'])
  onWindowScroll() {
    const scrollY = window.scrollY;
    const innerHeight = window.innerHeight;
    this.currentIndex = Math.floor(scrollY / (innerHeight - 75));
   
  }
}
