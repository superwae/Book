import { Component, EventEmitter, Output } from '@angular/core';
import { DropDownSearchComponent } from '../drop-down-search/drop-down-search.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DropdownOption } from '../../Models/DropDownOption';

@Component({
  selector: 'app-modal-genre',
  standalone: true,
  imports: [DropDownSearchComponent,FormsModule,CommonModule,ReactiveFormsModule],
  templateUrl: './modal-genre.component.html',
  styleUrl: './modal-genre.component.css'
})
export class ModalGenreComponent {

  
  currentSelection: DropdownOption | null = null; 
  labels: DropdownOption[] = [];
  selectedImage: File | null = null;
  selectedImageUrl: string | null = null;
  showModal: boolean = true;
  @Output() closeEvent = new EventEmitter<void>();

  allOptions: DropdownOption[] = [
    { id: 1, name: 'History' },
    { id: 2, name: 'Romance' },
    { id: 3, name: 'Science Fiction' },
    { id: 4, name: 'Fantasy' },
    { id: 5, name: 'Thriller' },
    { id: 6, name: 'Young Adult' },
    { id: 7, name: 'Children' },
    { id: 8, name: 'Science' },
    { id: 9, name: 'Horror' },
    { id: 10, name: 'Nonfiction' },
    { id: 11, name: 'Health' },
    { id: 12, name: 'Travel' },
    { id: 13, name: 'Cooking' },
    { id: 14, name: 'Art' },
    { id: 15, name: 'Comics' },
    { id: 16, name: 'Religion' },
    { id: 17, name: 'Philosophy' },
    { id: 18, name: 'Education' },
    { id: 19, name: 'Politics' },
    { id: 20, name: 'Business' },
    { id: 21, name: 'Technology' },
    { id: 22, name: 'Sports' },
    { id: 23, name: 'True Crime' },
    { id: 24, name: 'Poetry' },
    { id: 25, name: 'Drama' },
    { id: 26, name: 'Adventure' },
    { id: 27, name: 'Nature' },
    { id: 28, name: 'Humor' },
    { id: 29, name: 'Lifestyle' },
    { id: 30, name: 'Economics' },
    { id: 31, name: 'Astronomy' },
    { id: 32, name: 'Linguistics' },
    { id: 33, name: 'Literature' },
    { id: 34, name: 'Short Story' },
    { id: 35, name: 'Novel' },
    { id: 36, name: 'Medicine' },
    { id: 37, name: 'Psychology' },
    { id: 38, name: 'Anime' },
  ];
  selectedOptions: number[] = [];


  addLabel(): void {
    if (this.labels.length >= 6) {
      alert('You can select at most 6 genres.');
      return;
    }

    if (this.currentSelection && !this.labels.includes(this.currentSelection)) {
      this.labels.push(this.currentSelection);
      this.currentSelection = null; 
    } else {
      alert('Please select a valid option not already added.');
    }
  }

  removeLabel(index: number): void {
    this.labels.splice(index, 1);
  }

  saveSelections(): void {
    if (this.labels.length < 1) {
      alert('Please select at least 1 genre.');
      return;
    }
    console.log('Selected options:', this.labels);
    
  }
  onOptionSelected(option: DropdownOption): void {
    this.currentSelection = option;
  }

  onFileSelected(event: Event) {
    const target = event.target as HTMLInputElement;
    const files = target.files;

    if (files && files.length) {
      const file = files[0];
      this.selectedImage = file;

      const reader = new FileReader();
      reader.onload = (e: ProgressEvent<FileReader>) => {
        this.selectedImageUrl = e.target?.result as string;

      };
      reader.readAsDataURL(file);
    }
  }
  removeSelectedImage() {
    this.selectedImage = null;
    this.selectedImageUrl = null;
  }

  toggleModal(): void {
    this.showModal = !this.showModal;
  }

  close() {
    this.showModal = false;
    this.closeEvent.emit();
  }

}


