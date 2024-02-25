import { CommonModule } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { Router, Event as RouterEvent, NavigationEnd, RouterLink } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  standalone: true,
  imports: [CommonModule,RouterLink]
})
export class NavbarComponent {
  isAffix: boolean = false;
  navExpanded: boolean = false;
  showMenu: boolean = true;

  constructor(private router: Router) {
    this.router.events.pipe(
      filter((event: RouterEvent): event is NavigationEnd => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.showMenu = event.urlAfterRedirects !== '/login';
    });
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    this.isAffix = window.pageYOffset > 50;
  }

  toggleNavbar() {
    this.navExpanded = !this.navExpanded;
  }
  testClick() {
    console.log('Link clicked!');
  }
}