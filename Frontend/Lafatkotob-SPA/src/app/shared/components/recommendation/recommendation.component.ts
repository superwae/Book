import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { Book } from '../../../Book/Models/bookModel';
import { BookService } from '../../../Book/Service/BookService';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-recommendation',
  standalone: true,
  imports: [CommonModule,RouterLink],
  templateUrl: './recommendation.component.html',
  styleUrls: ['./recommendation.component.css']
})
export class RecommendationComponent implements OnInit, AfterViewInit {
  books: Book[] = [];

  constructor(private bookService: BookService) { }

  ngOnInit(): void {
    this.bookService.getAllBooks().subscribe({
      next: (books: Book[]) => {
        // Adjust as necessary to match your API response structure
        this.books = books.slice(0, 8);
      },
      error: (err) => console.error('Error fetching books:', err)
    });
  }

  ngAfterViewInit(): void {
    this.initializeTrackScroll();
    this.setInitialImagePositions();
    this.addImagePressFeedback();
  }

  private addImagePressFeedback(): void {
    const images = document.querySelectorAll('#image-track img');
    images.forEach(img => {
      img.addEventListener('mousedown', (e) => {
        e.preventDefault(); // Prevent default to avoid any unwanted behavior
        img.classList.add('image-pressed');
      });
  
      // Ensure to remove the class on mouse up to revert the effect
      img.addEventListener('mouseup', () => {
        img.classList.remove('image-pressed');
      });
  
      // Consider touch devices as well
      img.addEventListener('touchstart', (e) => {
        e.preventDefault(); // Prevent default action
        img.classList.add('image-pressed');
      });
  
      img.addEventListener('touchend', () => {
        img.classList.remove('image-pressed');
      });
    });
  }

  private setInitialImagePositions(): void {
    const track = document.getElementById("image-track");
    if (track) {
      const images = Array.from(track.getElementsByClassName("image") as HTMLCollectionOf<HTMLElement>);
      images.forEach((image, index) => {
        // Set a custom attribute to store the initial right offset percentage
        const initialRightOffset = 100 - (index * 10); // Example offset, adjust as needed
        image.setAttribute('data-initial-right-offset', `${initialRightOffset}%`);
        image.style.objectPosition = `${initialRightOffset}% center`;
      });
    }
  }
  
  private initializeTrackScroll(): void {
    const track = document.getElementById("image-track");
  
    const handleOnDown = (e: MouseEvent | TouchEvent) => {
      const clientX = e instanceof MouseEvent ? e.clientX : e.touches[0].clientX;
      if (track) track.setAttribute('data-mouse-down-at', String(clientX));
    };
  
    const handleOnUp = () => {
      if (!track) return; // Early return if track is null
    
      // Get the current percentage from 'data-percentage' attribute
      const currentPercentage = track.getAttribute('data-percentage') || "0";
    
      // Update 'data-prev-percentage' with the current percentage
      track.setAttribute('data-prev-percentage', currentPercentage);
    
      // Reset 'data-mouse-down-at' to "0"
      track.setAttribute('data-mouse-down-at', "0");
    };
    
  
    const handleOnMove = (e: MouseEvent | TouchEvent) => {
      if (!track) return;
    
      const mouseDownAt = parseFloat(track.getAttribute('data-mouse-down-at') || "0");
      if (mouseDownAt === 0) return;
    
      const clientX = e instanceof MouseEvent ? e.clientX : e.touches[0].clientX;
      const mouseDelta = mouseDownAt - clientX;
      const maxDelta = window.innerWidth / 2;
      const percentage = (mouseDelta / maxDelta) * -100;
      const prevPercentage = parseFloat(track.getAttribute('data-prev-percentage') || "0");
      const nextPercentage = Math.max(Math.min(prevPercentage + percentage, 0), -100);
    
      track.setAttribute('data-percentage', String(nextPercentage));
      track.style.transform = `translate(${nextPercentage}%, -50%)`;
    
      const images = Array.from(track.getElementsByClassName("image") as HTMLCollectionOf<HTMLElement>);
      images.forEach((image) => {
        const initialRightOffset = parseFloat(image.getAttribute('data-initial-right-offset') || "100");
        // Calculate the adjusted position considering the initial offset and current scroll
        const adjustedPosition = initialRightOffset + nextPercentage;
        image.style.objectPosition = `${adjustedPosition}% center`;
      });
    };
    
    
    
    
  
    window.addEventListener('mousedown', handleOnDown);
    window.addEventListener('mouseup', handleOnUp);
    window.addEventListener('mousemove', handleOnMove);
    window.addEventListener('touchstart', handleOnDown as any);
    window.addEventListener('touchend', handleOnUp as any);
    window.addEventListener('touchmove', handleOnMove as any);
  }
  
}