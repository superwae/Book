import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-filter',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.css'
})
export class FilterComponent {
  genres: { id: number, name: string }[] = [
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
  
  selectedGenreId: number | null = null;
  @Output() genreSelect = new EventEmitter<number | null>();

  selectGenre(genreId: number): void {
    this.selectedGenreId = this.selectedGenreId === genreId ? null : genreId;
    this.genreSelect.emit(this.selectedGenreId);
  }

  isActive(genreId: number): boolean {
    return this.selectedGenreId === genreId;
  }
}
