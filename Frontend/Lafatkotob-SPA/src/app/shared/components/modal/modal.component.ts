import { CommonModule } from '@angular/common';
import { Component ,EventEmitter,Input,Output} from '@angular/core';
import { ModaleService } from '../../Service/modal.service';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.css'
})
export class ModalComponent {
  @Input() title: string = '';
  @Input() show: boolean = false;
  @Output() closeEvent = new EventEmitter<void>();
  selectedImage: File | null = null;
  selectedImageUrl: string | null = null;

  constructor(private modalService: ModaleService) {}

  onFileSelected(event: Event) {
    const target = event.target as HTMLInputElement;
    const files = target.files;

    if (files && files.length) {
      const file = files[0];
      this.selectedImage = file;

      // Display the selected image by creating a URL for it
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

 

  close() {
    this.show = false;
    this.modalService.setShowModal(false);
    this.closeEvent.emit();
  }
}

