import { Component, HostListener, OnInit } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit {

  constructor() {}

  ngOnInit(): void {
    this.adjustSidebarPosition();
  }

  @HostListener('window:scroll', ['$event'])
  onScroll(event: any) {
    this.adjustSidebarPosition();
  }

  adjustSidebarPosition() {
    // Obtain the current height of the navbar
    const navbarHeight = document.querySelector('.nav')?.clientHeight || 0;
    
    // Adjust the top margin of the sidebar to match the navbar's height
    const sidebar = document.querySelector('.s-sidebar__nav');
    const sidebar2 = document.querySelector('.s-sidebar__trigger');

    if (sidebar) {
      sidebar.setAttribute('style', `margin-top: ${navbarHeight-60}px;`);
    }
    if (sidebar2) {
      sidebar2.setAttribute('style', `margin-top: ${navbarHeight-60}px;`);
    }
  }
}