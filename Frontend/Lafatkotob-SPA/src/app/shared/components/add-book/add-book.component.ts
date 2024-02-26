import { Component, OnInit } from '@angular/core';
import { BookService } from '../../../Book/Service/BookService';
import { CommonModule } from '@angular/common';
import { ModalComponent } from '../modal/modal.component';
import { ModaleService } from '../../Service/modal.service';

@Component({
  selector: 'app-add-book',
  standalone: true,
  imports: [CommonModule,ModalComponent],
  templateUrl: './add-book.component.html',
  styleUrl: './add-book.component.css'
})
export class AddBookComponent   {
  showModal: boolean = false;
  constructor(private modalService: ModaleService) {
  }

  openRegisterBookModal() {
    this.showModal = true;
    this.modalService.setShowModal(true);
    }
}
