import { CommonModule } from '@angular/common';
import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { Router, Event as RouterEvent, NavigationEnd, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import {  ModaleService } from '../../Service/ModalService/modal.service';
import { AppUsereService } from '../../../Auth/services/app-user.service';
import { AppUserModel } from '../../../Auth/Models/AppUserModel';
import { MyTokenPayload } from '../../Models/MyTokenPayload';
import { jwtDecode } from 'jwt-decode';

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
  isLoggedIn: boolean = false;
  profilePictureUrl: string | null = null;
  User: AppUserModel | null = null;
  showDropdown = false;
  private subscription: Subscription = new Subscription();
  private authSubscription: Subscription=new Subscription(); // Add this line

  constructor(
    private router: Router,
    private modalService: ModaleService,
    private appUserService: AppUsereService
    ) {
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

    // Correctly subscribe to authentication state changes
    this.authSubscription = this.appUserService.isAuthenticated.subscribe(isAuthenticated => {
      this.isLoggedIn = isAuthenticated;
      if (isAuthenticated) {
        // Fetch user info only if authenticated
        const userId = this.getUserInfoFromToken()?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        if (userId) {
          this.fetchUserInfo(userId);
        }
      } else {
        // Reset user info on logout
        this.User = null;
        this.profilePictureUrl = null;
      }
    });
  }

  fetchUserInfo(userId: string) {
    this.appUserService.getUserById(userId).subscribe({
      next: (userData: AppUserModel) => {
        this.User = userData;
        this.profilePictureUrl = userData.profilePicture;
      },
      error: (error) => {
        console.error('Error fetching user data:', error);
      },
    });
  }
  ngOnDestroy() {
    this.subscription.unsubscribe();
    if (this.authSubscription) {
      this.authSubscription.unsubscribe();
    }
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
  getUserInfoFromToken(): MyTokenPayload | undefined {
    const token = localStorage.getItem('token');
    if (token) {
      const decodedToken: MyTokenPayload = jwtDecode<MyTokenPayload>(token);
      return decodedToken;
    }
    return undefined;
  }


  toggleDropdown(): void {
    this.showDropdown = !this.showDropdown;
  }

  logout() {
    this.isLoggedIn = false;
    this.User = null;
    this.profilePictureUrl = null;
    this.appUserService.logout(); 
  }
  
}