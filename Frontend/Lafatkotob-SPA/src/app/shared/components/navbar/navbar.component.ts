import { CommonModule } from '@angular/common';
import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { Router, Event as RouterEvent, NavigationEnd, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { ModaleService } from '../../Service/ModalService/modal.service';
import { AppUsereService } from '../../../Auth/services/app-user.service';
import { AppUserModel } from '../../../Auth/Models/AppUserModel';
import { MyTokenPayload } from '../../Models/MyTokenPayload';
import { jwtDecode } from 'jwt-decode';
import { TooltipDirective } from '../../directives/tooltip.directive';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  standalone: true,
  imports: [CommonModule, RouterLink, TooltipDirective]
})
export class NavbarComponent implements OnInit, OnDestroy {
  isAffix: boolean = false;
  navExpanded: boolean = false;
  showMenu: boolean = true;
  showModal: boolean = false;
  isLoggedIn: boolean = false;
  profilePictureUrl: string | null = null;
  User: AppUserModel | null = null;
  showDropdown = false;
  private _keepDropdownOpen = false;
  showChatDropdown: boolean = false;
  showNotificationsDropdown: boolean = false;
  showEventsDropdown: boolean = false;
  private hideDropdownTimeout?: any;
  private subscription: Subscription = new Subscription();
  private authSubscription: Subscription = new Subscription();
  isProfilePage: boolean = false;
  prevScrollpos = window.pageYOffset;


  constructor
    (
      private router: Router,
      private modalService: ModaleService,
      private appUserService: AppUsereService
    ) {
    this.router.events.pipe(
      filter((event: RouterEvent): event is NavigationEnd => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.showMenu = event.urlAfterRedirects !== '/login';
    });

    this.router.events.pipe(
      filter((event: RouterEvent): event is NavigationEnd => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      if (event.urlAfterRedirects.startsWith('/user')) {
        this.isProfilePage = true
        this.transformNavbar();
      } else {
        this.reverseNavbarTransformation();
        this.isProfilePage = false;

      }
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
    const currentScrollPos = window.pageYOffset;
    if (this.prevScrollpos > currentScrollPos) 
    {
      this.isAffix = true; // Show navbar when scrolling up
      this.showMenu=true;

    } else {
      this.isAffix = false; 
      this.showMenu = false;

    }
    
    this.prevScrollpos = currentScrollPos;
  
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const chatDropdownElement = document.querySelector('.chat-dropdown-content');
    const notificationsDropdownElement = document.querySelector('.notifications-dropdown-content');
    const eventsDropdownElement = document.querySelector('.events-dropdown-content');

    const chatTriggerElement = document.querySelector('.chat-dropdown-trigger');
    const notificationsTriggerElement = document.querySelector('.notifications-dropdown-trigger');
    const eventsTriggerElement = document.querySelector('.events-dropdown-trigger');

    // Checking and closing the Profile Dropdown
    const dropdownElement = document.querySelector('.dropdown-content');
    const triggerElement = document.querySelector('.profile-dropdown');

    if (dropdownElement && triggerElement && !dropdownElement.contains(event.target as Node) && !triggerElement.contains(event.target as Node)) {
      this.showDropdown = false;
    }

    // Checking and closing the Chat Dropdown
    if (chatDropdownElement && chatTriggerElement && !chatDropdownElement.contains(event.target as Node) && !chatTriggerElement.contains(event.target as Node)) {
      this.showChatDropdown = false;
    }

    // Checking and closing the Notifications Dropdown
    if (notificationsDropdownElement && notificationsTriggerElement && !notificationsDropdownElement.contains(event.target as Node) && !notificationsTriggerElement.contains(event.target as Node)) {
      this.showNotificationsDropdown = false;
    }

    // Checking and closing the Events Dropdown
    if (eventsDropdownElement && eventsTriggerElement && !eventsDropdownElement.contains(event.target as Node) && !eventsTriggerElement.contains(event.target as Node)) {
      this.showEventsDropdown = false;
    }
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

  toggleDropdown() {
    console.log("Toggle dropdown called");
    this.showDropdown = !this.showDropdown;
  }


  toggleChatDropdown(event: MouseEvent): void {
    event.stopPropagation();
    this.showChatDropdown = !this.showChatDropdown;

    if (this.showChatDropdown) {
      this.showNotificationsDropdown = false;
      this.showEventsDropdown = false;
      this.showDropdown = false;
    }
  }

  toggleNotificationsDropdown(event: MouseEvent): void {
    event.stopPropagation();
    this.showNotificationsDropdown = !this.showNotificationsDropdown;

    // Close other dropdowns
    if (this.showNotificationsDropdown) {
      this.showChatDropdown = false;
      this.showEventsDropdown = false;
      this.showDropdown = false;
    }
  }

  toggleEventsDropdown(event: MouseEvent): void {
    event.stopPropagation();
    this.showEventsDropdown = !this.showEventsDropdown;

    // Close other dropdowns
    if (this.showEventsDropdown) {
      this.showChatDropdown = false;
      this.showNotificationsDropdown = false;
      this.showDropdown = false;
    }
  }

  transformNavbar() {

    const navbarElement = document.querySelector('.nav');
    if (navbarElement) {
      navbarElement.classList.add('affix');
    }
  }
  reverseNavbarTransformation() {
    const navbarElement = document.querySelector('.nav');
    if (navbarElement) {
      navbarElement.classList.remove('affix');
    }
  }

  logout() {
    this.isLoggedIn = false;
    this.User = null;
    this.profilePictureUrl = null;
    this.appUserService.logout();
  }

}