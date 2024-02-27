import { Component, OnInit } from '@angular/core';
import { BookService } from '../../../Book/Service/BookService';
import { CommonModule } from '@angular/common';
import { ModalComponent } from '../modal/modal.component';
import { ModaleService } from '../../Service/ModalService/modal.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-book',
  standalone: true,
  imports: [CommonModule,ModalComponent],
  templateUrl: './add-book.component.html',
  styleUrl: './add-book.component.css'
})
export class AddBookComponent   {
  showModal: boolean = false;
  constructor(private modalService: ModaleService,private router: Router) {
  }
  shouldHideButton(): boolean {
    
    const currentRoute = this.router.url;
    return ['/login', '/forgot-password', '/reset-password'].includes(currentRoute) || this.showModal;
    }

  openRegisterBookModal() {
    this.showModal = true;
    this.modalService.setShowModal(true);
    
    }
    closeModal() {
      this.showModal = false;
      this.modalService.setShowModal(false);
    }
}
