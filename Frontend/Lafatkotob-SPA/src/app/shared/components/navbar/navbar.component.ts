import { CommonModule } from '@angular/common';
import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { Router, Event as RouterEvent, NavigationEnd, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import {  ModaleService } from '../../Service/modal.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  standalone: true,
  imports: [CommonModule,RouterLink]
})
export class NavbarComponent implements OnInit,OnDestroy {
  isAffix: boolean = false;
  navExpanded: boolean = false;
  showMenu: boolean = true;
  showModal: boolean = false;
  private subscription: Subscription = new Subscription();

  constructor(private router: Router,private modalService: ModaleService) {
    this.router.events.pipe(
      filter((event: RouterEvent): event is NavigationEnd => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.showMenu = event.urlAfterRedirects !== '/login';
    });
  }

  ngOnInit() {
    this.subscription.add(this.modalService.showModal$.subscribe(visible => {
      this.showModal = visible;
    }));
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
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