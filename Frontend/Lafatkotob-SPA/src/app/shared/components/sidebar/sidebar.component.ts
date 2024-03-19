import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ModaleService } from '../../Service/ModalService/modal.service';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink,CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit ,OnDestroy {
show: boolean = true;
  username: string = "";
  private subscription: Subscription = new Subscription();
  constructor(
    private route: ActivatedRoute,
    private modalService:ModaleService
    ) { }

  ngOnInit(): void {

    this.subscription.add(this.modalService.showModal$.subscribe(visible => {
      if (!visible) {
        this.show = true;
      } else {
        this.show = false;
      }
    }));

    this.adjustSidebarPosition();
    
    if (this.route.parent) {
      this.route.parent.params.subscribe(params => {
        this.username = params['username'];
      });
    }
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
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
      setTimeout(() => this.adjustSidebarPosition(), 100);
      sidebar.setAttribute('style', `margin-top: ${navbarHeight - 65}px;`);
    }
    if (sidebar2) {
      setTimeout(() => this.adjustSidebarPosition(), 100);
      sidebar2.setAttribute('style', `margin-top: ${navbarHeight - 65}px;`);
    }
  }
}